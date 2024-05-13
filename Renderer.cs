using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using AshLib;

public class Renderer{
	public int height = Seagull.startWidth;
	public int width = Seagull.startHeight;
	
	private Seagull sg;
	private ResourceManager res;
	public Camera cam;
	public FontRenderer fr;
	
	private Matrix4 projection;
	
	//===========SCENE-PROPERTIES==============
	
	public Color4 backgroundColor;
	
	public Fog fog;
	
	public float fov;
	public float nearPlane;
	public float farPlane;
	
	public string name;
	public string author;
	public string description;
	
	public Instantiation[] ins;
	public TextInstantiation[] texIns;
	
	public PointLight[] poiLig;
	public DirLight dirLig;
	
	public Renderer(Seagull sea){ //init
		this.sg = sea;
		this.res = this.sg.res;
		this.cam = new Camera(this.sg);
		this.fr = new FontRenderer("fontShader", "font", this.sg, 16, 16);
		
		this.initializeDefaultSceneValues();
		
		this.updateProjection();
		
		this.initializeModels();
		this.initializeShaders();
		this.initializeTextures();
		this.loadScene(this.initializeScene());
	}
	
	private void initializeDefaultSceneValues(){
		this.backgroundColor = new Color4(0f, 0f, 0f, 1f);
		
		this.fog = new Fog(true, new Vector3(0f), 25f, 100f);
		
		this.fov = 45f;
		this.nearPlane = 0.1f;
		this.farPlane = 100f;
	}
	
	private void initializeModels(){
		float[] vertices = {
			-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
			0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
			0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
		
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
			0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
			-0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
		
			-0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
			-0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
			-0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
		
			0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
			0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
		
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
			0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
			0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
			0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
			-0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
		
			-0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
			0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
			0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
			-0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
			-0.5f,  0.5f, -0.5f,  0.0f, 1.0f
		};
		
		float[] v = {
			-1f, 1f, 0f, 0f, 100f,
			1f, 1f, 0f, 100f, 100f,
			-1f, -1f, 0f, 0f, 0f,
			
			1f, 1f, 0f, 100f, 100f,
			1f, -1f, 0f, 100f, 0f,
			-1f, -1f, 0f, 0f, 0f
		};
		
		float[] v2 = {
			-1f, 1f, 0f, 0f, 1f,
			1f, 1f, 0f, 1f, 1f,
			-1f, -1f, 0f, 0f, 0f,
			
			1f, 1f, 0f, 1f, 1f,
			1f, -1f, 0f, 1f, 0f,
			-1f, -1f, 0f, 0f, 0f
		};
		
		float[] pyr = new float[]
		{
			// Base vertices (bottom square)
			// Position         Texture Coordinates
			-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,  // Vertex 0
			0.5f, -0.5f, -0.5f,  1.0f, 0.0f,  // Vertex 1
			0.5f, -0.5f,  0.5f,  1.0f, 1.0f,  // Vertex 2
		
			0.5f, -0.5f,  0.5f,  1.0f, 1.0f,  // Vertex 2
			-0.5f, -0.5f,  0.5f,  0.0f, 1.0f,  // Vertex 3
			-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,  // Vertex 0
		
			// Side faces (triangles connecting base to apex)
			// Position         Texture Coordinates
			// Front-left triangle
			-0.5f, -0.5f, -0.5f,  0.0f, 0.0f,  // Vertex 0 (base)
			0.5f, -0.5f, -0.5f,  0.5f, 0.0f,  // Vertex 1 (base)
			0.0f,  0.5f,  0.0f,  0.25f, 1.0f,  // Vertex 4 (apex)
		
			// Front-right triangle
			0.5f, -0.5f, -0.5f,  0.0f, 0.0f,  // Vertex 1 (base)
			0.5f, -0.5f,  0.5f,  1.0f, 0.0f,  // Vertex 2 (base)
			0.0f,  0.5f,  0.0f,  0.5f, 1.0f,  // Vertex 4 (apex)
		
			// Back-right triangle
			0.5f, -0.5f,  0.5f,  0.5f, 0.0f,  // Vertex 2 (base)
			-0.5f, -0.5f,  0.5f,  1.0f, 0.0f,  // Vertex 3 (base)
			0.0f,  0.5f,  0.0f,  0.75f, 1.0f,  // Vertex 4 (apex)
		
			// Back-left triangle
			-0.5f, -0.5f,  0.0f,  0.8f, 0.0f,  // Vertex H (base)
			-0.5f, -0.5f, -0.5f,  1.0f, 0.0f,  // Vertex 0 (base)
			0.0f,  0.5f,  0.0f,  0.9f, 1.0f,  // Vertex 4 (apex)
			
			-0.5f, -0.5f,  0.0f,  0.2f, 0.0f,  // Vertex H (base)
			-0.5f, -0.5f,  0.5f,  0.0f, 0.0f,  // Vertex 3 (base)
			0.0f,  0.5f,  0.0f,  0.1f, 1.0f   // Vertex 4 (apex)
		};
		
		res.generateModel("cuadro", Model.calculateNormals(vertices, 5), 8, "323");
		res.generateModel("plano", Model.calculateNormals(v, 5), 8, "323");
		res.generateModel("plano2", Model.calculateNormals(v2, 5), 8, "323");
		res.generateModel("pyramid", Model.calculateNormals(pyr, 5), 8, "323");
	}
	
