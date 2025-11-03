using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

public partial class PlayerCamera : Camera3D
{

    // Propriedades exportadas para ajuste no editor
    [Export] public float MouseSensitivity { get; set; } = 0.1f; // Sensibilidade menor para C#
    [Export] private float _maxDistance = 1000f;

    private Card lastCardLooked = null;
    private Card pickedCard = null;

    public void HoverCard(Card cardLooked)
    {      
        if (cardLooked != lastCardLooked)
        {
            lastCardLooked?.OnMouseExited();
            cardLooked?.OnMouseEntered();

            lastCardLooked = cardLooked;

        }
    }
    public void PickupCard(Card card)
    {
        pickedCard = card;
        PlayerHand.PickCard(pickedCard);
    }

    public void DropCard()
    {

        PlayerHand.DropCard(pickedCard);
        pickedCard = null;
    }

    public void PlayCard(Card card, TabletopTile tile)
    {
        PlayerHand.PlayCard(card, tile);
        pickedCard = null;

    }



    public Godot.Collections.Dictionary RaycastScene()
    {
        Vector2 screenCenter = GetViewport().GetVisibleRect().Size / 2f;

        Vector3 from = ProjectRayOrigin(screenCenter);
        Vector3 to = from + ProjectRayNormal(screenCenter) * _maxDistance;

        var spaceState = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollideWithAreas = true;
        query.CollideWithBodies = false;
        return spaceState.IntersectRay(query);
            
    
    }

    public override void _Ready()
    {
        // Capturar e esconder o cursor do mouse
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    
    
    
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
                Input.MouseMode = Input.MouseModeEnum.Visible;
            else
                Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        if (!PlayerHand.Instance.AllowPlay) return;
        
        if (@event is InputEventMouseMotion mouseMotion)
        {
            // Rotação da câmera
            RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * MouseSensitivity));

            float change = -mouseMotion.Relative.Y * MouseSensitivity;
            float newCameraAngle = Mathf.RadToDeg(Rotation.X) + change;

            newCameraAngle = Mathf.Clamp(newCameraAngle, -90.0f, 90.0f);

            Rotation = new Vector3(Mathf.DegToRad(newCameraAngle), Rotation.Y, Rotation.Z);


            // Raycast de Detecção do mouse

            var result = RaycastScene();

            if (result.Count > 0)
            {

                if (pickedCard == null)
                {
                    var card = ((Node3D)result["collider"]) as Card;
                    if (card != null) HoverCard(card);
                }
            }
            else
            {
                lastCardLooked?.OnMouseExited();
                lastCardLooked = null;
            }

        }


        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            if (lastCardLooked != null)
            {
                if (pickedCard == null)
                {
                    PickupCard(lastCardLooked);
                }
            }
        }
        else if (pickedCard != null)
        {
            var result = RaycastScene();

            if (result.Count > 0)
            {
                var tile = ((Node3D)result["collider"]) as TabletopTile;
                if (tile == null) DropCard();
                else
                {
                    if (tile.IsTileValid(pickedCard.effect)) PlayCard(pickedCard, tile);
                    else DropCard(); 
                }
            }
            else DropCard();
        }


    }
    

    

}
