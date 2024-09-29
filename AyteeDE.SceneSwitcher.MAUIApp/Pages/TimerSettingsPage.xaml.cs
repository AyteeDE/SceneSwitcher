using AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

namespace AyteeDE.SceneSwitcher.MAUIApp.Pages;

public partial class TimerSettingsPage : ContentPage
{
	private TimerConfigurationViewModel _viewmodel;
	public TimerSettingsPage()
	{
		InitializeComponent();
		ConfigurationManager configurationManager = new ConfigurationManager();
		_viewmodel = new TimerConfigurationViewModel(configurationManager.Configuration.TimerSceneSwitcherConfig, configurationManager.Configuration.EndpointConfiguration);
		BindingContext = _viewmodel;
	}
	private void Scene_SelectedIndexChanged(object sender, EventArgs e)
	{
		_viewmodel.OnPropertyChanged(nameof(_viewmodel.EditScene));
	}
}