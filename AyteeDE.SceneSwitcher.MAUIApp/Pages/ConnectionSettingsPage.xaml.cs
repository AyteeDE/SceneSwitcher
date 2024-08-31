using AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

namespace AyteeDE.SceneSwitcher.MAUIApp.Pages;

public partial class ConnectionSettingsPage : ContentPage
{
	private EndpointConfigurationViewModel _viewModel;
	public ConnectionSettingsPage()
	{
		InitializeComponent();
		ConfigurationManager configurationManager = new ConfigurationManager();
		_viewModel = new EndpointConfigurationViewModel(configurationManager.Configuration.EndpointConfiguration);
		BindingContext = _viewModel;
	}
}