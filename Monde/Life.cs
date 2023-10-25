using Godot;

public partial class Life : Node
{
	[Export] 
	private float _maxLife = 100f;
	
	private float _currentLife = 0;
	
	 public void Damage(float amount)
	{
		ModifyLife(-amount);
	}

	public void Heal(float amount)
	{
		ModifyLife(amount);
	}
	
	public override void _Ready()
	{
		_currentLife = _maxLife;
	}
	
	 private void ModifyLife(float amount)
	{
		_currentLife += amount;
		_currentLife = Mathf.Clamp(_currentLife, 0f, _maxLife);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		GD.Print(_currentLife);
	}
}
