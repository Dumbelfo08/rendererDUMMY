using System;
using StbImageSharp;
using OpenTK;
using OpenTK.Graphics.OpenGL;

public class ResourceManager{
	
	protected Dictionary<string, Shader> shaderMap = new Dictionary<string, Shader>(); //Map of shader models
	protected Dictionary<string, Texture2D> textureMap = new Dictionary<string, Texture2D>(); //Map is texture models
	protected Dictionary<string, Model> modelMap = new Dictionary<string, Model>(); //Map of models
	
	public byte[] readFileBytes(string path){
		return File.ReadAllBytes(path);
	}
	
	public string readFile(string path){
		return File.ReadAllText(path);
	}
	
	public void generateShader(string name, string v, string f, string? g){
		Shader s = new Shader(v, f, g);
		shaderMap.Add(name, s);
	}
	
	public void generateShaderFile(string name, string v, string f, string? g){
		Shader s = new Shader(this.readFile(v), this.readFile(f), g != null ? this.readFile(g) : null);
		shaderMap.Add(name, s);
	}
	
	public Shader getShader(string name){
		return shaderMap[name];
	}
	
	public void useShader(string name){
		shaderMap[name].use();
	}
	
	public void generateTexture(string name, ImageResult image, TextureParams tp){
		Texture2D t = new Texture2D(image, tp);
		textureMap.Add(name, t);
	}
	
	public void generateTextureFile(string name, string path, TextureParams tp){
		ImageResult image = ImageResult.FromMemory(this.readFileBytes(path), ConvertFormat(tp.imageFormat));
		if (image == null || image.Data == null){
			throw new Exception("Image loading failed from:" + path);
		}
		Texture2D t = new Texture2D(image, tp);
		textureMap.Add(name, t);
	}
	
	public Texture2D getTexture(string name){
		return textureMap[name];
	}
	
	public void bindTexture(string name){
		textureMap[name].bind();
	}
	
	public void generateModel(string name, float[] vertices, int floatsPerVertice, string format){
		Model o = new Model(vertices, floatsPerVertice, format);
		modelMap.Add(name, o);
	}
	
	public void bindModel(string name){
		modelMap[name].bind();
	}
	
	public void drawModel(string name){
		modelMap[name].draw();
	}
	
	public void bindAndDrawModel(string name){
		modelMap[name].bind();
		modelMap[name].draw();
	}
	
	public void cleanUp(){
		foreach (KeyValuePair<string, Shader> kvp in this.shaderMap) //Cleanup shaders
        {
			kvp.Value.cleanUp();
        }
		
		foreach (KeyValuePair<string, Texture2D> kvp in this.textureMap) //Cleanup textures
        {
			kvp.Value.cleanUp();
        }
		
		foreach (KeyValuePair<string, Model> kvp in this.modelMap) //Cleanup models
        {
			kvp.Value.cleanUp();
        }
	}
	
	public static ColorComponents ConvertFormat(PixelFormat pixelFormat)
    {
        switch (pixelFormat)
        {
            case PixelFormat.Rgb:
                return ColorComponents.RedGreenBlue;
            case PixelFormat.Rgba:
                return ColorComponents.RedGreenBlueAlpha;
            // Add more cases for other pixel formats as needed
            default:
                throw new ArgumentException("Unsupported pixel format in conversion");
        }
    }
}