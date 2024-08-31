using AyteeDE.SceneSwitcher.Configuration.Application;
using AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

namespace AyteeDE.SceneSwitcher.MAUIApp.Pages;

public partial class ApplicationSettingsPage : ContentPage
{
	private ApplicationConfigurationViewModel _viewmodel;
	public ApplicationSettingsPage()
	{
		InitializeComponent();
		ConfigurationManager configurationManager = new ConfigurationManager();
		_viewmodel = new ApplicationConfigurationViewModel(configurationManager.Configuration.ApplicationSceneSwitcherConfig, configurationManager.Configuration.EndpointConfiguration);
		BindingContext = _viewmodel;
	}
	private void Scene_SelectedIndexChanged(object sender, EventArgs e)
	{
		_viewmodel.OnPropertyChanged(nameof(_viewmodel.EditScene));
	}
}