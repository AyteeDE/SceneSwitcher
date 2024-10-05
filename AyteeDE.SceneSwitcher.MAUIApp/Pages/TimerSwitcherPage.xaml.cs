using AyteeDE.SceneSwitcher.Switching;
using AyteeDE.StreamAdapter.Core.Communication;

namespace AyteeDE.SceneSwitcher.MAUIApp.Pages;

public partial class TimerSwitcherPage : ContentPage
{
	private bool _isRunning;
	private TimerSceneSwitcher _switcher;
	public TimerSwitcherPage()
	{
		InitializeComponent();
		ConfigurationManager configurationManager = new ConfigurationManager();
		_switcher = new TimerSceneSwitcher(configurationManager.Configuration.EndpointConfiguration, configurationManager.Configuration.TimerSceneSwitcherConfig);
		_switcher.OnSceneSwitched += OnSceneSwitched;
	}

    private void OnSceneSwitched(object? sender, SceneSwitchingEventArgs e)
    {
        System.Console.WriteLine($"Switched to {e.Scene.Name}");
    }

    private void Toggle_Clicked(object sender, EventArgs e)
	{
		if(_isRunning)
		{
			_switcher.StopSwitching();
			cmdToggle.Text = "Start";
		}
		else
		{
			_switcher.StartSwitching();
			cmdToggle.Text = "Stop";
		}
	}
}