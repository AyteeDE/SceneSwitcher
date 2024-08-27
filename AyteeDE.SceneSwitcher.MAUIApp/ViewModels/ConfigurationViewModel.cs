using System;
using System.ComponentModel;

namespace AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

public class ConfigurationViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool HasValidEndpointConfiguration { get; set; }
    public bool HasValidTimerConfiguration { get; set; }
    public bool HasValidApplicationConfiguration { get; set; }
}
