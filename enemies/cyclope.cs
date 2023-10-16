using Godot;
using System;

public partial class cyclope : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 20;
	[Export]
	public float Limit { get; set; } = 0.5f;
	
	private AnimatedSprite2D animations;
	
	private Vector2 startPosition;
	private Vector2 endPosition;

	public override void _Ready()
	{
		startPosition = Position;
		endPosition = startPosition + new Vector2(0, 3 * 16);
		animations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public void ChangeDirection()
	{
		var tempEnd = endPosition;
		endPosition = startPosition;
		startPosition = tempEnd;
	}

	public void UpdateVelocity()
	{
		var moveDirection = endPosition - Position;
		if (moveDirection.Length() < Limit)
		{
			ChangeDirection();
		}
		
		Velocity = moveDirection.Normalized() * Speed;
	}

	 public void UpdateAnimation()
	{
		string animationString = "walkUp";
		if (Velocity.Y > 0)
		{
			animationString = "walkDown";
		}

		animations.Play(animationString);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		UpdateVelocity();
		MoveAndSlide();
		UpdateAnimation();
	}
}
