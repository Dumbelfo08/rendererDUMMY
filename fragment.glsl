#version 330 core

#define MAX_LIGHTS 32
#define MAX_SPOTLIGHTS 32

//STRUCTS

struct Material{
	vec3 tint;
	sampler2D tex;
	
	vec3 specularColor;
	float shininess;
	
	int fogApplies;
	
	int mode;
};

struct PointLight{
	vec3 color;
	vec3 position;
	
	float ambientStrength;
	float diffuseStrength;
	float specularStrength;
	
	int attenuationApplies;
	
	float attQuadratic;
	float attLinear;
	float attConstant;
};

struct DirLight{
	vec3 color;
	vec3 direction;
	
	float ambientStrength;
	float diffuseStrength;
	float specularStrength;
};

struct SpotLight{
	vec3 color;
	vec3 direction;
	
	float innerCutoff;
	float outerCutoff;
	
	float ambientStrength;
	float diffuseStrength;
	float specularStrength;
	
	int attenuationApplies;
	
	float attQuadratic;
	float attLinear;
	float attConstant;
};

struct Fog{
	vec3 color;
	float start;
	float end;
};

//OUTS

out vec4 fragColor;

//INS

in vec2 TexCoord;
in float depth;
in vec3 Normal;
in vec3 FragPos;

//UNIFORMS

uniform PointLight[MAX_LIGHTS] lights;
uniform DirLight dirLight;
uniform SpotLight[MAX_SPOTLIGHTS] spotLights;

uniform Fog fog;

uniform Material mat;

uniform vec3 viewPos;

//METHODS

float calculateFogFactor(){
	float fogFactor = float(mat.fogApplies) * clamp((depth - fog.start) / (fog.end - fog.start), 0.0, 1.0);
	return fogFactor;
}

vec4 calculatePhongDirection(vec4 col){
	vec4 ambient = dirLight.ambientStrength * vec4(dirLight.color, 1.0) * col;
	
	vec3 lightDir = normalize(-dirLight.direction);
    float diff = max(dot(normalize(Normal), lightDir), 0.0);
	
	vec4 diffuse = dirLight.diffuseStrength * diff * vec4(dirLight.color, 1.0) * col;
	
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, normalize(Normal));
	
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), mat.shininess);
	vec4 specular = dirLight.specularStrength * vec4(mat.specularColor, 1.0) * spec * vec4(dirLight.color, 1.0);
	
	return ambient + specular + diffuse;
}

vec4 calculatePhongPoint(int i, vec4 col){
	vec4 ambient = lights[i].ambientStrength * vec4(lights[i].color, 1.0) * col;
	
	vec3 lightDir = normalize(lights[i].position - FragPos);
    float diff = max(dot(normalize(Normal), lightDir), 0.0);
	
	vec4 diffuse = lights[i].diffuseStrength * diff * vec4(lights[i].color, 1.0) * col;
	
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, normalize(Normal));
	
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), mat.shininess);
	vec4 specular = lights[i].specularStrength * vec4(mat.specularColor, 1.0) * spec * vec4(lights[i].color, 1.0);
	
	if(lights[i].attenuationApplies == 1){
		float distance = length(lights[i].position - FragPos);
		float attenuation = 1.0 / (lights[i].attConstant + lights[i].attLinear * distance + lights[i].attQuadratic * (distance * distance));  
		
		ambient  *= attenuation; 
		diffuse  *= attenuation;
		specular *= attenuation;  
	}
	
	return ambient + specular + diffuse;
}

//MAIN

void main()
{		
	vec4 col = vec4(0.0);
	switch (mat.mode){
		case 0:
			col = vec4(mat.tint, 1.0);
			break;
		case 1:
			for(int i; i < MAX_LIGHTS; i++){
				col += calculatePhongPoint(i, vec4(mat.tint, 1.0));
			}
			col += calculatePhongDirection(vec4(mat.tint, 1.0));
			break;
		case 2:
			col = texture(mat.tex, TexCoord);
			break;
		case 3:
			for(int i; i < MAX_LIGHTS; i++){
				col += calculatePhongPoint(i, texture(mat.tex, TexCoord));
			}
			col += calculatePhongDirection(texture(mat.tex, TexCoord));
			break;
		case 4:
			col = texture(mat.tex, TexCoord) * vec4(mat.tint, 1.0);
		case 5:
			for(int i; i < MAX_LIGHTS; i++){
				col += calculatePhongPoint(i, texture(mat.tex, TexCoord) * vec4(mat.tint, 1.0));
			}
			col += calculatePhongDirection(texture(mat.tex, TexCoord) * vec4(mat.tint, 1.0));
			break;
		default:
			col = vec4(0.5, 1.0, 0.5, 1.0);
			break;
	}
	
	vec4 final = mix(col, vec4(fog.color, 1.0), calculateFogFactor());
    fragColor = final;
} 