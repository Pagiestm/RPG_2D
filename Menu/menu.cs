using Godot;
using System;

public partial class menu : Control
{
	public Node simultaneousScene;
	
	public override void _Ready()
	{
		simultaneousScene = ResourceLoader.Load<PackedScene>("res://Monde/monde.tscn").Instantiate();
	}

	private void _on_sart_button_pressed()
	{
		if (simultaneousScene.GetParent() == null)
		{
			GetTree().Root.AddChild(simultaneousScene);
		}
	}
	
	private void _on_quit_button_pressed()
	{
		GetTree().Quit();
	}
}
