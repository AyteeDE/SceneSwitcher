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
}