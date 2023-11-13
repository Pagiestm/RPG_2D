using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class joueur : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 55;
	private AnimationPlayer animations;
	private Node2D weapon;
	
	[Export]
	private Life _life = null;
	
	private List<cyclope> cyclopes = new List<cyclope>();
	
	public Life Life => _life;
	
	private string lastAnimDirection = "Down";
	
	private bool isAttacking = false;
	
	public Vector2 PlayerPosition { get; private set; }
	
	public override void _Ready()
	{
		animations = GetNode<AnimationPlayer>("AnimationPlayer");
		weapon = GetNode<Node2D>("weapon");
		weapon.Visible = false;
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDirection * Speed;

		if (Input.IsActionJustPressed("attack"))
	{
		animations.Play("attack" + lastAnimDirection);
		isAttacking = true;
		weapon.Visible = true;
		CallDeferred(nameof(WaitForAttackAnimation));

		// Détecter les cyclopes touchés par l'attaque
		foreach (cyclope currentCyclope in cyclopes)
		{
			if (IsCyclopeInRange(currentCyclope))
			{
				currentCyclope.Attacked(25); // 25 est la valeur des dégâts
			}
		}
	}
}

private bool IsCyclopeInRange(cyclope currentCyclope)
{
	float attackRange = 20.0f; // Ajustez la portée de l'attaque selon vos besoins
	float distanceToCyclope = (currentCyclope.GlobalPosition - GlobalPosition).Length();

	return distanceToCyclope < attackRange;
}
	
	// Ajout d'une méthode pour ajouter un cyclope à la liste
	public void AddCyclope(cyclope newCyclope)
	{
		cyclopes.Add(newCyclope);
	}

	private async void WaitForAttackAnimation()
	{
		await ToSignal(animations, "animation_finished");
		weapon.Visible = false;
		isAttacking = false;
	}

	
	public void UpdateAnimation()
	{	
			
		if (isAttacking)
			return;
				
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
				lastAnimDirection = direction;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
		UpdateAnimation();
		
		PlayerPosition = GlobalPosition;
	}
}
