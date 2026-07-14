using Godot;
using System;

public partial class ScoreManager : Node
{
    public static int HighScore {get;set;}

 
    public override void _Ready()
    {
        SignalHub.Instance.OnNewHeight += OnNewHeight;
    }

    private void OnNewHeight(int height)
    {
        if (height > HighScore)
        {
            HighScore = height;
        }
    }
    public override void _ExitTree() // To avoid Node disposed error
	{
    	SignalHub.Instance.OnNewHeight -= OnNewHeight;
	}
}
