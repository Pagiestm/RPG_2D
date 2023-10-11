using Godot;
using System;

public partial class joueur : CharacterBody2D
{
[Export]
	public int Speed { get; set; } = 115;

	public void GetInput()
	{

		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDirection * Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
}
