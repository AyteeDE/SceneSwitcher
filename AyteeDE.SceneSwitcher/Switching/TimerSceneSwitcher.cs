using AyteeDE.SceneSwitcher.Configuration.Timer;
using AyteeDE.StreamAdapter.Core.Communication;
using AyteeDE.StreamAdapter.Core.Configuration;

namespace AyteeDE.SceneSwitcher.Switching;

public class TimerSceneSwitcher
{
    private TimerSceneSwitcherConfig _timerSceneSwitcherConfig;
    private Timer _timer;
    private IStreamAdapter _adapter;
    public TimerSceneSwitcher(EndpointConfiguration endpointConfiguration, TimerSceneSwitcherConfig timerSceneSwitcherConfig)
    {
        _timerSceneSwitcherConfig = timerSceneSwitcherConfig;
        _adapter = AdapterFactory.CreateInstance(endpointConfiguration);
    }
    public bool IsTimerRunning
    {
        get => _timer != null;
    }
    public async void StartSwitching()
    {
        _currentScene = await TryGetCurrentSceneOnStart();

        AutoResetEvent autoReset = new AutoResetEvent(false);
        _timer = new Timer(SwitchScene, autoReset, _timerSceneSwitcherConfig.Interval, _timerSceneSwitcherConfig.Interval);
    }
    public void StopSwitching()
    {
        _timer.Dispose();
    }
    private async void SwitchScene(Object stateInfo)
    {
        var next = GetNextScene();
        if(next.DurationOverride != 0)
        {
            _timer.Change(next.DurationOverride, next.DurationOverride);
        }
        else
        {
            _timer.Change(_timerSceneSwitcherConfig.Interval, _timerSceneSwitcherConfig.Interval);
        }
        await _adapter.SetCurrentProgramScene(next.Scene);
        _currentScene = next;
        SubscribedEventHandler.InvokeSubscribedEvent(OnSceneSwitched, this, new SceneSwitchingEventArgs(_currentScene.Scene));
    }
    private TimerSceneSwitcherScene _currentScene;
    private TimerSceneSwitcherScene GetNextScene()
    {
        if(_timerSceneSwitcherConfig.IsRandom)
        {
            return GetNextRandomScene();
        }
        else
        {
            return GetNextSortedScene();
        }
    }
    private async Task<TimerSceneSwitcherScene> TryGetCurrentSceneOnStart()
    {
        var scene = await _adapter.GetCurrentProgramScene();
        var match = _timerSceneSwitcherConfig.Scenes.FirstOrDefault(s => s.Scene.Equals(scene));
        return match;
    }
    private Random _random = new Random();
    private TimerSceneSwitcherScene GetNextRandomScene(int currentIterations = 0)
    {
        int rngPosition = _random.Next(_timerSceneSwitcherConfig.Scenes.Count);
        var scene = _timerSceneSwitcherConfig.Scenes[rngPosition];

        //Cancel the recursion if random scene is the current scenes 5 times in a row to prevent stackoverflow
        if(scene.Equals(_currentScene) || currentIterations == 5)
        {
            return scene;
        }
        else
        {
            return GetNextRandomScene(currentIterations + 1);
        }
    }
    private TimerSceneSwitcherScene GetNextSortedScene()
    {
        int currentPosition = _currentScene == null ? -1 : _currentScene.Position;
        var next = _timerSceneSwitcherConfig.Scenes.OrderBy(s => s.Position).FirstOrDefault(s => s.Position > currentPosition);
        if(next == null)
        {
            return _timerSceneSwitcherConfig.Scenes.OrderBy(s => s.Position).FirstOrDefault();
        }
        else
        {
            return next;
        }
    }
    public event EventHandler<SceneSwitchingEventArgs> OnSceneSwitched;
}
