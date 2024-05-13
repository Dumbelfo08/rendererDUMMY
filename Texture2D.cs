using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StbImageSharp;

public class Texture2D {
	public int id;
	
	public int width;
	public int height;
	
	public PixelInternalFormat internalFormat;
	
	public Texture2D(ImageResult image, TextureParams tp) {	
		this.internalFormat = PixelInternalFormat.Rgba8;
		
		this.width = image.Width; //Extract this needed values
		this.height = image.Height;
		
		this.id = GL.GenTexture(); //Generate the handle for the texture
		GL.BindTexture(TextureTarget.Texture2D, this.id); //bind it
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) tp.wrapS); //Set the wrap parameters
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) tp.wrapT);
		
		if(tp.wrapS == TextureWrapMode.ClampToBorder || tp.wrapT == TextureWrapMode.ClampToBorder){
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new float[]{tp.borderColor.X, tp.borderColor.Y, tp.borderColor.Z}); //if needed set the border color
		}
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) tp.filterMin); //set upscaling/downscaling filter options
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) tp.filterMax);

		
		GL.TexImage2D(TextureTarget.Texture2D, 0, this.internalFormat, this.width, this.height, 0, tp.imageFormat, PixelType.UnsignedByte, image.Data); //actually generate the texture
		
		//if needed, generate mipmaps
		if(tp.filterMin == TextureMinFilter.NearestMipmapNearest || tp.filterMin == TextureMinFilter.LinearMipmapNearest || tp.filterMin == TextureMinFilter.NearestMipmapLinear || tp.filterMin == TextureMinFilter.LinearMipmapLinear){
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}
    	
    	GL.BindTexture(TextureTarget.Texture2D, 0); //unbind texture
	}
	
	public void bind() {
		GL.BindTexture(TextureTarget.Texture2D, this.id);
	}
	
	public void cleanUp(){
		GL.DeleteTexture(this.id); //Must have been unbinded first
	}
}