#version 330 core

layout (location = 0) in vec2 pos;

uniform vec2 cameraPos;

void main(void){
    gl_Position = vec4((pos *0.5)- cameraPos, 0, 1);
}