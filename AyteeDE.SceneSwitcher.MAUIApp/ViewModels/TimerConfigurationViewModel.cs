using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AyteeDE.SceneSwitcher.Configuration.Timer;
using AyteeDE.StreamAdapter.Core.Communication;
using AyteeDE.StreamAdapter.Core.Configuration;
using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

public class TimerConfigurationViewModel : INotifyPropertyChanged
{
    private TimerSceneSwitcherConfig _timerConfig;
    private EndpointConfiguration _endpointConfig;
    private List<Scene> _adapterScenes;
    private ObservableCollection<TimerSceneSwitcherSceneViewModel> _observableScenesList = new ObservableCollection<TimerSceneSwitcherSceneViewModel>();

    public TimerConfigurationViewModel(TimerSceneSwitcherConfig timerConfiguration, EndpointConfiguration endpointConfiguration)
    {
        _timerConfig = timerConfiguration;
        _endpointConfig = endpointConfiguration;
        foreach(var scene in _timerConfig.Scenes.OrderBy(s=>s.Position))
        {
            TimerSceneSwitcherSceneViewModel sceneViewModel = new TimerSceneSwitcherSceneViewModel(scene);
            Scenes.Add(sceneViewModel);
        }
        GetAdapterScenes();
    }
    private async void GetAdapterScenes()
    {
        var adapter = AdapterFactory.CreateInstance(_endpointConfig);
        try
        {
            if(!await adapter.ConnectAsync())
            {
                throw new Exception("Connection could not be established.");
            }
            AdapterScenes = await adapter.GetScenes();
        }
        catch(Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error","Scenes could not be loaded, no connection to streaming client.", "OK");
        }
    }
    public int Interval
    {
        get => _timerConfig.Interval;
        set
        {
            if(_timerConfig.Interval != value)
            {
                _timerConfig.Interval = value;
                OnPropertyChanged(nameof(Interval));
            }
        }
    }
    public bool IsRandom
    {
        get => _timerConfig.IsRandom;
        set
        {
            if(_timerConfig.IsRandom != value)
            {
                _timerConfig.IsRandom = value;
                OnPropertyChanged(nameof(IsRandom));
            }
        }
    }
    public ObservableCollection<TimerSceneSwitcherSceneViewModel> Scenes
    {
        get => _observableScenesList;
        set
        {
            _observableScenesList = value;
            OnPropertyChanged(nameof(Scenes));
        }
    }
    private TimerSceneSwitcherSceneViewModel _editScene;
    public TimerSceneSwitcherSceneViewModel EditScene
    {
        get => _editScene;
        set
        {
            _editScene = value;
            OnPropertyChanged(nameof(EditScene));
            OnPropertyChanged(nameof(IsEditEnabled));
        }
    }
    public List<Scene> AdapterScenes
    {
        get => _adapterScenes;
        set
        {
            _adapterScenes = value;
            OnPropertyChanged(nameof(AdapterScenes));
        }
    }
    public bool IsEditEnabled => EditScene != null;
    public ICommand SaveCommand => new Command(SaveConfig);
    public ICommand AddNewRuleCommand => new Command(AddNewRule);
    public ICommand RemoveRuleCommand => new Command(RemoveRule);
    public ICommand MoveRuleUpCommand => new Command(MoveRuleUp);
    public ICommand MoveRuleDownCommand => new Command(MoveRuleDown);
    private void SaveConfig()
    {
        var scenesList = new List<TimerSceneSwitcherScene>();
        foreach(var sceneViewModel in Scenes)
        {
            var scene = new TimerSceneSwitcherScene
            {
                Scene = sceneViewModel.Scene,
                Position = sceneViewModel.Position,
                DurationOverride = sceneViewModel.DurationOverride
            };
            scenesList.Add(scene);
        }

        _timerConfig.Scenes = scenesList;
        ConfigurationManager configurationManager = new ConfigurationManager();
        configurationManager.UpdateTimerConfiguration(_timerConfig);
        EditScene = null;
    }
    private void AddNewRule()
    {
        EditScene = new TimerSceneSwitcherSceneViewModel(new TimerSceneSwitcherScene());
        EditScene.Scene = _adapterScenes[0];
        EditScene.Position = Scenes.Count;
        Scenes.Add(EditScene);
        SortSceneCollection();
    }
    private void RemoveRule()
    {
        if(Scenes.Contains(EditScene))
        {
            Scenes.Remove(EditScene);
            EditScene = null;
            OnPropertyChanged(nameof(Scenes));
        }
    }
    private void MoveRuleUp()
    {
        MoveRule(-1);	
    }
    private void MoveRuleDown()
    {
        MoveRule(1);
    }
    private void MoveRule(int direction)
    {
        int currentIndex = Scenes.IndexOf(EditScene);
        int newIndex = currentIndex + direction;
        if(newIndex >= 0 && newIndex < Scenes.Count)
        {
            Scenes.Move(currentIndex, newIndex);
            SortSceneCollection();
            EditScene = Scenes[newIndex];
        }
    }
    private void SortSceneCollection()
    {
        var sortedSceneCollection = new ObservableCollection<TimerSceneSwitcherSceneViewModel>();
        int index = 0;
        foreach(var scene in Scenes)
        {
            scene.Position = index;
            index++;
            sortedSceneCollection.Add(scene);
        }
        Scenes = sortedSceneCollection;
    }
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
