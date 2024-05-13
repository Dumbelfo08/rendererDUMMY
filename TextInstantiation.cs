using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class TextInstantiation{
	public int[] map;
	private string text;
	public Vector2 position;
	public float size;
	public bool consistent;
	
	public TextInstantiation(string t, FontRenderer f, Vector2 pos, float s, bool c){
		this.text = t;
		this.map = f.textToMap(this.text);
		this.position = pos;
		this.size = s;
		this.consistent = c;
	}
	
	public TextInstantiation(string t, FontRenderer f, float x, float y, float s, bool c){
		this.text = t;
		this.map = f.textToMap(this.text);
		this.position = new Vector2(x, y);
		this.size = s;
		this.consistent = c;
	}
	
	public void setText(string t, FontRenderer f){
		this.text = t;
		this.map = f.textToMap(this.text);
	}
}