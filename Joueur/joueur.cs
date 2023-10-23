using Godot;
using System;

public partial class joueur : CharacterBody2D
{
[Export]
	public int Speed { get; set; } = 55;
	private AnimationPlayer animations;
	[Export]
	public int MaxHealth { get; set; } = 3;
	private int currentHealth;
	
	public override void _Ready()
	{
		animations = GetNode<AnimationPlayer>("AnimationPlayer");
		currentHealth = MaxHealth;
	}

	public void GetInput()
	{

		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDirection * Speed;
	}
	
	public void UpdateAnimation()
{
	if (Velocity.Length() == 0)
	{
		animations.Stop();
	}
	else
	{
		string direction = "Down";
		if (Velocity.X < 0.0f)
		{
			direction = "Left";
		}
		else if (Velocity.X > 0.0f)
		{
			direction = "Right";
		}
		else if (Velocity.Y < 0.0f)
		{
			direction = "Up";
		}

		animations.Play("walk" + direction);
	}
}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
		UpdateAnimation();
	}
	
	//Détection enemies
	private void _on_hurt_box_area_entered(Area2D area)
	{
		if (area.Name == "hitBox")
		{
			GD.Print(area.GetParent().Name);
			currentHealth -= 1;
			if (currentHealth < 0)
			{
				currentHealth = MaxHealth;
			}
			GD.Print(currentHealth);
		}
	}
}
