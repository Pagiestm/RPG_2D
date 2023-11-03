using Godot;

public partial class Weapon : Node2D
{
	private Area2D weapon;
	private bool isAttacking = false; // Marqueur d'attaque avec l'arme

	public override void _Ready()
	{
		if (GetChildCount() > 0)
			weapon = GetChild(0) as Area2D;
	}

	public void Enable()
	{
		if (weapon == null)
			return;

		Visible = true;
		weapon.Call("Enable");
		isAttacking = true; // Marquer l'attaque avec l'arme
	}

	public void Disable()
	{
		if (weapon == null)
			return;

		Visible = false;
		weapon.Call("Disable");
		isAttacking = false; // Marquer la fin de l'attaque avec l'arme
	}

	public bool IsAttacking()
	{
		return isAttacking;
	}
}
