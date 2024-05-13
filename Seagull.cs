using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using AshLib;

public class Seagull : GameWindow{
	
	public Renderer ren;
	public ResourceManager res;
	public static DeltaHelper dh;
	public ConsoleHandler ch;
	
	public const int startWidth = 640;
	public const int startHeight = 480;
	public const string startTitle = "Seagull Renderer";
	
	private bool fullScreened = false;
	private bool fBeingPressed = false;
	
	public int fpsTarget = 144;
	
	private Seagull() : base(GameWindowSettings.Default, NativeWindowSettings.Default){
		this.CenterWindow(new Vector2i(startWidth, startHeight));
		this.Title = startTitle;
	}
	
	private void init(){
		dh = new DeltaHelper();
		dh.Start();
		
		StbImage.stbi_set_flip_vertically_on_load(1);
		GL.Enable(EnableCap.DepthTest);
		
		ch = new ConsoleHandler(this);
		
		res = new ResourceManager();
		
		ren = new Renderer(this);
	}
	
	private void onResize(int width, int  height){
		GL.Viewport(0, 0, width, height);
		if(this.ren != null){
			this.ren.width = width;
			this.ren.height = height;
			this.ren.updateProjection();
		}
	}
	
	private void handleKeyboardInput(){
		if (!IsFocused) // check to see if the window is focused
		{
			return;
		}
		
		if (KeyboardState.IsKeyDown(Keys.Escape))
		{
			Close();
		}
		
		if (KeyboardState.IsKeyDown(Keys.F))
		{
			if(this.fullScreened && !this.fBeingPressed){
				this.setFullScreen(false);
				this.fBeingPressed = true;
			} else if (!this.fBeingPressed){
				this.setFullScreen(true);
				this.fBeingPressed = true;
			}
		} else {
			this.fBeingPressed = false;
		}
		
		if (KeyboardState.IsKeyDown(Keys.W))
		{
			this.ren.cam.handleMovement(0);
		}
		
		if (KeyboardState.IsKeyDown(Keys.S))
		{
			this.ren.cam.handleMovement(1);
		}
		
		if (KeyboardState.IsKeyDown(Keys.A))
		{
			this.ren.cam.handleMovement(2);
		}
		
		if (KeyboardState.IsKeyDown(Keys.D))
		{
			this.ren.cam.handleMovement(3);
		}
		
		if (KeyboardState.IsKeyDown(Keys.Space))
		{
			this.ren.cam.handleMovement(4);
		}
		
		if (KeyboardState.IsKeyDown(Keys.LeftShift))
		{
			this.ren.cam.handleMovement(5);
		}
		
	}
	
	private void draw(){		
		this.ren.draw();
		
		this.Context.SwapBuffers();
	}
	
	private void mouseMove(float x, float y){
		if (IsFocused)
        {
            this.ren.cam.handleMouseMov(x, y);
        }
	}
	
	public void setFullScreen(bool b){
		if(b){
			this.WindowState = WindowState.Fullscreen;
			this.fullScreened = true;
		} else {
			this.WindowState = WindowState.Normal;
			this.fullScreened = false;
		}
	}
	
	public void setTitle(string title){
		this.Title = title;
	}
	
	public void setCursorGrabbedState(bool b){
		if(b){
			CursorState = CursorState.Grabbed;
		} else {
			CursorState = CursorState.Normal;
		}
		
	}
	
	private void cleanUp(){
		GL.BindVertexArray(0); //Unbind any VAO
		GL.UseProgram(0); //Use no shader program
		GL.BindTexture(TextureTarget.Texture2D, 0); //Unbind any texture
		
		this.ren.cleanUp();
		this.res.cleanUp();
	}
	
	private void checkErrors(){
		OpenTK.Graphics.OpenGL.ErrorCode errorCode = GL.GetError();
        while (errorCode != OpenTK.Graphics.OpenGL.ErrorCode.NoError)
        {
            Console.WriteLine($"OpenGL Error: {errorCode}");
            errorCode = GL.GetError();
        }
	}
	
	public static void Main(){
		using(Seagull sg = new Seagull()){
			sg.Run();
		}
	}
	
	protected override void OnLoad(){		
		this.init();
		base.OnLoad();
	}
	
	protected override void OnUnload(){
		this.cleanUp();
		base.OnUnload();
	}
	
	protected override void OnResize(ResizeEventArgs args){
		this.onResize(args.Width, args.Height);
		base.OnResize(args);
	}
	
	protected override void OnUpdateFrame(FrameEventArgs args){
		this.handleKeyboardInput();
		this.ch.handleInput();
		base.OnUpdateFrame(args);
	}
	
	protected override void OnRenderFrame(FrameEventArgs args){
		this.draw();
		this.checkErrors();
		base.OnRenderFrame(args);
		dh.Target((float) this.fpsTarget);
		dh.Frame();
	}
	
	protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        this.mouseMove(e.X, e.Y);
		base.OnMouseMove(e);
    }
}