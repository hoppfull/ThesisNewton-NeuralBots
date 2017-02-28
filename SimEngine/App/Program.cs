using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using OpenTK.Input;

namespace App {
    class Program {
        static void Main(string[] args) {
            using (var w = new MyWindow(10, 10, 10)) {
                w.Run(30.0);
            }
        }
    }
    class MyWindow : GameWindow {
        private double[,] CreaturePositions;
        private double[,] CreatureVelocities;
        private double[] CreatureThrusts;
        private double[] CreatureDirections;
        private double[] CreatureHealths;
        private int[] CreatureActions; //TODO: Fix actiontype
        private short[,,] CreatureChromosomePairs;
        private float[,] CreatureColors;
        private double[,] FoodPositions;
        private bool[] FoodExists;
        private double ActiveAreaRadius;
        private Random rng = new Random();
        private int NCreatures;
        private int NFoods;
        private int VBO;
        private int IBO;
        private int ShaderProgramId;
        private int cameraPos_Uniform;
        private float[] CameraPosition;



        public MyWindow(int NCreatures, int NFoods, double ActiveAreaRadius) {
            Title = "hello window";

            CreaturePositions = new double[NCreatures, 2];
            CreatureVelocities = new double[NCreatures, 2];
            CreatureThrusts = new double[NCreatures];
            CreatureDirections = new double[NCreatures];
            CreatureHealths = new double[NCreatures];
            CreatureActions = new int[NCreatures];
            CreatureChromosomePairs = new short[NCreatures, 2, 0]; //TODO: Fix gene count
            CreatureColors = new float[NCreatures, 3]; //TODO: Fix color generation
            FoodPositions = new double[NFoods, 2];
            FoodExists = new bool[NFoods];
            this.ActiveAreaRadius = ActiveAreaRadius;
            this.NCreatures = NCreatures;
            this.NFoods = NFoods;
            CameraPosition = new float[2];
            SpawnDeathBehaviour(); //TODO: Fix temporary solution
        }


        protected override void OnKeyDown(KeyboardKeyEventArgs e) {
            base.OnKeyDown(e);
            float camSpeed = 0.1f;
            if (e.Key == Key.Left) CameraPosition[0] -= camSpeed;
            if (e.Key == Key.Right) CameraPosition[0] += camSpeed;
            if (e.Key == Key.Down) CameraPosition[1] += camSpeed;
            if (e.Key == Key.Up) CameraPosition[1] -= camSpeed;
            if (e.Key == Key.BackSpace) {
                CameraPosition[0] = 0;
                CameraPosition[1] = 0;
            }
        }

        private void SpawnDeathBehaviour() {

            for (int i = 0; i < NFoods; i++) {
                if (!FoodExists[i]) {
                    double d = rng.NextDouble() * ActiveAreaRadius;
                    FoodPositions[i, 0] = d * Math.Cos(rng.NextDouble());
                    FoodPositions[i, 1] = d * Math.Sin(rng.NextDouble());
                    FoodExists[i] = true;
                }
            }

            for (int i = 0; i < NCreatures; i++) {
                if (CreatureHealths[i] <= 0) {
                    CreatureHealths[i] = 1;
                    double d = rng.NextDouble() * ActiveAreaRadius;
                    CreaturePositions[i, 0] = d * Math.Cos(rng.NextDouble());
                    CreaturePositions[i, 1] = d * Math.Sin(rng.NextDouble());
                    CreatureVelocities[i, 0] = 0;
                    CreatureVelocities[i, 1] = 0;
                    CreatureThrusts[i] = 0;
                    CreatureDirections[i] = rng.Next();
                    //CreatureChromosomePairs[i,0,?]
                    //CreatureChromosomePairs[i,1,?]
                }
            }
        }

        private class Node {
            public double[,] Positions;
        }

        private Node GenTree(double[,] ps) {
            return new Node() { Positions = ps };
        }

        private double[,] SearchTree(Node tree, double t, double b, double l, double r) {
            double[,] result;
            int nResults = 0;

            for (int i = 0; i < tree.Positions.Length; i++) {
                double posX = tree.Positions[i, 0];
                double posY = tree.Positions[i, 1];
                if (posX >= l && posX <= r && posY <= t && posY >= b) nResults++;
            }

            result = new double[nResults, 2];
            int j = 0;
            for (int i = 0; i < tree.Positions.Length; i++) {
                double posX = tree.Positions[i, 0];
                double posY = tree.Positions[i, 1];
                if (posX >= l && posX <= r && posY <= t && posY >= b) {
                    result[j, 0] = posX;
                    result[j, 1] = posY;
                    j++;
                }
            }
            return result;
        }

        protected override void OnLoad(EventArgs e) {
            GL.ClearColor(1, 0, 0, 0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.CullFace(CullFaceMode.Back);

            float[] vertices = new float[] {
                -1, 1,
                -1,-1,
                 1,-1,
                 1, 1
            };

            int[] indices = new int[] {
                0, 1, 2,
                2, 3, 0
            };

            VBO = GL.GenBuffer();
            IBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0 * sizeof(float));
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            {
                string vsSource = File.ReadAllText("res/sprite_vs.glsl");
                string fsSource = File.ReadAllText("res/sprite_fs.glsl");
                int vs_id = GL.CreateShader(ShaderType.VertexShader);
                int fs_id = GL.CreateShader(ShaderType.FragmentShader);

                GL.ShaderSource(vs_id, vsSource);
                GL.ShaderSource(fs_id, fsSource);

                GL.CompileShader(vs_id);
                GL.CompileShader(fs_id);
                {
                    int status = 0;
                    GL.GetShader(vs_id, ShaderParameter.CompileStatus, out status);
                    if (status == 0) throw new Exception($"Vertex shader error: {GL.GetShaderInfoLog(vs_id)}");
                    GL.GetShader(fs_id, ShaderParameter.CompileStatus, out status);
                    if (status == 0) throw new Exception($"Fragment shader error: {GL.GetShaderInfoLog(fs_id)}");
                }

                ShaderProgramId = GL.CreateProgram();
                GL.AttachShader(ShaderProgramId, vs_id);
                GL.AttachShader(ShaderProgramId, fs_id);
                GL.LinkProgram(ShaderProgramId);

                {
                    int status = 0;
                    GL.GetProgram(ShaderProgramId, GetProgramParameterName.LinkStatus, out status);
                    if (status == 0) throw new Exception($"Shader program error: {GL.GetProgramInfoLog(ShaderProgramId)}");
                }

                GL.DetachShader(ShaderProgramId, vs_id);
                GL.DetachShader(ShaderProgramId, fs_id);

                GL.DeleteShader(vs_id);
                GL.DeleteShader(fs_id);
            }
            cameraPos_Uniform = GL.GetUniformLocation(ShaderProgramId, "cameraPos");
            if (cameraPos_Uniform == -1) throw new Exception("Failed to get Uniform!");
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {

        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(ShaderProgramId);
            GL.Uniform2(cameraPos_Uniform, CameraPosition[0], CameraPosition[1]);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);
            GL.UseProgram(0);

            SwapBuffers();
        }
    }
}
