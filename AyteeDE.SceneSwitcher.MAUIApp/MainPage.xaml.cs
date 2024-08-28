﻿using AyteeDE.SceneSwitcher.MAUIApp.Pages;

namespace AyteeDE.SceneSwitcher.MAUIApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
	public async void Settings_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SettingsPage());
	}
}

