using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AyteeDE.SceneSwitcher.Configuration.Application;
using AyteeDE.StreamAdapter.Core.Communication;
using AyteeDE.StreamAdapter.Core.Configuration;
using AyteeDE.StreamAdapter.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

public class ApplicationConfigurationViewModel : INotifyPropertyChanged
{
    private ApplicationSceneSwitcherConfig _applicationconfig;
    private EndpointConfiguration _endpointconfig;
    private List<Scene> _adapterScenes;
    private ObservableCollection<ApplicationSceneSwitcherSceneViewModel> _observableScenesList = new ObservableCollection<ApplicationSceneSwitcherSceneViewModel>();
    public ApplicationConfigurationViewModel(ApplicationSceneSwitcherConfig applicationConfiguration, EndpointConfiguration endpointConfiguration)
    {
        _applicationconfig = applicationConfiguration;
        _endpointconfig = endpointConfiguration;
        foreach(var scene in _applicationconfig.Scenes.OrderBy(s=>s.Priority))
        {
            ApplicationSceneSwitcherSceneViewModel sceneViewModel = new ApplicationSceneSwitcherSceneViewModel(scene);
            Scenes.Add(sceneViewModel);
        }
        GetAdapterScenes();
    }
    private async void GetAdapterScenes()
    {
        var adapter = AdapterFactory.CreateInstance(_endpointconfig);
        await adapter.ConnectAsync();
        AdapterScenes = await adapter.GetScenes();
    }   
    public int PollingInterval
    {
        get => _applicationconfig.PollingInterval;
        set
        {
            if(_applicationconfig.PollingInterval != value)
            {
                _applicationconfig.PollingInterval = value;
                OnPropertyChanged(nameof(PollingInterval));
            }
        }
    }
    public ObservableCollection<ApplicationSceneSwitcherSceneViewModel> Scenes
    {
        get => _observableScenesList;
        set
        {
            _observableScenesList = value;
            OnPropertyChanged(nameof(Scenes));
        }
    }
    private ApplicationSceneSwitcherSceneViewModel _editScene;
    public ApplicationSceneSwitcherSceneViewModel EditScene
    {
        get => _editScene;
        set
        {
            if(_editScene != value)
            {
                _editScene = value;
                OnPropertyChanged(nameof(EditScene));
                OnPropertyChanged(nameof(IsEditEnabled));
                //OnPropertyChanged(nameof(Scenes));
            }
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
    public bool IsEditEnabled
    {
        get => EditScene != null;
    }
    public ICommand SaveCommand => new Command(SaveConfig);
    public ICommand AddNewRuleCommand => new Command(AddNewRule);
    public ICommand RemoveRuleCommand => new Command(RemoveRule);
    public ICommand MoveRuleUpCommand => new Command(MoveRuleUp);
    public ICommand MoveRuleDownCommand => new Command(MoveRuleDown);
    private async void SaveConfig()
    {
        var scenesList = new List<ApplicationSceneSwitcherScene>();
        foreach(var sceneViewModel in Scenes)
        {
            var scene = new ApplicationSceneSwitcherScene()
            {
                Scene = sceneViewModel.Scene,
                ProcessName = sceneViewModel.ProcessName,
                Priority = sceneViewModel.Priority,
                NeedsFocus = sceneViewModel.NeedsFocus,
                UseWindowTitleInsteadOfProcessName = sceneViewModel.UseWindowTitleInsteadOfProcessName
            };

            scenesList.Add(scene);
        }

        _applicationconfig.Scenes = scenesList;
        ConfigurationManager configurationManager = new ConfigurationManager();
        configurationManager.UpdateApplicationConfiguration(_applicationconfig);
        EditScene = null;
    }
    private async void AddNewRule()
    {
        EditScene = new ApplicationSceneSwitcherSceneViewModel(new ApplicationSceneSwitcherScene());
        EditScene.Scene = _adapterScenes[0];
        EditScene.Priority = 0;

        foreach(var scene in Scenes)
        {
            scene.Priority++;
        }

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
            Scenes.Move(newIndex, currentIndex);
            SortSceneCollection();
            EditScene = Scenes[newIndex];
        }
    }
    private void SortSceneCollection()
    {
        var sortedSceneCollection = new ObservableCollection<ApplicationSceneSwitcherSceneViewModel>();
        int index = 0;
        foreach(var scene in Scenes)
        {
            scene.Priority = index;
            index++;
            sortedSceneCollection.Add(scene);
        }
        Scenes = sortedSceneCollection;
    }
    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
