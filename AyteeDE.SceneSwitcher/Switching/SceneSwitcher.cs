using System;
using AyteeDE.StreamAdapter.Core.Communication;
using AyteeDE.StreamAdapter.Core.Configuration;
using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.Switching;

public class SceneSwitcher
{
    protected IStreamAdapter _adapter;
    protected Timer _timer;
    protected SceneSwitcher(EndpointConfiguration endpointConfiguration)
    {
        _adapter = AdapterFactory.CreateInstance(endpointConfiguration);
    }
    protected bool IsTimerRunning
    {
        get => _timer != null;
    }
    protected void StartSwitching(TimerCallback timerCallback, int dueTime, int period)
    {
        AutoResetEvent autoReset = new AutoResetEvent(false);
        _timer = new Timer(timerCallback, autoReset, dueTime, period);
    }
    public void StopSwitching()
    {
        _timer.Dispose();
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
}
