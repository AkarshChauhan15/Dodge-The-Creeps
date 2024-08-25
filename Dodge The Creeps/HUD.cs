using Godot;
using System;
using System.Xml.Serialization;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();
    [Signal] 
    public delegate void ChangeColourEventHandler();

    public Timer timer;
    public Label message;
    public override void _Ready()
    {
        timer = GetNode<Timer>("MessageTimer");
        message = GetNode<Label>("Message");
    }
    
    public void ShowMessage(string text)
    {
        message.Text = text;
        message.Show();

        timer.Start();
    }

    async public void ShowGameOver()
    {
        ShowMessage("Game Over!");
        await ToSignal(timer, Timer.SignalName.Timeout);

        message.Text = "Dodge the Creeps!";
        message.Show();
        GetNode<Button>("ChangeColour").Show();
        GetNode<Button>("QuitButton").Show();

        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        GetNode<Button>("StartButton").Show();
    }
    public void UpdateScore(int score)
    {
        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }
    private void OnStartButtonPressed()
    {
        GetNode<Button>("StartButton").Hide();
        GetNode<Button>("QuitButton").Hide();
        GetNode<Button>("ChangeColour").Hide();
        EmitSignal(SignalName.StartGame);
    }

    private void OnChangeColourPressed()
    {
        EmitSignal(SignalName.ChangeColour);
    }
    private void OnQuitButtonPressed()
    {
        GetNode<ConfirmationDialog>("QuitDialog").Popup();
    }
    private void OnQuitAccepted()
    {
        GetTree().Quit();
    }
    private void OnMessageTimerTimeout()
    {
        message.Hide();
    }
}
