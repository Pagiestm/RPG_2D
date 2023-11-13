using Godot;
using System;

public partial class cyclope : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 30;
	
	private AnimationPlayer animations;
	
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
	private int health = 100;
	
	 public ProgressBar ProgressBar { get; private set; }
	
	public override void _Ready()
	{
		animations = GetNode<AnimationPlayer>("AnimationPlayer");
		ProgressBar = GetNode<ProgressBar>("ProgressBar");
		
		player.AddCyclope(this);
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
	
	public void Attacked(float damage)
	{
		health -= (int)damage;
		//GD.Print(health);
		ProgressBar.Value = Mathf.Max(0, health);
		if (health <= 0)
		{
			isDead = true;
			animations.Play("death");
		}
	}

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
		if (isDead && ! animations.IsPlaying()) // Vérifie si l'animation de mort est terminée
		{
			QueueFree(); // Libère la mémoire de l'ennemi
			return;
		}
		
		if (isDead) return;
		MoveAndSlide();
		UpdateAnimation();
	}
}
