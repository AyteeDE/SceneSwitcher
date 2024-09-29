using System.ComponentModel;
using System.Runtime.CompilerServices;
using AyteeDE.SceneSwitcher.Configuration.Timer;
using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

public class TimerSceneSwitcherSceneViewModel : INotifyPropertyChanged
{
    private TimerSceneSwitcherScene _scene;
    public TimerSceneSwitcherSceneViewModel(TimerSceneSwitcherScene scene)
    {
        _scene = scene;
    }
    public Scene Scene
    {
        get => _scene.Scene;
        set
        {
            if(_scene.Scene == null || !_scene.Scene.Equals(value))
            {
                _scene.Scene = value;
                OnPropertyChanged(nameof(Scene));
            }
        }
    }
    public int Position
    {
        get => _scene.Position;
        set
        {
            if(_scene.Position != value)
            {
                _scene.Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
    }
    public int DurationOverride
    {
        get => _scene.DurationOverride;
        set
        {
            if(_scene.DurationOverride != value)
            {
                _scene.DurationOverride = value;
                OnPropertyChanged(nameof(DurationOverride));
            }
        }
    }
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
