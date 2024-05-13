using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class FontRenderer{
	
	private static bool first = false; //first generated fontrenderer, for not creating models more than once
	
	public string fontShader;
	public string fontTexture;
	public char[] map;
	private Seagull sg;
	private string mapRaw;
	
	private int unfoundSymbolPos;
	
	public int glyphRows; //Rows of the font texture
	public int glyphColumns; //columns of the font texture
	
	public FontRenderer(string s, string t, Seagull sg, int row, int col){
		this.fontShader = s;
		this.fontTexture = t;
		this.sg = sg;
		this.glyphRows = row;
		this.glyphColumns = col;
		this.mapRaw = "ABCDEFGHIJKLMNOPQRSTUVWXYZ .,:+-*/\\'\"$()[]^?!%~º1234567890 |□#<>abcdefghijklmnopqrstuvwxyz"; //default map
		this.generateMap();
		if(!first){ //We cant generate nor is effective to generate the same model more than one time
			this.generateModel(); 
			first = true;
		}
	}
	
	public FontRenderer(string s, string t, Seagull sg, int row, int col, string raw){
		this.fontShader = s;
		this.fontTexture = t;
		this.sg = sg;
		this.glyphRows = row;
		this.glyphColumns = col;
		this.mapRaw = raw; //Specified map
		this.generateMap();
		if(!first){ //We cant generate nor is effective to generate the same model more than one time
			this.generateModel();
			first = true;
		}
	}
	
	private void generateModel(){
		float[] vertices = { //y is in -1 so starting pos of the text is in the left upper corner
			1f, -1f,
			1f, 0f,
			0f, -1f,
			1f, 0f,
			0f, 0f,
			0f, -1f,
		};
		
		sg.res.generateModel("font", vertices, 2, "2");
	}
	
	private void generateMap(){
		this.map = mapRaw.ToCharArray();
		for(int i = 0; i < this.map.Length; i++){
			if(this.map[i] == '□'){
				this.unfoundSymbolPos = i;
				break;
			}
		}
	}
	
	public int[] textToMap(string text){
		int[] l = new int[text.Length];
		char[] c = text.ToCharArray();
		
		for(int i = 0; i < c.Length; i++){
			for(int j = 0; j < this.map.Length; j++){
				if(c[i] == this.map[j]){
					l[i] = j;
					break;
				}
				if(j == this.map.Length - 1){
					l[i] = this.unfoundSymbolPos;
				}
			}
		}
		return l;
	}
	
	public void drawText(string text, float x, float y, float size){
		int[] l = textToMap(text);
		
		sg.res.useShader(fontShader);
		sg.res.getShader(this.fontShader).setIntArray("letters", l); //Set the letters so it knows wich glyphs to actually choose
		sg.res.getShader(this.fontShader).setVector2("position", new Vector2(x, y)); //Starting position
		sg.res.getShader(this.fontShader).setFloat("size", size); //Size will be static no matter the size of the window
		sg.res.getShader(this.fontShader).setFloat("aspectRatio", this.sg.ren.getAspectRatio());
		sg.res.getShader(this.fontShader).setVector2i("glyphStructure", new Vector2i(this.glyphRows, this.glyphColumns)); //Pass the number of rows and cols
		
		sg.res.bindTexture(this.fontTexture);
		
		sg.res.bindModel("font");
		GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, 6, l.Length);  
	}
	
	public void drawTextConsistent(string text, float x, float y, float size){ //Consistent size in pixels
		int[] l = textToMap(text);
		
		sg.res.useShader(fontShader);
		sg.res.getShader(this.fontShader).setIntArray("letters", l); //Set the letters so it knows wich glyphs to actually choose
		sg.res.getShader(this.fontShader).setVector2("position", new Vector2(x, y)); //Starting position
		sg.res.getShader(this.fontShader).setFloat("size", size/(float)this.sg.ren.height); //Size will be static no matter the size of the window
		sg.res.getShader(this.fontShader).setFloat("aspectRatio", this.sg.ren.getAspectRatio());
		sg.res.getShader(this.fontShader).setVector2i("glyphStructure", new Vector2i(this.glyphRows, this.glyphColumns)); //Pass the number of rows and cols
		
		sg.res.bindTexture(this.fontTexture);
		
		sg.res.bindModel("font");
		GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, 6, l.Length);  
	}
	
	public void drawText(TextInstantiation t){
		sg.res.useShader(fontShader);
		sg.res.getShader(this.fontShader).setIntArray("letters", t.map); //Set the letters so it knows wich glyphs to actually choose
		sg.res.getShader(this.fontShader).setVector2("position", t.position); //Starting position
		sg.res.getShader(this.fontShader).setFloat("size", t.consistent ? t.size/(float)this.sg.ren.height : t.size); //Size will be static no matter the size of the window
		sg.res.getShader(this.fontShader).setFloat("aspectRatio", this.sg.ren.getAspectRatio());
		sg.res.getShader(this.fontShader).setVector2i("glyphStructure", new Vector2i(this.glyphRows, this.glyphColumns)); //Pass the number of rows and cols
		
		sg.res.bindTexture(this.fontTexture);
		
		sg.res.bindModel("font");
		GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, 6, t.map.Length);  
	}
}