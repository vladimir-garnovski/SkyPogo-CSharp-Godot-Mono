using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Platform : Node3D
{
	[Export] private Timer _vanishTimer; // Timer for the disappearing of the platform
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private Area3D _area3D;
	[Export] private float _waitTime = 4.0f;
	[Export] private AudioStreamPlayer _landEffect;

	private bool _timerStarted = false; // Is Hit (?)

	
	//[Signal]
	//public delegate void OnNewPlatformEventHandler(Vector3 PlatformPos); // New platform custom signal

	public override void _Ready()
	{
		_vanishTimer.Timeout += OnVanishTimerTimeout;
		_animationPlayer.AnimationFinished += OnAnimationFinished;
		_area3D.BodyEntered += OnArea3DBodyEntered;
	}

	private void OnVanishTimerTimeout()
    {
        _animationPlayer.Play("vanish");
    }
	private void OnArea3DBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Player") && !_timerStarted) 
		{
			_timerStarted = true;
			_vanishTimer.Start(_waitTime *  0.75 + 1.2 *GD.Randf() ); // WaitTime *   0.75 to 1.2
			SignalHub.EmitOnNewPlatfrom(Position);
			_landEffect.Play();
		}
		
    }
	private void OnAnimationFinished(StringName animName)
    {
		QueueFree();	
    }
	

}
