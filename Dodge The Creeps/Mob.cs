using Godot;
using System;

public partial class Mob : RigidBody2D
{
    public override void _Ready()
    {
        var Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        string[] mobTypes = Sprite.SpriteFrames.GetAnimationNames();
        Sprite.Play(mobTypes[GD.Randi() % mobTypes.Length]);
    }
    
    private void ScreenExited()
    {
        QueueFree();
    }
}
