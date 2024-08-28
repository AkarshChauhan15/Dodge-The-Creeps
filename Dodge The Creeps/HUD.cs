using Godot;
using System;
using System.Xml.Serialization;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();
    [Signal] 
    public delegate void ChangeColourEventHandler();
    [Signal]
    public delegate void MenuEventHandler();

    public Timer timer;
    public Label message;
    public override void _Ready()
    {
        timer = GetNode<Timer>("MessageTimer");
        message = GetNode<Label>("Message");
        GetNode<Button>("StartButton").GrabFocus();
    }
    
    public void ShowMessage(string text)
    {
        message.Text = text;
        message.Show();

        timer.Start();
    }

    async public void ShowGameOver(bool t)
    {
        if (t)
        {
            ShowMessage("Game Over!");
            await ToSignal(timer, Timer.SignalName.Timeout);
        }
        message.Text = "Dodge the Creeps!";
        message.Show();
        GetNode<Button>("ChangeColour").Show();

        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetNode<Button>("StartButton").Show();
        GetNode<Button>("QuitButton").Show();
        GetNode<Button>("StartButton").GrabFocus();
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
    public void ResumeGame()
    {
        Engine.TimeScale = 1.0;
        GetNode<Control>("PauseMenu").Hide();
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}
