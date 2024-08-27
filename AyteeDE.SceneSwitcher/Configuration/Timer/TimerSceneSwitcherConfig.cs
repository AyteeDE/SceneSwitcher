namespace AyteeDE.SceneSwitcher.Configuration.Timer;

public class TimerSceneSwitcherConfig
{
    public int Interval { get; set; }
    public bool IsRandom { get; set; } = false;
    public List<TimerSceneSwitcherScene> Scenes { get; set; } = new List<TimerSceneSwitcherScene>();
}
