using AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

namespace AyteeDE.SceneSwitcher.MAUIApp.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}
	public void ConnectionSettings_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new ConnectionSettingsPage());
	}
	public void ApplicationSettings_Clicked(Object sender, EventArgs e)
	{
		Navigation.PushAsync(new ApplicationSettingsPage());
	}
	public void TimerSettings_Clicked(Object sender, EventArgs e)
	{
		Navigation.PushAsync(new TimerSettingsPage());
	}
}