#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTex;
layout (location = 2) in vec3 aNor;

out vec2 TexCoord; //Texture coordinates
out float depth; //Depth for fog calculations
out vec3 Normal; //Normal for light calculations
out vec3 FragPos; //Fragment position

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform mat3 normal;

void main()
{
	gl_Position = projection * view * model * vec4(aPos, 1.0); //The position
	
	depth = gl_Position.z; //The depth
	TexCoord = aTex; //The texture coordinates
	Normal = normal * aNor; 
	FragPos = vec3(model * vec4(aPos, 1.0));
}