	private void initializeShaders(){
		res.generateShaderFile("fontShader", "fontvertex.glsl", "fontfragment.glsl",null);
		res.generateShaderFile("mShader", "vertex.glsl", "fragment.glsl",null);
		res.generateShaderFile("mainShader", "vertex.glsl", "fragment.glsl",null);
	}
	
	private void initializeTextures(){
		TextureParams t = new TextureParams();
		t.filterMin = TextureMinFilter.NearestMipmapLinear;
		t.filterMax = TextureMagFilter.Linear;
		res.generateTextureFile("roanoke", "irak.png", t);
		res.generateTextureFile("mor", "Untitled.png", new TextureParams());
		res.generateTextureFile("ora", "ora.png", new TextureParams());
		res.generateTextureFile("floor", "floor.png", new TextureParams());
		res.generateTextureFile("obama", "obama.png", t);
		res.generateTextureFile("tib", "tibu.jpg", t);
		res.generateTextureFile("font", "font.png", new TextureParams());
		res.generateTextureFile("awawa", "awawa.png", new TextureParams());
		res.generateTextureFile("light", "light.png", new TextureParams());
	}
	
	private Scene initializeScene(){
		Scene sce = new Scene();
		sce.backgroundColor = new Vector3(0.2f, 0.2f, 0.3f);
		sce.fog = new Fog(true, new Vector3(0.2f, 0.2f, 0.3f), 15f, 60f);
		sce.allowMovement = true;
		sce.allowCameraMovement = true;
		sce.movementSpeed = 8f;
		sce.startingPos = new Vector3(0f, 0f, 3f);
		sce.fov = 45f;
		sce.nearPlane = 0.1f;
		sce.farPlane = 100f;
		
		sce.name = "Developer Gallery";
		
		sce.fullscreened = false;
		sce.targetFPS = 144;
		
		Instantiation[] i = new Instantiation[16];
		
		i[0] = new Instantiation("cuadro", "mor", new Vector3(0f,0f,-2f), new Vector3(30f, 0f, 0f));
		i[1] = new Instantiation("cuadro", "roanoke", 3, false, new Vector3(2f,1f,1f), new Vector3(-0f, -1f, -2f), new Vector3(20f, 0f, 0f));
		i[2] = new Instantiation("cuadro", "mor", new Vector3(0f, 7f, -6f), new Vector3(0f, 0f, 0f));
		i[3] = new Instantiation("cuadro", "ora", new Vector3(0.5f, 2f, 1f), new Vector3(10f, 0f, 12f), new Vector3(0f, 60f, 0f));
		i[4] = new Instantiation("plano", "floor", new Vector3(100f), new Vector3(0f, -3f, 0f), new Vector3(90f, 0f, 0f));
		i[5] = new Instantiation("cuadro", "ora", 3, false, new Vector3(1f), new Vector3(1f), new Vector3(0f, 0f, 0f));
		i[6] = new Instantiation("pyramid", "obama", new Vector3(2f), new Vector3(0f, 4f, 20f), new Vector3(0f, 0f, 0f));
		i[7] = new Instantiation("pyramid", "mor", new Vector3(4f), new Vector3(9f, 5f, -5f), new Vector3(0f, 40f, 0f));
		i[8] = new Instantiation("pyramid", "ora", new Vector3(0f, 7f, -3f), new Vector3(0f, 0f, 0f));
		i[9] = new Instantiation("pyramid", "roanoke", 3, false, new Vector3(10f, 8f, 10f), new Vector3(30f, 7f, -10f), new Vector3(0f, 135f, 0f));
		i[10] = new Instantiation("cuadro", "tib", 3, false, new Vector3(1f), new Vector3(15f, 4f, 10f), new Vector3(0f, 0f, 0f));
		i[11] = new Instantiation("plano2", "roanoke", 3, false, new Vector3(res.getTexture("roanoke").width * 10f / res.getTexture("roanoke").height,10f,1f), new Vector3(20f, 12f, 20f), new Vector3(0f, 90f, 0f));
		i[12] = new Instantiation("cuadro", new Vector3(1f), 0, false, new Vector3(0.5f), new Vector3(1f), new Vector3(0f));
		i[13] = new Instantiation("cuadro", new Vector3(0f, 1f, 0f), 0, false, new Vector3(0.5f), new Vector3(3f, 2f, 10f), new Vector3(0f));
		i[14] = new Instantiation("cuadro", new Vector3(1f, 0f, 0f), 0, false, new Vector3(0.5f), new Vector3(13f, 2f, 10f), new Vector3(0f));
		i[15] = new Instantiation("cuadro", "light", new Vector3(2f,1.5f,4.5f), new Vector3(0f, 0f, 0f));
		
		sce.i = i;
		
		PointLight[] p = new PointLight[3];
		p[0] = new PointLight(new Vector3(1f), new Vector3(1f), 0.0075f, 0.045f, 1f);
		p[1] = new PointLight(new Vector3(3f, 2f, 10f), new Vector3(0f, 1f, 0f), 0.07f, 0.19f, 1f);
		p[2] = new PointLight(new Vector3(13f, 2f, 10f), new Vector3(1f, 0f, 0f), 0.0049f, 0.0062f, 1f);
		
		sce.p = p;
		
		//sce.dl = new DirLight(new Vector3(0f, -1f, 0f), new Vector3(0f, 0f, 1f));
		
		TextInstantiation[] t = new TextInstantiation[2];
		t[0] = new TextInstantiation("TEXT RENDERING (actually) WORKS! :D", this.fr, -1f, -0.95f, 0.05f, false);
		t[1] = new TextInstantiation("FPS: 0", this.fr, -1f, 1f, 48f, true);
		
		sce.t = t;
		
		return sce;
	}
	
