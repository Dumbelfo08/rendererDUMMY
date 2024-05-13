using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class Model{
	public int VAO;
	public int numberOfVertices;
	
	public Model(float[] vertices, int floatsPerVertice, string format){
		
		if(vertices.Length % floatsPerVertice != 0){
			throw new Exception("Uncorrectly formatted vertices array. Uncorrect length");
		}
		this.numberOfVertices = vertices.Length / floatsPerVertice;
		
		int VBO = GL.GenBuffer(); //Initialize VBO
		GL.BindBuffer(BufferTarget.ArrayBuffer, VBO); //Bind VBO
		GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw); //Set VBO to vertices
		
		VAO = GL.GenVertexArray(); //Initialize VAO
		GL.BindVertexArray(VAO); //Bind VAO
		
		int j = 0;
		for(int i = 0; i < format.Length; i++){
			GL.VertexAttribPointer(i, Int32.Parse(new string(format.ToCharArray()[i], 1)), VertexAttribPointerType.Float, false, floatsPerVertice * sizeof(float), j * sizeof(float)); //Set parameters so it knows how to process it. 
			GL.EnableVertexAttribArray(i); //It is in layout i, so we set it
			j += Int32.Parse(new string(format.ToCharArray()[i], 1));
		}
		
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //Unbind VBO
		GL.BindVertexArray(0); //Unbind VAO
		GL.DeleteBuffer(VBO); //Delete VBO, we wont even need it anymore. If we delete before unbinding the VAO, it will unbind or something idk just dont do it
	}
	
	public void bind(){
		GL.BindVertexArray(VAO); //Bind the VAO containg the object
	}
	
	public void draw(){
		GL.DrawArrays(PrimitiveType.Triangles, 0, numberOfVertices); //IMPORTANT: NUMBER OF VERTICES, NOT TRIANGLES
	}
	
	public void cleanUp(){
		GL.DeleteVertexArray(VAO); //Delete VAO for clearing resources
	}
	
	public static float[] calculateNormals(float[] source, int floatsByVertex){
		int vertexCount = source.Length / floatsByVertex;
		int faceCount = vertexCount / 3;
		float[] final = new float[vertexCount * (floatsByVertex + 3)];
		
		//printVertex(source, floatsByVertex);
		
		for(int i = 0; i < faceCount; i++){
			float v1x, v1y, v1z; //Vertex 1 x y and z
			v1x = source[i * 3 * floatsByVertex];
			v1y = source[i * 3 * floatsByVertex + 1];
			v1z = source[i * 3 * floatsByVertex + 2];
			Vector3 v1 = new Vector3(v1x, v1y, v1z);
			
			float v2x, v2y, v2z; //vertex 2 x y and z
			v2x = source[i * 3 * floatsByVertex + floatsByVertex];
			v2y = source[i * 3 * floatsByVertex + floatsByVertex + 1];
			v2z = source[i * 3 * floatsByVertex + floatsByVertex + 2];
			Vector3 v2 = new Vector3(v2x, v2y, v2z);
			
			float v3x, v3y, v3z; //vertex 3 x y and z
			v3x = source[i * 3 * floatsByVertex + 2 * floatsByVertex];
			v3y = source[i * 3 * floatsByVertex + 2 * floatsByVertex + 1];
			v3z = source[i * 3 * floatsByVertex + 2 * floatsByVertex + 2];
			Vector3 v3 = new Vector3(v3x, v3y, v3z);
			
			Vector3 e1 = v2 - v1; //Edge 1
			Vector3 e2 = v3 - v1; //Edge 2
			
			Vector3 normal = Vector3.Normalize(Vector3.Cross(Vector3.Normalize(e1), Vector3.Normalize(e2)));
			Vector3 negNormal = Vector3.Normalize(-normal);
			
			Vector3 v1o, v2o, v3o;
			v1o = -v1;
			v2o = -v2;
			v3o = -v3;
			
			float theta, phi;
			theta = Vector3.Dot(normal, v1o); //We take first vertex because why not
			phi = Vector3.Dot(negNormal, v1o);
			
			normal = theta <= phi ? normal : negNormal;
			
			for(int j = 0; j < floatsByVertex; j++){
				final[i * 3 * (floatsByVertex + 3) + j] = source[i * 3 * floatsByVertex + j];
			}
			final[i * 3 * (floatsByVertex + 3) + floatsByVertex] = normal.X;
			final[i * 3 * (floatsByVertex + 3) + floatsByVertex + 1] = normal.Y;
			final[i * 3 * (floatsByVertex + 3) + floatsByVertex + 2] = normal.Z;
			
			for(int j = 0; j < floatsByVertex; j++){
				final[i * 3 * (floatsByVertex + 3) + (floatsByVertex + 3) + j] = source[i * 3 * floatsByVertex + floatsByVertex + j];
			}
			final[i * 3 * (floatsByVertex + 3) + (floatsByVertex + 3) + floatsByVertex] = normal.X;
			final[i * 3 * (floatsByVertex + 3) + (floatsByVertex + 3) + floatsByVertex + 1] = normal.Y;
			final[i * 3 * (floatsByVertex + 3) + (floatsByVertex + 3) + floatsByVertex + 2] = normal.Z;
			
			for(int j = 0; j < floatsByVertex; j++){
				final[i * 3 * (floatsByVertex + 3) + 2 * (floatsByVertex + 3) + j] = source[i * 3 * floatsByVertex + 2 * floatsByVertex + j];
			}
			final[i * 3 * (floatsByVertex + 3) + 2 * (floatsByVertex + 3) + floatsByVertex] = normal.X;
			final[i * 3 * (floatsByVertex + 3) + 2 * (floatsByVertex + 3) + floatsByVertex + 1] = normal.Y;
			final[i * 3 * (floatsByVertex + 3) + 2 * (floatsByVertex + 3) + floatsByVertex + 2] = normal.Z;
		}
		
		//printVertex(final, floatsByVertex + 3);
		
		return final;
	}
	
	public static void printVector(Vector3 v){
		Console.WriteLine("X: "+v.X+" Y: "+v.Y+" Z: "+v.Z);
	}
	
	public static void printVertex(float[] v, int nv){
		for(int i = 0; i < v.Length; i++){
			if(i != 0){
				Console.Write(", ");
			}
			if(i % nv == 0 && i != 0){
				Console.WriteLine("");
			}
			Console.Write(v[i]);
		}
		Console.WriteLine("");
		Console.WriteLine("");
	}
}