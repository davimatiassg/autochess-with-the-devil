using Godot;
using System;

public partial class MainMenu : Control
{
    public Action playGame;

    public Action exitGame;

    public void OnButtonStartPressed()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        playGame();
        QueueFree();
    }
    public void OnButtonExitPressed()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        exitGame();
        QueueFree();
    }

    public override void _Ready()
    {
        base._Ready();
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
        
}
