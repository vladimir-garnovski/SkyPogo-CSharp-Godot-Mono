using Godot;
using System;

public partial class SignalHub : Node
{
	[Signal]
	public delegate void OnNewPlatformEventHandler(Vector3 platformPos); // New platform custom signal
	[Signal]
	public delegate void OnGameOverEventHandler(); // Game Over signal
	[Signal]
	public delegate void OnNewHeightEventHandler(int height); // New platform custom signal

	public static SignalHub Instance {get; private set;} // Signal Hub self instance
	
	public override void _Ready()
	{
		Instance = this;
	}
	public static void EmitOnNewPlatfrom(Vector3 platformPos) // Emit OnNewPlatform
	{
		Instance.EmitSignal(SignalName.OnNewPlatform, platformPos );
	}
	public static void EmitOnGameOver()
	{
		Instance.EmitSignal(SignalName.OnGameOver);
	}
	public static void EmitOnNewHeight(int height)
	{
		Instance.EmitSignal(SignalName.OnNewHeight, height);
	}
}
