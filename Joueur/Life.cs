using Godot;

public partial class Life : Node
{
	[Signal] public delegate void OnLifeChangedEventHandler(float newLife);
	
	[Export] 
	private float _maxLife = 100f;
	
	private float _currentLife = 0;
	
	public float LifePrecent => _currentLife / _maxLife;
	
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
		
		//Appeler le signal
		EmitSignal(SignalName.OnLifeChanged, _currentLife);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		//GD.Print(_currentLife);
	}
}
