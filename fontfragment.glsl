#version 330 core
out vec4 fragColor;

in vec2 textureCoords;

uniform sampler2D fontTexture;

void main()
{	
	vec4 col = texture(fontTexture, textureCoords); //Apply the coordinates passed from the vertex shader
	
	if(col.a < 0.05) //Discard fragment if it is completely transparent
        discard;
		
	fragColor = col;
} 