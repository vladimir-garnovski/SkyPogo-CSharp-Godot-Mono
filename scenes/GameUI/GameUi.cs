using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class GameUi : Control
{
    const String SAVE_PATH = "user://sky_enc.cfg";
    const String SECTION = "game";
    const String VALUE_KEY = "score";
    const String PW = "somepassword1";

    [Export] private ColorRect _gameOverRect;
    [Export] private Label _bestHeightLabel;
    [Export] private Label _heightLabel;


    private void LoadScore()
    {
        ConfigFile config = new ConfigFile();
        if (config.LoadEncryptedPass(SAVE_PATH, PW) == Error.Ok)
        {
            ScoreManager.HighScore = config.GetValue(SECTION, VALUE_KEY, 0).AsInt32();
        }
    }
    public override void _Ready()
    {
        SignalHub.Instance.OnGameOver += OnGameOver; // Recieve the signal from SignalHub, upon recieving call OnGameOver()
        SignalHub.Instance.OnNewHeight += OnNewHeight;

        _bestHeightLabel.Text  = "Best:"+ScoreManager.HighScore.ToString();
    }

    private void OnNewHeight(int height)
    {
        
        _heightLabel.Text = height.ToString();
    }

    private void OnGameOver()
    {
       SaveScore();
       _gameOverRect.Visible = true;
       GetTree().Paused = true;
    }
    public override void _UnhandledInput(InputEvent @event)
    {
         if (@event.IsActionPressed("reload"))
        {
            GetTree().ReloadCurrentScene();
            
            
        }
    }
    public override void _EnterTree()
    {
        LoadScore();
        GetTree().Paused = false;
    }
    public override void _ExitTree() // To avoid object disposed error
    {
        SignalHub.Instance.OnGameOver -= OnGameOver; 
        SignalHub.Instance.OnNewHeight -= OnNewHeight;
    }
    private void SaveScore()
    {
        ConfigFile config = new ConfigFile();
        config.SetValue(SECTION,VALUE_KEY,ScoreManager.HighScore);
        config.SaveEncryptedPass(SAVE_PATH,PW);
    }
}
