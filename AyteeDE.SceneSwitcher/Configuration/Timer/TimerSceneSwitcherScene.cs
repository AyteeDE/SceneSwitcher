using System;
using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.Configuration.Timer;

public class TimerSceneSwitcherScene : IEquatable<TimerSceneSwitcherScene>
{
    public Scene Scene { get; set; }
    public int Position { get; set; } = 0;
    public int DurationOverride { get; set; } = 0;

    public bool Equals(TimerSceneSwitcherScene? other)
    {
        if(other == null || other.Scene == null) return false;
        return Scene.Equals(other.Scene) && Position == other.Position;
    }

    public override string ToString()
    {
        return Scene.Name;
    }
}
