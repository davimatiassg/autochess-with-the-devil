using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Godot;
using Godot.Collections; // Necessário para 'Dictionary'

public partial class DialogMessenger : CanvasLayer
{
	public static DialogMessenger Instance;

	private static readonly PackedScene _DIALOGSCREEN = GD.Load<PackedScene>("res://scenes/DialogScene.tscn");

	public static DialogScreen currentDialog;
	

	public static async Task SpawnDialog(Dictionary dialogData)
	{
		if (currentDialog != null && IsInstanceValid(currentDialog)) return;


		currentDialog = _DIALOGSCREEN.Instantiate<DialogScreen>();
		currentDialog.Data = dialogData;
		Instance.AddChild(currentDialog);

		while (IsInstanceValid(currentDialog)) await Task.Delay(200);
	}

	public override void _Ready()
	{
		base._Ready();
		if (Instance == null) Instance = this;
		else if (Instance != this) QueueFree();	
    } 
}
