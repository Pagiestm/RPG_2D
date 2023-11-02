using Godot;
using System;

public partial class cyclope : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 40;
	//[Export]
	//public float Limit { get; set; } = 0.5f;
	
	private AnimatedSprite2D animations;
	
	//private Vector2 startPosition;
	//private Vector2 endPosition;
	
	
	[Export]
	private joueur player; // Référence au joueur
	public Vector2 PlayerPosition { get; private set; }
	[Export] 
	private float _attackDistance = 30f;
	[Export] 
	private float _attackDamagePerSeconds = 10f;
	[Export]
	private float _followDistance = 50f;
	private bool isMoving = true; // Variable pour suivre l'état du mouvement 

	public override void _Ready()
	{
		/*startPosition = Position;
		endPosition = startPosition + new Vector2(0, 3 * 16);*/
		animations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	//Distance du joueur par rapport à un enemie
	public override void _Process(double delta)
	{
		// Accédez à la position du joueur depuis la classe joueur
		PlayerPosition = player.PlayerPosition;
		Vector2 deltaPlayerPosition = PlayerPosition - GlobalPosition;
		float distanceToPlayer = deltaPlayerPosition.Length();

		//Afficher la distance
		//GD.Print(distanceToPlayer);
		if (distanceToPlayer < _attackDistance)
		{
			Attack(delta);
			return;
		} else {
			GD.Print("Je suis loin");
		}
		// Ajoutez une nouvelle condition pour vérifier la proximité du joueur
		if (distanceToPlayer < _followDistance)
		{
			GoToPlayer(deltaPlayerPosition);
		}
		else
		{
			StopMoving();
		}
	}

	private void Attack(double delta)
	{
		GD.Print("attaque");
		
		player.Life.Damage((float)delta * _attackDamagePerSeconds);
	}

	private void GoToPlayer(Vector2 deltaPlayerPosition)
	{
		GD.Print("Go To");
		Vector2 directionToPlayer = deltaPlayerPosition.Normalized();
		Velocity = directionToPlayer * Speed;
	}

	public void StopMoving()
	{
	isMoving = false;
	Velocity = Vector2.Zero; // Arrêtez le mouvement en définissant la vitesse à zéro
	}

	
	/*public void ChangeDirection()
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
	}*/

	public void UpdateAnimation()
	{
		Vector2 directionToPlayer = PlayerPosition - GlobalPosition;

		// Vérifier si le joueur s'approche suffisamment
		if (directionToPlayer.Length() < _followDistance)
		{
			if (Mathf.Abs(directionToPlayer.X) > Mathf.Abs(directionToPlayer.Y))
			{
				if (directionToPlayer.X > 0)
				{
					animations.Play("walkRight");
				}
				else if (directionToPlayer.X < 0)
				{
					animations.Play("walkLeft");
				}
			}
			else
			{
				if (directionToPlayer.Y > 0)
				{
					animations.Play("walkDown");
				}
				else if (directionToPlayer.Y < 0)
				{
					animations.Play("walkUp");
				}
			}
		}
		else
		{
			animations.Stop(); // Arrêter les animations si le joueur est loin de l'ennemi
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		//UpdateVelocity();
		MoveAndSlide();
		UpdateAnimation();
	}
}
