using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Text;

public class Shader {
	public int id;
	
	public Shader(string vertex, string fragment, string? geometry) {
		int vertexShader = GL.CreateShader(ShaderType.VertexShader);
		GL.ShaderSource(vertexShader, vertex);
		GL.CompileShader(vertexShader);
		
		int compileStatus;
		GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out compileStatus);
		if (compileStatus == 0)
		{
			string log = GL.GetShaderInfoLog(vertexShader);
			throw new Exception("GLSL VERTEX SHADER COMPILING ERROR:\n"+log);
		}
		
		int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
		GL.ShaderSource(fragmentShader, fragment);
		GL.CompileShader(fragmentShader);
		
		GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out compileStatus);
		if (compileStatus == 0)
		{
			string log = GL.GetShaderInfoLog(fragmentShader);
			throw new Exception("GLSL FRAGMENT SHADER COMPILING ERROR:\n"+log);
		}
		
		int geometryShader = 0;
		if(geometry != null) {
			geometryShader = GL.CreateShader(ShaderType.GeometryShader);
			GL.ShaderSource(geometryShader, geometry);
			GL.CompileShader(geometryShader);
			
			GL.GetShader(geometryShader, ShaderParameter.CompileStatus, out compileStatus);
			if (compileStatus == 0)
			{
				string log = GL.GetShaderInfoLog(geometryShader);
				throw new Exception("GLSL GEOMETRY SHADER COMPILING ERROR:\n"+log);
			}
		}
		
		this.id = GL.CreateProgram();
		GL.AttachShader(this.id, vertexShader);
		GL.AttachShader(this.id, fragmentShader);
		if(geometry != null) {
			GL.AttachShader(this.id, geometryShader);
		}
		GL.LinkProgram(this.id);
		GL.ValidateProgram(this.id);
		
		GL.DetachShader(this.id, vertexShader);
		GL.DetachShader(this.id, fragmentShader);
		if(geometry != null) {
	    	GL.DetachShader(this.id, geometryShader);
	    }
		
		GL.DeleteShader(vertexShader);
	    GL.DeleteShader(fragmentShader);
	    if(geometry != null) {
	    	GL.DeleteShader(geometryShader);
	    }
		
		int infoLogLength;
		GL.GetProgram(this.id, GetProgramParameterName.InfoLogLength, out infoLogLength);
		if (infoLogLength > 0)
		{
			string infoLog = "";
			GL.GetProgramInfoLog(this.id, infoLogLength, out _, out infoLog);
			Console.WriteLine("Shader Program Info Log:");
			Console.WriteLine(infoLog);
		}
	}
	
	public void use() {
		GL.UseProgram(this.id);
	}
	
	public void setInt(string name, int data){
		GL.Uniform1(GL.GetUniformLocation(this.id, name), data);
	}
	public void setFloat(string name, float data){
		GL.Uniform1(GL.GetUniformLocation(this.id, name), data);
	}
	public void setIntArray(string name, int[] data){
		GL.Uniform1(GL.GetUniformLocation(this.id, name), data.Length, data);
	}
	public void setFloatArray(string name, float[] data){
		GL.Uniform1(GL.GetUniformLocation(this.id, name), data.Length, data);
	}
	public void setMatrix3(string name, Matrix3 data){
		GL.UniformMatrix3(GL.GetUniformLocation(this.id, name), false, ref data);
	}
	public void setMatrix4(string name, Matrix4 data){
		GL.UniformMatrix4(GL.GetUniformLocation(this.id, name), false, ref data);
	}
	public void setVector3(string name, Vector3 data){
		GL.Uniform3(GL.GetUniformLocation(this.id, name), data.X, data.Y, data.Z);
	}
	public void setVector4(string name, Vector4 data){
		GL.Uniform4(GL.GetUniformLocation(this.id, name), data.X, data.Y, data.Z, data.W);
	}
	public void setVector2(string name, Vector2 data){
		GL.Uniform2(GL.GetUniformLocation(this.id, name), data.X, data.Y);
	}
	public void setVector2i(string name, Vector2i data){
		GL.Uniform2(GL.GetUniformLocation(this.id, name), data.X, data.Y);
	}
	
	public void cleanUp(){
		GL.DeleteProgram(this.id); //Delete the shader for clearing resources
	}
}