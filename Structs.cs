using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public struct TextureParams{
	public PixelFormat imageFormat;
	
	public TextureWrapMode wrapS;
	public TextureWrapMode wrapT;
	public Vector3 borderColor;
	
	public TextureMinFilter filterMin;
	public TextureMagFilter filterMax;
	
	public TextureParams(){ //Constructor for setting defaults
		this.imageFormat = PixelFormat.Rgba;
		
		this.wrapS = TextureWrapMode.Repeat;
		this.wrapT = TextureWrapMode.Repeat;
		this.borderColor = new Vector3(0f, 0f, 0f);
		
		this.filterMin = TextureMinFilter.NearestMipmapNearest;
		this.filterMax = TextureMagFilter.Nearest;
	}
}

public struct Fog{
	public bool applies;
	public Vector3 color;
	public float start;
	public float end;
	
	public Fog(bool a, Vector3 c, float s, float e){
		this.applies = a;
		this.color = c;
		this.start = s;
		this.end = e;
	}
}

public struct PointLight{
	public Vector3 position;
	public Vector3 color;
	
	public float ambientStrength;
	public float diffuseStrength;
	public float specularStrength;
	
	public bool attenuationApplies;
	
	public float attQuadratic;
	public float attLinear;
	public float attConstant;
	
	public PointLight(Vector3 p, Vector3 c){
		this.position = p;
		this.color = c;
		this.attenuationApplies = false;
		this.ambientStrength = 0.1f;
		this.diffuseStrength = 1f;
		this.specularStrength = 0.5f;
	}
	
	public PointLight(Vector3 p, Vector3 c, float q, float l, float con){
		this.position = p;
		this.color = c;
		this.attenuationApplies = true;
		this.attQuadratic = q;
		this.attLinear = l;
		this.attConstant = con;
		this.ambientStrength = 0.1f;
		this.diffuseStrength = 1f;
		this.specularStrength = 0.5f;
	}
	
	public PointLight(Vector3 p, Vector3 c, float q, float l, float con, float a, float d, float s){
		this.position = p;
		this.color = c;
		this.attenuationApplies = true;
		this.attQuadratic = q;
		this.attLinear = l;
		this.attConstant = con;
		this.ambientStrength = a;
		this.diffuseStrength = d;
		this.specularStrength = s;
	}
}

public struct DirLight{
	public Vector3 direction;
	public Vector3 color;
	
	public float ambientStrength;
	public float diffuseStrength;
	public float specularStrength;
	
	public DirLight(Vector3 p, Vector3 c){
		this.direction = p;
		this.color = c;
		this.ambientStrength = 0.1f;
		this.diffuseStrength = 1f;
		this.specularStrength = 0.5f;
	}
	
	public DirLight(Vector3 p, Vector3 c, float a, float d, float s){
		this.direction = p;
		this.color = c;
		this.ambientStrength = a;
		this.diffuseStrength = d;
		this.specularStrength = s;
	}
}

public struct Scene{
	public Instantiation[] i;
	
	public PointLight[] p;
	public DirLight dl;
	
	public TextInstantiation[] t;
	
	public Vector3 backgroundColor;
	
	public Fog fog;
	
	public bool allowMovement;
	public bool allowCameraMovement;
	public float movementSpeed;
	public Vector3 startingPos;
	public float startingYaw;
	public float startingPitch;
	
	public float fov;
	public float nearPlane;
	public float farPlane;
	
	public bool allowConsoleCommands;
	
	public string name;
	public string author;
	public string description;
	
	public bool fullscreened;
	public int targetFPS;
	
	public Scene(){ //Constructor for defaults
		this.backgroundColor = new Vector3(0f, 0f, 0f);
		
		this.fog.applies = true;
		this.fog.color = new Vector3(0f, 0f, 0f);
		this.fog.start = 25f;
		this.fog.end = 100f;	
		
		this.allowMovement = true;
		this.allowCameraMovement = true;
		this.movementSpeed = 5f;
		this.startingPos = new Vector3(0f, 0f, 0f);
		this.startingYaw = 0f;
		this.startingPitch = 0f;
		
		this.fov = 45f;
		this.nearPlane = 0.1f;
		this.farPlane = 100f;
		
		this.allowConsoleCommands = true;
		
		this.name = "default_name";
		this.author = "default_author";
		this.description = "default_desc";
		
		this.fullscreened = false;
		this.targetFPS = 144;
	}
}