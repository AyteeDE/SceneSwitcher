using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AyteeDE.SceneSwitcher.Configuration.Application;
using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

public class ApplicationSceneSwitcherSceneViewModel : INotifyPropertyChanged
{
    private ApplicationSceneSwitcherScene _scene;
    public ApplicationSceneSwitcherSceneViewModel(ApplicationSceneSwitcherScene scene)
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
    public string ProcessNameOrDefaultView
    {
        get
        {
            if(String.IsNullOrWhiteSpace(_scene.ProcessName))
            {
                return "Default";
            }
            else
            {
                return _scene.ProcessName;
            }
        }
    }
    public string ProcessName
    {
        get => _scene.ProcessName;
        set
        {
            if(_scene.ProcessName != value)
            {
                _scene.ProcessName = value;
                OnPropertyChanged(nameof(ProcessName));
                OnPropertyChanged(nameof(ProcessNameOrDefaultView));
            }
        }
    }
    public int Priority
    {
        get => _scene.Priority;
        set
        {
            if(_scene.Priority != value)
            {
                _scene.Priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }
    }
    public bool NeedsFocus
    {
        get => _scene.NeedsFocus;
        set
        {
            if(_scene.NeedsFocus != value)
            {
                _scene.NeedsFocus = value;
                OnPropertyChanged(nameof(NeedsFocus));
            }
        }
    }
    public bool UseWindowTitleInsteadOfProcessName
    {
        get => _scene.UseWindowTitleInsteadOfProcessName;
        set
        {
            if(_scene.UseWindowTitleInsteadOfProcessName != value)
            {
                _scene.UseWindowTitleInsteadOfProcessName = value;
                OnPropertyChanged(nameof(UseWindowTitleInsteadOfProcessName));
            }
        }
    }
    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
