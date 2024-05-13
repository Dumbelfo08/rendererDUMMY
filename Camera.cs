using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class Camera {
	private Seagull sg;
	
	private Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f); //Position
	private Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
	private Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
	
	public float yaw;
	public float pitch;
	
	public float movementSpeed;
	private float sensitivity;
	
	public bool allowMovement;
	public bool allowCameraMovement;
	
	private float altitude;
	
	private Vector2 mouseLastPos;
	
	public Camera(Seagull sg) {
		this.sg = sg;
		this.allowMovement = true;
		this.allowCameraMovement = true;
		this.movementSpeed = 6f;
		
		this.altitude = 0f;
		this.sensitivity = 0.3f;
		this.update();
	}
	
	public void setPos(Vector3 p){
		this.pos = p;
		this.altitude = p.Y;
	}
	
	public void setPos(float x, float y, float z){
		this.pos = new Vector3(x, y, z);
		this.altitude = y;
	}
	
	public Vector3 getPos(){
		return this.pos;
	}
	
	public void setYawPitch(float y, float p){
		if(pitch > 89.0f)
	        pitch = 89.0f;
	    if(pitch < -89.0f)
	        pitch = -89.0f;
		update();
	}
	
	public Matrix4 getView() {
		Matrix4 v = Matrix4.LookAt(pos, pos + front, up);
		return v;
	}
	
	public void update() {
		float yawRad = (float) MathHelper.DegreesToRadians(yaw);
		float pitchRad = (float) MathHelper.DegreesToRadians(pitch);

		Vector3 direction = new Vector3(
		    (float) (Math.Cos(yawRad) * Math.Cos(pitchRad)),
		    (float) Math.Sin(pitchRad),
		    (float) (Math.Sin(yawRad) * Math.Cos(pitchRad))
		);

		front = Vector3.Normalize(direction);
	}
	
	public void handleMovement(int d) {
		if(!this.allowMovement){
			return;
		}
		float cameraSpeed = this.movementSpeed * (float) Seagull.dh.deltaTime;
		if (d == 0){
			Vector3 t = front;
			t.Y = 0f;
			t = Vector3.Normalize(t);
			pos += t * cameraSpeed;
			pos.Y = altitude;
		}
        if (d == 1){
        	Vector3 t = front;
			t.Y = 0f;
			t = Vector3.Normalize(t);
			pos -= t * cameraSpeed;
			pos.Y = altitude;
		}
        if (d == 2){
			Vector3 t = Vector3.Normalize(Vector3.Cross(front, up));
			t.Y = 0f;
			t = Vector3.Normalize(t);
			pos -= t * cameraSpeed;
			pos.Y = altitude;
		}
        if (d == 3){
        	Vector3 t = Vector3.Normalize(Vector3.Cross(front, up));
			t.Y = 0f;
			t = Vector3.Normalize(t);
			pos += t * cameraSpeed;
			pos.Y = altitude;
		}
		
		if (d == 4){
        	pos += up * cameraSpeed;
			altitude = pos.Y;
		}		
		if (d == 5){
        	pos -= up * cameraSpeed;
			altitude = pos.Y;
		}
	}
	
	public void handleMouseMov(float x, float y) {
		if(!this.allowCameraMovement){
			return;
		}
		
		float xoffset = x - mouseLastPos.X;
		float yoffset = y - mouseLastPos.Y;
		mouseLastPos = new Vector2(x, y);
		
		yaw += sensitivity * xoffset;
		pitch -= sensitivity * yoffset;
		
		if(pitch > 89.0f)
	        pitch = 89.0f;
	    if(pitch < -89.0f)
	        pitch = -89.0f;
		
		update();
	}
}
