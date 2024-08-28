using AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

namespace AyteeDE.SceneSwitcher.MAUIApp.Pages;

public partial class ConnectionSettingsPage : ContentPage
{
	private EndpointConfigurationViewModel viewModel;
	public ConnectionSettingsPage()
	{
		InitializeComponent();
		ConfigurationManager configurationManager = new ConfigurationManager();
		viewModel = new EndpointConfigurationViewModel(configurationManager.Configuration.EndpointConfiguration);
		BindingContext = viewModel;
	}
}