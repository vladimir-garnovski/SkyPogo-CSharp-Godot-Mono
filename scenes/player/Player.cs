using Godot;
using System;


public partial class Player : CharacterBody3D
{
	private const float GRAVITY = 60f;
    private const float JUMP_FORCE = 32.0f;  

	private const float ROTATION_SPEED = 4.0f;
	private const float MOVE_SPEED =  5.0f;
	private const float FALL_OFF_MARGIN = 20.0f;
	private float _fallOffY = 0.0f;
	private bool _fellOff = false;
	private float _bestHeight = 0.0f;
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private AudioStreamPlayer _fallSound;
	

    public override void _Ready()
    {
       _fallOffY = Position.Y - FALL_OFF_MARGIN;
    }
	public override void _PhysicsProcess(double delta)
	{
		HandleGravity(delta);
		HandleMovement();
		HandleRotation(delta);
		MoveAndSlide();
		HandleAnimation();
		UpdateHeight();
		HandleFall();
	}
	private void HandleFall()
	{
		if (!_fellOff && (Position.Y < _fallOffY) ) // In NOT fell off AND the position is below the fall off
		{
			_fellOff = true;
			
			_fallSound.Play();
			_fallSound.Finished += GameOver;
		}
	}
	private void GameOver()
	{
		SignalHub.EmitOnGameOver(); // Emit OnGameOver signal 
	}
	private void HandleGravity(double delta)
	{
		Vector3 velocity = Velocity;
		velocity.Y += -GRAVITY * (float)delta; 	
    	if(IsOnFloor())
		{
			velocity.Y = JUMP_FORCE ;
		}
		Velocity = velocity;
	}
	private void HandleAnimation()
	{
		if (Velocity.Y > 0)
		{
			_animationPlayer.Play("jump");
		}
		else
		{
			_animationPlayer.Play("fall");
		}
	}
	private void HandleRotation(double delta)
	{
		if(Input.IsActionPressed("ui_left"))
		{
			RotateY(ROTATION_SPEED * (float)delta);
		}
		if(Input.IsActionPressed("ui_right"))
		{
			RotateY(-ROTATION_SPEED * (float)delta);
		}
	}
	private void HandleMovement() // Only X and Z
	{
		Vector3 velocity = Velocity;

		Vector3 forward = Transform.Basis.Z * Input.GetActionStrength("ui_up"); // the vector direction we're facing in the Z 
		velocity.X = forward.X * MOVE_SPEED;
		velocity.Z = forward.Z * MOVE_SPEED;

		Velocity = velocity;
	}
	private void UpdateHeight()
	{
		if (Position.Y > _bestHeight)
		{
			_bestHeight = Position.Y;
			
			SignalHub.EmitOnNewHeight((int)_bestHeight);
		}
	}
}
