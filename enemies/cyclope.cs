using Godot;
using System;

public partial class cyclope : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 30;
	//[Export]
	//public float Limit { get; set; } = 0.5f;
	
	private AnimationPlayer animations;
	
	//private Vector2 startPosition;
	//private Vector2 endPosition;
	
	private bool isDead = false;
	
	
	[Export]
	private joueur player; // Référence au joueur
	public Vector2 PlayerPosition { get; private set; }
	[Export] 
	private float _attackDistance = 15f;
	[Export] 
	private float _attackDamagePerSeconds = 10f;
	[Export]
	private float _followDistance = 50f;
	private bool isMoving = true; // Variable pour suivre l'état du mouvement 

	public override void _Ready()
	{
		/*startPosition = Position;
		endPosition = startPosition + new Vector2(0, 3 * 16);*/
		animations = GetNode<AnimationPlayer>("AnimationPlayer");
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
			//GD.Print("Je suis loin");
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
		//GD.Print("attaque");
		
		player.Life.Damage((float)delta * _attackDamagePerSeconds);
	}

	private void GoToPlayer(Vector2 deltaPlayerPosition)
	{
		//GD.Print("Go To");
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
		if (isDead) return;
		//UpdateVelocity();
		MoveAndSlide();
		UpdateAnimation();
	}

	private async void _on_hurt_box_area_entered(Area2D area)
	{
		if (area == GetNode<Area2D>("hurtBox")) return;
		isDead = true;
		GD.Print("cyclope hit");
		animations.Play("death");
		await ToSignal(animations, "animation_finished");
		QueueFree();
	}
}
