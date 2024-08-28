
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using AyteeDE.SceneSwitcher.Configuration;
using AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

namespace AyteeDE.SceneSwitcher.MAUIApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		ConfigurationManager configurationManager = new ConfigurationManager();
		configurationManager.TryLoadConfig();
		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState? activationState)
    {
		var window = base.CreateWindow(activationState);
		const int newWidth= 800;
		const int newHeight= 1200;
		window.Width = newWidth;
		window.MaximumWidth = newWidth;
		window.MinimumWidth = newWidth;
		window.Height = newHeight;
		window.MaximumHeight = newHeight;
		window.MinimumHeight = newHeight;
		return window;
    }

	
}
