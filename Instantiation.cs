using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public struct Instantiation{ //this class reprensents an instatantiation of an object in a position and all of that
	public string model;
	public string texture;
	
	public Vector3 tint;
	
	public Matrix4 modelMatrix;
	public Matrix3 normalMatrix;
	
	private Vector3 scalar;
	private Vector3 translation;
	private Vector3 rotation;
	private Vector3 revAround;
	private Vector3 revAroundCenter;
	
	public bool fogApplies;
	
	public int drawMode;
	
	public Instantiation(string mod, string tex, Vector3 scalar, Vector3 translation, Vector3 rotation){
		this.model = mod; //model
		this.texture = tex; //texture
		this.drawMode = 3;
		this.scalar = scalar;
		this.translation = translation;
		this.rotation = rotation;
		this.calculate();
		this.fogApplies = true;
	}
	
	public Instantiation(string mod, string tex, Vector3 translation, Vector3 rotation){
		this.model = mod; //model
		this.texture = tex; //texture
		this.drawMode = 3;
		this.scalar = new Vector3(1f);
		this.translation = translation;
		this.rotation = rotation;
		this.calculate();
		this.fogApplies = true;
	}
	
	public Instantiation(string mod, string tex, int mode, bool b, Vector3 scalar, Vector3 translation, Vector3 rotation){
		this.model = mod;
		this.texture = tex;
		this.drawMode = mode;
		this.scalar = scalar;
		this.translation = translation;
		this.rotation = rotation;
		this.calculate();
		this.fogApplies = b;
	}
	
	public Instantiation(string mod, string tex){
		this.model = mod;
		this.texture = tex;
		this.drawMode = 3;
		this.scalar = new Vector3(1f);
		this.translation = new Vector3(0f);
		this.rotation = new Vector3(0f);
		this.calculate();
		this.fogApplies = true;
	}
	
	public Instantiation(string mod, Vector3 t, Vector3 scalar, Vector3 translation, Vector3 rotation){
		this.model = mod; //model
		this.tint = t; //tint
		this.drawMode = 1;
		this.scalar = scalar;
		this.translation = translation;
		this.rotation = rotation;
		this.calculate();
		this.fogApplies = true;
	}
	
	public Instantiation(string mod, Vector3 t, Vector3 translation, Vector3 rotation){
		this.model = mod; //model
		this.tint = t; //tint
		this.drawMode = 1;
		this.scalar = new Vector3(1f);
		this.translation = translation;
		this.rotation = rotation;
		this.calculate();
		this.fogApplies = true;
	}
	
	public Instantiation(string mod, Vector3 t, int mode, bool b, Vector3 scalar, Vector3 translation, Vector3 rotation){
		this.model = mod;
		this.tint = t; //tint
		this.drawMode = mode;
		this.scalar = scalar;
		this.translation = translation;
		this.rotation = rotation;
		this.calculate();
		this.fogApplies = b;
	}
	
	public Instantiation(string mod, Vector3 t){
		this.model = mod;
		this.tint = t; //tint
		this.drawMode = 1;
		this.scalar = new Vector3(1f);
		this.translation = new Vector3(0f);
		this.rotation = new Vector3(0f);
		this.calculate();
		this.fogApplies = true;
	}
	
	public Vector3 getPos(){
		Matrix4 m1 = Matrix4.CreateTranslation(translation);
		Matrix4 m4 = Matrix4.CreateTranslation(revAroundCenter);
		Matrix4 m5 = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(revAround.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(revAround.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(revAround.Z));
		Matrix4 pM = m4 * m5 * m1;
		Vector4 pV = Vector4.TransformRow(new Vector4(1f), pM);
		return new Vector3(pV.X, pV.Y, pV.Z);
	}
	
	public void calculate(){
		Matrix4 m1 = Matrix4.CreateTranslation(translation);
		Matrix4 m2 = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
		Matrix4 m3 = Matrix4.CreateScale(scalar);
		Matrix4 m4 = Matrix4.CreateTranslation(revAroundCenter);
		Matrix4 m5 = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(revAround.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(revAround.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(revAround.Z));
		modelMatrix = m3 * m2 * m4 * m5 * m1;
		
		normalMatrix = Matrix3.Transpose(Matrix3.Invert(new Matrix3(modelMatrix)));
	}
	
	public void scale(Vector3 s){
		this.scalar = s;
		this.calculate();
	}
	
	public void scale(float x, float y, float z){
		this.scalar = new Vector3(x,y,z);
		this.calculate();
	}
	
	public void scaleX(float s){
		this.scalar.X = s;
		this.calculate();
	}
	
	public void scaleY(float s){
		this.scalar.Y = s;
		this.calculate();
	}
	
	public void scaleZ(float s){
		this.scalar.Z = s;
		this.calculate();
	}
	
	public void rotate(Vector3 r){
		this.rotation = r;
		this.calculate();
	}
	
	public void rotate(float x, float y, float z){
		this.rotation = new Vector3(x,y,z);
		this.calculate();
	}
	
	public void rotateX(float r){
		this.rotation.X = r;
		this.calculate();
	}
	
	public void rotateY(float r){
		this.rotation.Y = r;
		this.calculate();
	}
	
	public void rotateZ(float r){
		this.rotation.Z = r;
		this.calculate();
	}
	
	public void translate(Vector3 t){
		this.translation = t;
		this.calculate();
	}
	
	public void translate(float x, float y, float z){
		this.translation = new Vector3(x,y,z);
		this.calculate();
	}
	
	public void translateX(float t){
		this.translation.X = t;
		this.calculate();
	}
	
	public void translateY(float t){
		this.translation.Y = t;
		this.calculate();
	}
	
	public void translateZ(float t){
		this.translation.Z = t;
		this.calculate();
	}
	
	public void revolveAround(Vector3 r){
		this.revAround = r;
		this.calculate();
	}
	
	public void revolveAround(float r1, float r2, float r3){
		this.revAround = new Vector3(r1, r2, r3);
		this.calculate();
	}
	
	public void revolveAroundX(float r){
		this.revAround.X = r;
		this.calculate();
	}
	
	public void revolveAroundY(float r){
		this.revAround.Y = r;
		this.calculate();
	}
	
	public void revolveAroundZ(float r){
		this.revAround.Z = r;
		this.calculate();
	}
	
	public void revolveAroundCenter(Vector3 t){
		this.revAroundCenter = t;
		this.calculate();
	}
}