	private void loadScene(Scene sce){
		this.backgroundColor = new Color4(sce.backgroundColor.X, sce.backgroundColor.Y, sce.backgroundColor.Z, 1f);
		this.fog = sce.fog;
		
		this.cam.allowMovement = sce.allowMovement;
		this.cam.allowCameraMovement = sce.allowCameraMovement;
		this.sg.setCursorGrabbedState(this.cam.allowCameraMovement);
		this.cam.movementSpeed = sce.movementSpeed;
		this.cam.setPos(sce.startingPos);
		this.cam.yaw = sce.startingYaw;
		this.cam.pitch = sce.startingPitch;
		this.cam.update();
		
		this.fov = sce.fov;
		this.nearPlane = sce.nearPlane;
		this.farPlane = sce.farPlane;
		
		this.sg.ch.consoleAllowed = sce.allowConsoleCommands;
		
		this.name = sce.name;
		this.author = sce.author;
		this.description = sce.description;
		this.sg.setTitle(Seagull.startTitle + " - " + this.name);
		
		this.sg.setFullScreen(sce.fullscreened);
		this.sg.fpsTarget = sce.targetFPS;
		
		this.ins = sce.i;
		this.poiLig = sce.p;
		this.dirLig = sce.dl;
		this.texIns = sce.t;
	}
	
