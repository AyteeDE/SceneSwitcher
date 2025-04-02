using System.Threading.Tasks;
using AyteeDE.SceneSwitcher.Configuration.Application;
using AyteeDE.SceneSwitcher.Monitoring;
using AyteeDE.StreamAdapter.Core.Communication;
using AyteeDE.StreamAdapter.Core.Configuration;

namespace AyteeDE.SceneSwitcher.Switching;

public class ApplicationSceneSwitcher : SceneSwitcher
{
    private ApplicationSceneSwitcherConfig _applicationSceneSwitcherConfig;
    private ApplicationSceneSwitcherScene _currentScene;
    private SystemMonitoring _systemMonitoring;
    private HardwareMonitoring _hardwareMonitoring;
    public ApplicationSceneSwitcher(EndpointConfiguration endpointConfiguration, ApplicationSceneSwitcherConfig config) : base(endpointConfiguration)
    {
        _systemMonitoring = new SystemMonitoring();
        _hardwareMonitoring = new HardwareMonitoring();
        _applicationSceneSwitcherConfig = config;
        OnSceneChanged += SceneChanged;
    }
    public void StartSwitching()
    {
        base.StartSwitching(TimerTick, 0, _applicationSceneSwitcherConfig.PollingInterval);
    }
    public void SceneChanged(object sender, SceneSwitchingEventArgs e)
    {
        bool sceneInConfig = false;
        foreach(var scene in _applicationSceneSwitcherConfig.Scenes)
        {
            if(e.Scene.Equals(scene.Scene))
            {
                sceneInConfig = true;
            }
        }
        if(sceneInConfig)
        {
            if(_isPaused)
            {
                ResumeSwitching(TimerTick, 0, _applicationSceneSwitcherConfig.PollingInterval);
            }
        }
        else
        {
            if(!_isPaused)
            {
                _currentScene = null;
                PauseSwitching();
            }
        }
    }
    private async void TimerTick(Object stateInfo)
    {
        var matchingScene = await FindMatchingScene();
        if(matchingScene != null && !matchingScene.Equals(_currentScene))
        {
            await Task.Delay(matchingScene.SwitchingDelay);
            await SwitchScene(matchingScene.Scene);
            _currentScene = matchingScene;
        }
    }
    private async Task<ApplicationSceneSwitcherScene> FindMatchingScene()
    {
        foreach(var scene in _applicationSceneSwitcherConfig.Scenes.OrderByDescending(s => s.Priority))
        {
            int gpuLoad = 0;
            if(SystemMonitoring.OS == PlatformID.Win32NT && !String.IsNullOrWhiteSpace(scene.GPUName))
            {
                gpuLoad = await _hardwareMonitoring.GetGPUCoreLoad(scene.GPUName);
            }

            if(gpuLoad < scene.GPULoadLimit) //if GPU Load limit is not exceeded, continue with next scene
            {
                continue;
            }

            if(scene.NeedsFocus && scene.UseWindowTitleInsteadOfProcessName && SystemMonitoring.OS == PlatformID.Win32NT) //check OS, not working on Mac
            {
                if(_systemMonitoring.GetFocussedWindowTitle().Contains(scene.ProcessName.ToLower()))
                {
                    return scene;
                }
            }
            else if(scene.NeedsFocus && SystemMonitoring.OS == PlatformID.Win32NT) //check OS, not working on Mac
            {
                if(_systemMonitoring.GetFocussedWindowProcessName() == scene.ProcessName.ToLower())
                {
                    return scene;
                }
            }
            else
            {
                if(_systemMonitoring.GetProcesses().FirstOrDefault(p => p == scene.ProcessName.ToLower()) != null || String.IsNullOrWhiteSpace(scene.ProcessName)) //empty ProcessName -> Default Scene after no other matching scene was found
                {
                    return scene;
                }
            }
        }
        return null;
    }
}
