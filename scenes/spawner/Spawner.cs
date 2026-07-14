using Godot;
using System;


public partial class Spawner : Node
{
	
	[Export] private PackedScene[] _platformScenes;

	private readonly Vector2 OFFSET_SIDE = new Vector2(1.7f,4.0f);
	private readonly Vector2 OFFSET_UP = new Vector2(2.7f,4.5f);

    public override void _Ready()
    {
		SignalHub.Instance.OnNewPlatform += OnSpawnPlatform;
    }

	public void OnSpawnPlatform(Vector3 oldPlatformPos)
	{
		int randomPlatformIndex = new Random().Next(0,_platformScenes.Length);

		Platform newPlatform =  _platformScenes[randomPlatformIndex].Instantiate<Platform>();
		newPlatform.Position = oldPlatformPos + new Vector3(
															GetRandomOffset(OFFSET_SIDE),
															(float)GD.RandRange(OFFSET_UP.X,OFFSET_UP.Y),
															GetRandomOffset(OFFSET_SIDE)
		);
		AddChild(newPlatform);
	}
	private float GetRandomOffset(Vector2 offsetRange)
	{
		float magnitude = (float)GD.RandRange(offsetRange.X,offsetRange.Y);
		
		if (GD.Randf() < 0.5) { return magnitude;}
		else 				  { return -magnitude;}

	}

	// Godot calls this automatically when the node is being removed/destroyed
	// ASK richard
	public override void _ExitTree() // To avoid Node disposed error
	{
   		 // Unsubscribe so the old Spawner doesn't ghost-fire!
    	SignalHub.Instance.OnNewPlatform -= OnSpawnPlatform;
	}
	// ---------
}

