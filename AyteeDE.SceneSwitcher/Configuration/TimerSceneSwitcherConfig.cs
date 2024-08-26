using System;

namespace AyteeDE.SceneSwitcher.Configuration;

public class TimerSceneSwitcherConfig
{
    public int Interval { get; set; }
    public bool IsRandom { get; set; } = false;
    public List<TimerSceneSwitcherScenesConfig> Scenes { get; set; } = new List<TimerSceneSwitcherScenesConfig>();
}
