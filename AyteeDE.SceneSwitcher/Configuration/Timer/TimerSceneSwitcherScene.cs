using System;
using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.Configuration.Timer;

public class TimerSceneSwitcherScene
{
    public Scene Scene { get; set; }
    public int Position { get; set; } = 0;
    public int DurationOverride { get; set; } = 0;
    public override string ToString()
    {
        return Scene.Name;
    }
}
