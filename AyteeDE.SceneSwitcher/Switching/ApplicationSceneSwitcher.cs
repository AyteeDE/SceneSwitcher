using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using AyteeDE.SceneSwitcher.Configuration.Application;
using AyteeDE.StreamAdapter.Core.Communication;
using AyteeDE.StreamAdapter.Core.Configuration;
using LibreHardwareMonitor.Hardware;

namespace AyteeDE.SceneSwitcher.Switching;

public class ApplicationSceneSwitcher : SceneSwitcher
{
    private ApplicationSceneSwitcherConfig _applicationSceneSwitcherConfig;
    private ApplicationSceneSwitcherScene _currentScene;
    private static PlatformID _os = Environment.OSVersion.Platform; //Imported DLLs for getting Foreground-windows only work on windows -> OS-Validation
    public ApplicationSceneSwitcher(EndpointConfiguration endpointConfiguration, ApplicationSceneSwitcherConfig config) : base(endpointConfiguration)
    {
        _applicationSceneSwitcherConfig = config;
        OnSceneChanged += SceneChanged;
    }
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int length);
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
        var matchingScene = FindMatchingScene();
        if(matchingScene != null && !matchingScene.Equals(_currentScene))
        {
            await Task.Delay(matchingScene.SwitchingDelay);
            await SwitchScene(matchingScene.Scene);
            _currentScene = matchingScene;
        }
    }
    private ApplicationSceneSwitcherScene FindMatchingScene()
    {
        foreach(var scene in _applicationSceneSwitcherConfig.Scenes.OrderByDescending(s => s.Priority))
        {
            if(scene.NeedsFocus && scene.UseWindowTitleInsteadOfProcessName && _os == PlatformID.Win32NT)
            {
                if(GetFocussedWindowTitle().Contains(scene.ProcessName.ToLower()))
                {
                    return scene;
                }
            }
            else if(scene.NeedsFocus && _os == PlatformID.Win32NT)
            {
                if(GetFocussedWindowProcessName() == scene.ProcessName.ToLower())
                {
                    return scene;
                }
            }
            else
            {
                if(GetProcesses().FirstOrDefault(p => p == scene.ProcessName.ToLower()) != null || String.IsNullOrWhiteSpace(scene.ProcessName)) //empty ProcessName -> Default Scene after no other matching scene was found
                {
                    return scene;
                }
            }
        }
        return null;
    }
    private List<string> GetProcesses()
    {
        List<string> processList = new List<string>();
        var processes = Process.GetProcesses();
        foreach(var process in processes)
        {
            processList.Add(process.ProcessName.ToLower());
        }
        return processList;
    }
    private string GetFocussedWindowProcessName()
    {
        IntPtr handle = GetForegroundWindow();
        GetWindowThreadProcessId(handle, out uint processId);
        var process = Process.GetProcessById((int)processId);
        return process.ProcessName.ToLower();
    }
    private string GetFocussedWindowTitle()
    {
        IntPtr handle = GetForegroundWindow();
        const int nChars = 256;
        StringBuilder stringBuilder = new StringBuilder(nChars);
        if(GetWindowText(handle, stringBuilder, nChars) > 0)
        {
            return stringBuilder.ToString().ToLower();
        }
        return String.Empty;
    }
    private int GetGPULoad()
    {
        int loadValue = 0;

        Computer computer= new Computer()
        {
            IsGpuEnabled = true
        };

        computer.Open();

        foreach(var hardware in computer.Hardware)
        {
            if(hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd || hardware.HardwareType == HardwareType.GpuIntel)
            {
                hardware.Update();
                foreach(var sensor in hardware.Sensors)
                {
                    if(sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Core"))
                    {
                        if(sensor.Value != null)
                        {
                            loadValue = (int)sensor.Value;
                            break;
                        }
                    }
                }
                break;
            }
        }

        return loadValue;
    }
}
