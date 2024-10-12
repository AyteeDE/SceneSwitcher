using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using AyteeDE.SceneSwitcher.Configuration.Application;
using AyteeDE.StreamAdapter.Core.Communication;
using AyteeDE.StreamAdapter.Core.Configuration;

namespace AyteeDE.SceneSwitcher.Switching;

public class ApplicationSceneSwitcher : SceneSwitcher
{
    private ApplicationSceneSwitcherConfig _applicationSceneSwitcherConfig;
    private ApplicationSceneSwitcherScene _currentScene;
    private static PlatformID _os = Environment.OSVersion.Platform; //Imported DLLs for getting Foreground-windows only work on windows -> OS-Validation
    public ApplicationSceneSwitcher(EndpointConfiguration endpointConfiguration, ApplicationSceneSwitcherConfig config) : base(endpointConfiguration)
    {
        _applicationSceneSwitcherConfig = config;
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
    private async void TimerTick(Object stateInfo)
    {
        var matchingScene = FindMatchingScene();
        if(matchingScene != null && !_currentScene.Equals(matchingScene))
        {
            await Task.Delay(matchingScene.SwitchingDelay);
            await SwitchScene(matchingScene.Scene);
            _currentScene = matchingScene;
        }
    }
    private ApplicationSceneSwitcherScene FindMatchingScene()
    {
        foreach(var scene in _applicationSceneSwitcherConfig.Scenes.OrderBy(s => s.Priority))
        {
            if(scene.NeedsFocus && scene.UseWindowTitleInsteadOfProcessName && _os == PlatformID.Win32NT)
            {
                if(GetFocussedWindowTitle().Contains(scene.ProcessName))
                {
                    return scene;
                }
            }
            else if(scene.NeedsFocus && _os == PlatformID.Win32NT)
            {
                if(GetFocussedWindowProcessName() == scene.ProcessName)
                {
                    return scene;
                }
            }
            else
            {
                if(GetProcesses().FirstOrDefault(p => p == scene.ProcessName) != null || String.IsNullOrWhiteSpace(scene.ProcessName)) //empty ProcessName -> Default Scene after no other matching scene was found
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
            processList.Add(process.ProcessName);
        }
        return processList;
    }
    private string GetFocussedWindowProcessName()
    {
        IntPtr handle = GetForegroundWindow();
        GetWindowThreadProcessId(handle, out uint processId);
        var process = Process.GetProcessById((int)processId);
        return process.ProcessName;
    }
    private string GetFocussedWindowTitle()
    {
        IntPtr handle = GetForegroundWindow();
        const int nChars = 256;
        StringBuilder stringBuilder = new StringBuilder(nChars);
        if(GetWindowText(handle, stringBuilder, nChars) > 0)
        {
            return stringBuilder.ToString();
        }
        return String.Empty;
    }
    public event EventHandler<SceneSwitchingEventArgs> OnSceneSwitched;
}
