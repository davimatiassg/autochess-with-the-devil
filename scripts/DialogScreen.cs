using Godot;
using Godot.Collections; // Necessário para 'Dictionary'

public partial class DialogScreen : Control
{
	// _textVel é a velocidade com que o texto aparece na tela, mas quanto menor, mais rápido vai aparecer
	// O _id é o indice do dicionário, que é incrementado depois pra mudar de diálogo
	// data é o dicionário em si
	
	// Literais 'float' em C# precisam do sufixo 'f'
	private float _textVel = 0.05f; 
	private int _id = 0;

	// Em C#, é boa prática expor variáveis públicas como Propriedades (PascalCase).
	// O script DialogTest vai acessar esta propriedade 'Data'.
	public Dictionary Data { get; set; } = new Dictionary();

	//Aq tô exportando pra linkar os objetos da cena DialogScreen
	[Export] private Label _name;
	[Export] private RichTextLabel _dialogText;
	[Export] private TextureRect _dialogIcon;

	//No ready, quando for instanciado vai chamar a func de inicializar diálogo
	public override void _Ready()
	{
		// Métodos em C# são por convenção em PascalCase
		_InitializeDialog();
		// 'pass' não existe em C#
	}

	//O process checa quando o player ta segurando enter, pra acelerar o diálogo
	public override void _Process(double delta)
	{
		// 'and' vira '&&'
		// Propriedades são PascalCase (VisibleRatio)
		if (Input.IsActionPressed("ui_accept") && _dialogText.VisibleRatio < 1)
		{
			_textVel = 0.01f;
			return;
		}

		//Se só apertar enter, skipa o diálogo
		_textVel = 0.05f;
		if (Input.IsActionJustPressed("ui_accept"))
		{
			_id++; // _id += 1
			
			//Checagem pra saber se o dicionário chegou no fim
			// .size() vira .Count em C#
			if (_id == Data.Count)
			{
				QueueFree(); // Métodos são PascalCase
				return;
			}

			_InitializeDialog();
		}
	}

	//Começa o diálogo
	// Como esta função usa 'await', ela precisa ser declarada como 'async void'
	private async void _InitializeDialog()
	{
		//Bug q tava dando, tratei assim, mas n sei se tem algo melhor a se fazer
		// 'not' vira '!' e .has() vira .ContainsKey()
		if (!Data.ContainsKey(_id))
		{
			GD.Print("Diálogo não encontrado para o ID: ", _id);
			QueueFree(); //Diálogo fecha
			return;
		}

		//Pega os dados do dicionário data
		// Em C#, precisamos ser explícitos sobre os tipos dentro do Dictionary (que guarda 'Variant')
		var entry = Data[_id].AsGodotDictionary();
		
		_name.Text = entry["title"].AsString();
		_dialogText.Text = entry["dialog"].AsString();
		
		// Carregar o ícone requer GD.Load e uma conversão para o tipo de textura
		string iconPath = entry["icon"].AsString();
		_dialogIcon.Texture = GD.Load<Texture2D>(iconPath);


		//Parte da exibição de caractéres aos poucos
		_dialogText.VisibleCharacters = 0;
		while (_dialogText.VisibleRatio < 1)
		{
			// A sintaxe do 'await' em C# para esperar um sinal (como 'timeout') é esta:
			await ToSignal(GetTree().CreateTimer(_textVel), Timer.SignalName.Timeout);
			
			_dialogText.VisibleCharacters++; // _dialogText.visible_characters += 1
		}
	}
}
