using Godot;
using System;

public partial class PlayerCam : Camera3D
{
	[Export] private Vector3 _buffer = new Vector3(0,17,13);

	[Export]private Vector3 _basePosition;
	 
	 [Export]private float _smoothSpeed = 2.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SignalHub.Instance.OnNewPlatform += OnNewPlatfrom;
		_basePosition = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (Position.DistanceTo(_basePosition) < 0.01)
			Position = _basePosition;
		else	
			Position = Position.Lerp(_basePosition, _smoothSpeed * (float)delta); // 10%
	}
	private void OnNewPlatfrom(Vector3 newPlatformPos)
	{
		_basePosition = newPlatformPos + _buffer;
		
		
	}
}
