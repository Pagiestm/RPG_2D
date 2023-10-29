using Godot;
using System;

public partial class HUD : Control
{	
	[Export]
	private Life _life = null;
	[Export]
	private ProgressBar bar = null;
	
	public override void _Ready()
	{
		_life.OnLifeChanged += OnLifeChanged;
	}
	
	private void OnLifeChanged(float newLife)
	{
		bar.Value = _life.LifePrecent;
	}
}
