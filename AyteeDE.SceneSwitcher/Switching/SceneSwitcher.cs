using System;
using AyteeDE.StreamAdapter.Core.Communication;
using AyteeDE.StreamAdapter.Core.Configuration;
using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.Switching;

public class SceneSwitcher
{
    protected IStreamAdapter _adapter;
    protected Timer _timer;
    protected bool _isPaused;
    protected SceneSwitcher(EndpointConfiguration endpointConfiguration)
    {
        _adapter = AdapterFactory.CreateInstance(endpointConfiguration);
        _adapter.OnCurrentProgramSceneChanged += OnCurrentProgramSceneChanged;
    }

    private void OnCurrentProgramSceneChanged(object? sender, Scene e)
    {
        SubscribedEventHandler.InvokeSubscribedEvent(OnSceneChanged, this, new SceneSwitchingEventArgs(e));
    }

    protected bool IsTimerRunning
    {
        get => _timer != null;
    }
    protected void StartSwitching(TimerCallback timerCallback, int dueTime, int period)
    {
        AutoResetEvent autoReset = new AutoResetEvent(false);
        _timer = new Timer(timerCallback, autoReset, dueTime, period);
        SubscribedEventHandler.InvokeSubscribedEvent(OnSwitchingStarted, this);
    }
    public void StopSwitching()
    {
        _timer.Dispose();
        SubscribedEventHandler.InvokeSubscribedEvent(OnSwitchingStopped, this);
        _isPaused = false;
    }
    public void PauseSwitching()
    {
        StopSwitching();
        _isPaused = true;
        SubscribedEventHandler.InvokeSubscribedEvent(OnSwitchingPaused, this);
    }
    public void ResumeSwitching(TimerCallback timerCallback, int dueTime, int period)
    {
        StartSwitching(timerCallback, dueTime, period);
        _isPaused = false;
        SubscribedEventHandler.InvokeSubscribedEvent(OnSwitchingResumed, this);
    }
    protected async Task<bool> SwitchScene(Scene scene)
    {
        if(await _adapter.SetCurrentProgramScene(scene))
        {
            SubscribedEventHandler.InvokeSubscribedEvent(OnSceneSwitched, this, new SceneSwitchingEventArgs(scene));
            return true;
        }
        return false;
    }
    public event EventHandler<SceneSwitchingEventArgs> OnSceneSwitched;
    public event EventHandler<SceneSwitchingEventArgs> OnSceneChanged;
    public event EventHandler OnSwitchingStarted;
    public event EventHandler OnSwitchingStopped;
    public event EventHandler OnSwitchingPaused;
    public event EventHandler OnSwitchingResumed;
}
