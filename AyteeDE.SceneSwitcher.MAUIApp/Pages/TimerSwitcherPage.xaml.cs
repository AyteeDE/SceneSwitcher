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
		MainThread.BeginInvokeOnMainThread(() => {
			lblLog.Text = $"Switched to {e.Scene.Name}\r\n" + lblLog.Text;
		});
    }
	protected override void OnDisappearing() 
	{
		if(_isRunning)
		{
			Toggle_Clicked(this, null);
		}
	}
    private void Toggle_Clicked(object sender, EventArgs e)
	{
		if(_isRunning)
		{
			_switcher.StopSwitching();
			_isRunning = false;
			cmdToggle.Text = "Start";
		}
		else
		{
			_switcher.StartSwitching();
			_isRunning = true;
			cmdToggle.Text = "Stop";
		}
	}
}