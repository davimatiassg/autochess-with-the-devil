using Godot;
using System;

public partial class GameCamera : Camera3D
{
    // Propriedades exportadas para ajuste no editor
    [Export] public float MouseSensitivity { get; set; } = 0.1f; // Sensibilidade menor para C#
    [Export] private float _maxDistance = 1000f;

    public override void _Ready()
    {
        // Capturar e esconder o cursor do mouse
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    
    private Card lastCardLooked = null;
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            // Rotação da câmera
            RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * MouseSensitivity));

            float change = -mouseMotion.Relative.Y * MouseSensitivity;
            float newCameraAngle = Mathf.RadToDeg(Rotation.X) + change;

            newCameraAngle = Mathf.Clamp(newCameraAngle, -90.0f, 90.0f);

            Rotation = new Vector3(Mathf.DegToRad(newCameraAngle), Rotation.Y, Rotation.Z);


            // Raycast de Detecção do mouse

            Vector2 screenCenter = GetViewport().GetVisibleRect().Size / 2f;

            Vector3 from = ProjectRayOrigin(screenCenter);
            Vector3 to = from + ProjectRayNormal(screenCenter) * _maxDistance;

            var spaceState = GetWorld3D().DirectSpaceState;
            var query = PhysicsRayQueryParameters3D.Create(from, to);
            query.CollideWithAreas = true;
            query.CollideWithBodies = false;
            var result = spaceState.IntersectRay(query);

            if (result.Count > 0)
            {
                var collider = (Node3D)result["collider"];
                var cardLooked = collider.GetParent<Card>();
                GD.Print(cardLooked);
                if (cardLooked != lastCardLooked)
                {
                    lastCardLooked?.OnMouseExited();
                    cardLooked?.OnMouseEntered();

                    lastCardLooked = cardLooked;
                }
            }
            else {lastCardLooked?.OnMouseExited(); lastCardLooked = null; }

        }












        // Liberar o cursor do mouse com "ui_cancel" (Escape)
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
                Input.MouseMode = Input.MouseModeEnum.Visible;
            else
                Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }
    

}
