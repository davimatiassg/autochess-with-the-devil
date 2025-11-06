using Godot;
using Godot.Collections; // Necessário para 'Dictionary'

public partial class DialogTest : Node3D
{
	//Pra usar em algum lugar, vc cria uma packed scene, faz o preload e instância quando precisa rolar o diálogo
	//Ao instânciar vc tem que definir os dados desse diálogo, a sequência das falas
	
	// Em C#, usamos GD.Load para carregar recursos. 
	// 'static readonly' garante que ele seja carregado apenas uma vez, como o 'const' do GDScript.
	private static readonly PackedScene _DIALOG_SCREEN = GD.Load<PackedScene>("res://scenes/DialogScene.tscn");
	
	// A inicialização do Dicionário em C# é um pouco mais verbosa.
	// Usamos o 'Dictionary' genérico do Godot.
	private Dictionary _dialog_data = new Dictionary
	{
		{ 0, new Dictionary
			{
				{ "icon", "res://icon.svg" },
				{ "title", "Djabao" },
				{ "dialog", "Eu sou o diabo, faço apostas, gosto de rock e tals, chega mais" }
			}
		},
		{ 1, new Dictionary
			{
				{ "icon", "res://icon.svg" },
				{ "title", "Você" },
				{ "dialog", "Krl, que massa, bora apostar aí" }
			}
		},
		{ 2, new Dictionary
			{
				{ "icon", "res://icon.svg" },
				{ "title", "Djabao" },
				{ "dialog", "Pode ser, mas se tu perder levo tua alma boyzão" }
			}
		},
		{ 3, new Dictionary
			{
				{ "icon", "res://icon.svg" },
				{ "title", "Você" },
				{ "dialog", "Tá suave, já tô aq mermo, simbora" }
			}
		}
	};

	//Pra linkar com o HUD
	// Em C#, @export vira [Export]
	[Export] private CanvasLayer _hud;

	//utilizando a tecla espaço pra testar a ativação do diálogo
	// 'delta' é um 'double' em C#
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_select"))
		{
			// Usamos .Instantiate<T>() para instanciar e já converter para o tipo do nosso script C#
			Dialog_Screen _new_dialog = _DIALOG_SCREEN.Instantiate<Dialog_Screen>();
			
			// GD.Print é o 'print' do C#
			GD.Print("Novo diálogo aq: ", _new_dialog);

			// 'Data' será uma Propriedade pública no script Dialog_Screen.cs
			_new_dialog.Data = _dialog_data;
			_hud.AddChild(_new_dialog);
		}
	}
}
