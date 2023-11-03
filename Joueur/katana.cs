using Godot;
using System;

public partial class Katana : Area2D
{
	private CollisionShape2D shape;

	public override void _Ready()
	{
		shape = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public void Enable()
	{
		shape.Disabled = false;
	}

	public void Disable()
	{
		shape.Disabled = true;
	}
}