	public void updateProjection(){
		this.projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(this.fov), this.getAspectRatio(), this.nearPlane, this.farPlane);
	}
	
	public float getAspectRatio(){
		return (float)this.width/(float)this.height;
	}
	
	private void transformObjects(){
		ins[0].rotate(new Vector3(20f * Seagull.dh.getTime(), 40f * Seagull.dh.getTime(), 50f * Seagull.dh.getTime()));
		
		ins[2].scale(new Vector3(0.5f + 3f*(float) Math.Sin(Seagull.dh.getTime())));
		ins[2].rotate(new Vector3(0f, 90f * (float)Math.Sin(Seagull.dh.getTime()), 0f));
		
		ins[6].rotate(new Vector3(0f, 180f * Seagull.dh.getTime(), 0f));
		
		ins[8].translateY(3f + 6f * (float) Math.Sin(Seagull.dh.getTime() + 2f));
		
		ins[5].revolveAroundCenter(new Vector3(4f, 0f, 0f));
		ins[5].revolveAroundY(90f * Seagull.dh.getTime());
		ins[5].rotateY(20f * Seagull.dh.getTime());
		
		ins[15].translateZ(6f + 6f * (float) Math.Sin(Seagull.dh.getTime()));
		
		ins[14].translateZ(24f * (float) Math.Sin(Seagull.dh.getTime() + 4f));
		poiLig[2].position = ins[14].getPos();
		
		texIns[1].setText("FPS: " + Seagull.dh.fps.ToString("F0"), fr);
	}
	
	public void draw(){
		this.transformObjects();
		
		GL.ClearColor(this.backgroundColor);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		
		Matrix4 view = this.cam.getView();
		
		res.useShader("mainShader");
		
		res.getShader("mainShader").setMatrix4("view", view);
		res.getShader("mainShader").setMatrix4("projection", this.projection);
		
		res.getShader("mainShader").setVector3("viewPos", this.cam.getPos());
		
		if(this.fog.applies){
			res.getShader("mainShader").setVector3("fog.color", this.fog.color);
			res.getShader("mainShader").setFloat("fog.start", this.fog.start);
			res.getShader("mainShader").setFloat("fog.end", this.fog.end);
		}
		
		if(poiLig != null){
			for(int i = 0; i < poiLig.Length; i++){
				PointLight n = poiLig[i];
				res.getShader("mainShader").setVector3("lights[" + i + "].color", n.color);
				res.getShader("mainShader").setVector3("lights[" + i + "].position", n.position);
				if(n.attenuationApplies){
					res.getShader("mainShader").setFloat("lights[" + i + "].attQuadratic", n.attQuadratic);
					res.getShader("mainShader").setFloat("lights[" + i + "].attLinear", n.attLinear);
					res.getShader("mainShader").setFloat("lights[" + i + "].attConstant", n.attConstant);
				}
				res.getShader("mainShader").setInt("lights[" + i + "].attenuationApplies", n.attenuationApplies ? 1 : 0);
				res.getShader("mainShader").setFloat("lights[" + i + "].ambientStrength", n.ambientStrength);
				res.getShader("mainShader").setFloat("lights[" + i + "].diffuseStrength", n.diffuseStrength);
				res.getShader("mainShader").setFloat("lights[" + i + "].specularStrength", n.specularStrength);
			}
		}
		
		res.getShader("mainShader").setVector3("dirLight.direction", dirLig.direction);
		res.getShader("mainShader").setVector3("dirLight.color", dirLig.color);
		
		res.getShader("mainShader").setFloat("dirLight.ambientStrength", dirLig.ambientStrength);
		res.getShader("mainShader").setFloat("dirLight.diffuseStrength", dirLig.diffuseStrength);
		res.getShader("mainShader").setFloat("dirLight.specularStrength", dirLig.specularStrength);

		res.getShader("mainShader").setVector3("mat.specularColor", new Vector3(1f));
		res.getShader("mainShader").setFloat("mat.shininess", 32f);
		
		foreach(Instantiation n in this.ins){
			if (n.model == null) continue;
			res.getShader("mainShader").setMatrix4("model", n.modelMatrix);
			res.getShader("mainShader").setMatrix3("normal", n.normalMatrix);
			res.getShader("mainShader").setInt("mat.fogApplies", this.fog.applies ? (n.fogApplies ? 1 : 0) : 0);
			
			int mode = n.drawMode;
			res.getShader("mainShader").setInt("mat.mode", mode);
			
			if(mode == 0 || mode == 1 || mode == 4 || mode == 5){
				if (n.tint == null){
					throw new Exception("Tint is null and drawmode is not");
				}
				res.getShader("mainShader").setVector3("mat.tint", n.tint);
			}
			
			if(mode == 2 || mode == 3 || mode == 4 || mode == 5){
				if (n.texture == null){
					throw new Exception("Texture is null and drawmode is not");
				}
				GL.ActiveTexture(TextureUnit.Texture0);
				res.bindTexture(n.texture);
				res.getShader("mainShader").setInt("mat.texture", 0);
			}
			
			res.bindAndDrawModel(n.model);
		}
		
		//fr.drawText("TEXT RENDERING (actually) WORKS! :D", -1f, -0.95f, 0.05f);
		//fr.drawTextConsistent("FPS: " + Seagull.dh.fps.ToString("F0"), -1f, 1f, 48f);
		if(texIns != null){
			foreach(TextInstantiation t in texIns){
				fr.drawText(t);
			}
		}
	}
	
	public void cleanUp(){
		
	}
}