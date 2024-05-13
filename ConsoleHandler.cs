using OpenTK.Mathematics;

public class ConsoleHandler{
	private Seagull sg;
	
	private string currentCommand;
	private int lines = 0;
	
	public bool consoleAllowed;
	
	public ConsoleHandler(Seagull s){
		this.sg = s;
		this.currentCommand = "";
	}
	
	public void handleInput(){
		if(Console.KeyAvailable){
			
			ConsoleKeyInfo key = Console.ReadKey(true);

			if (key.Key == ConsoleKey.Enter)
			{
				Console.SetCursorPosition(0, Console.CursorTop - lines);
				lines = 0;
				
				if(this.consoleAllowed){
					// Process the user's currentCommand
					this.processCommand(currentCommand);
				} else {
					writeError("Commands are not activated");
				}

				// Clear the currentCommand
				currentCommand = "";
			}
			else if (key.Key == ConsoleKey.Backspace && currentCommand.Length > 0)
			{
				// Handle backspace
				currentCommand = currentCommand.Substring(0, currentCommand.Length - 1);
				if(Console.CursorLeft != 0){
					Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
					Console.Write(" ");
					Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
				} else if(currentCommand.Length != 0){
					Console.SetCursorPosition(Console.WindowWidth - 1, Console.CursorTop - 1);
					Console.Write(" ");
					Console.SetCursorPosition(Console.WindowWidth - 1, Console.CursorTop - 1);
				}
			}
			else if (!char.IsControl(key.KeyChar))
			{
				int o = Console.CursorTop;
				currentCommand += key.KeyChar;
				Console.Write(key.KeyChar);
				int n = Console.CursorTop;
				if(n == o+1){
					lines++;
				}
			}
		}
	}
	
	private void processCommand(string command){
		Console.WriteLine(command);
		
		string[] words = command.Split(" ");
		
		switch (words[0]){
			case "setInitialPos":
				this.sg.ren.cam.setPos(0f, 0f, 0f);
				break;
			case "setPos":
				if(words.Length != 4){
					writeError("Incorrect number of arguments");
					break;
				}
				float a, b, c = 0f;
				if(!float.TryParse(words[1], out a)){
					writeError("Incorrect formatting of 1st argument");
					break;
				}
				if(!float.TryParse(words[2], out b)){
					writeError("Incorrect formatting of 2nd argument");
					break;
				}
				if(!float.TryParse(words[3], out c)){
					writeError("Incorrect formatting of 3rd argument");
					break;
				}
				this.sg.ren.cam.setPos(a, b, c);
				break;
			case "getPos":
				Vector3 p = this.sg.ren.cam.getPos();
				Console.WriteLine(p.X + ", " + p.Y + ", " + p.Z);
				break;
			case "setInitialView":
				this.sg.ren.cam.yaw = 0f;
				this.sg.ren.cam.pitch = 0f;
				this.sg.ren.cam.update();
				break;
			case "setView":
				if(words.Length != 3){
					writeError("Incorrect number of arguments");
					break;
				}
				if(!float.TryParse(words[1], out a)){
					writeError("Incorrect formatting of 1st argument yaw");
					break;
				}
				if(!float.TryParse(words[2], out b)){
					writeError("Incorrect formatting of 2nd argument pitch");
					break;
				}
				this.sg.ren.cam.yaw = a;
				this.sg.ren.cam.pitch = b;
				this.sg.ren.cam.update();
				break;
			case "getView":
				Console.WriteLine("Yaw: " + this.sg.ren.cam.yaw + " Pitch: " + this.sg.ren.cam.pitch);
				break;
			case "info":
				Console.WriteLine("Scene name: " + this.sg.ren.name);
				Console.WriteLine("Scene author: " + this.sg.ren.author);
				Console.WriteLine("Scene description: " + this.sg.ren.description);
				break;
			default:
				writeError("Unexistent command");
				break;
		}
		
		Console.WriteLine("");
	}
	
	private void writeError(string s){
		Console.WriteLine("SYNTAX ERROR: " + s);
	}
}