using Godot;
using System;

public partial class FullScreenImage : TextureRect
{
    public static FullScreenImage Instance;

    public void FadeImage(Texture2D texture, double duration)
    {
        Tween tween = CreateTween();
        if (texture != default && Texture == default) //Fade in
        {
            Texture = texture;
            Modulate = Colors.Transparent;
            tween.TweenProperty(this, "modulate", Colors.White, duration);
        }

        else if (texture == default && Texture != default) //Fade out
        {
            Modulate = Colors.White;
            tween.TweenProperty(this, "modulate", Colors.Transparent, duration);
            tween.TweenCallback(Callable.From(() => Texture = texture));
        }

        else if (texture != default && Texture != default) //Fade-in-between
        {
            Modulate = Colors.White;
            tween.TweenProperty(this, "modulate", Colors.Black, duration / 2);
            tween.TweenCallback(Callable.From(() => Texture = texture));
            tween.TweenProperty(this, "modulate", Colors.White, duration / 2);
        }

    }

    public void ZoomImage(float zoom, Vector2I offset, double duration)
    {
        StretchMode = StretchModeEnum.KeepAspect;
        Tween tween = CreateTween();
        tween.SetParallel();
        tween.TweenProperty(this, "scale", Vector2.One * zoom, duration).SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(this, "pivot_offset", Size / 2 + offset, duration).SetTrans(Tween.TransitionType.Sine);
    }

    public void RemoveZoom(double duration) => ZoomImage(1, Vector2I.Zero, duration);

    public override void _Ready()
    {
        base._Ready();
        if (Instance == null) Instance = this;
        else if (Instance != this) { QueueFree(); return; }

    }

}
