
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
		DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
		ConfigurationManager configurationManager = new ConfigurationManager();
		configurationManager.TryLoadConfig();
		MainPage = new AppShell();
	}

    private void DeviceDisplay_MainDisplayInfoChanged(object? sender, DisplayInfoChangedEventArgs e)
    {
		var window = base.Windows.FirstOrDefault();
		SetWindowSize(window);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {

		var window = base.CreateWindow(activationState);
		SetWindowSize(window);
		// UIScreen uIScreen = new UIScreen();
		// int width = (int)uIScreen.Bounds.Width;
		// int height = (int)uIScreen.Bounds.Height;

		return window;
    }
	private void SetWindowSize(Window window)
	{
		int screenwidth = 0;
		int screenheight = 0;

		screenwidth = (int)DeviceDisplay.MainDisplayInfo.Width;
		screenheight = (int)DeviceDisplay.MainDisplayInfo.Height;

		const int defaultwidth = 500;
		const int defaultheight = 750;
		int newWidth = defaultwidth *  (screenwidth/1920);
		int newHeight = defaultheight * (screenheight/1080);

		window.Width = newWidth;
		window.MaximumWidth = newWidth;
		window.MinimumWidth = newWidth;
		window.Height = newHeight;
		window.MaximumHeight = newHeight;
		window.MinimumHeight = newHeight;
	}
	
}
