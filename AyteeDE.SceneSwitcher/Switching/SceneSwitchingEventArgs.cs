using System;
using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.Switching;

public class SceneSwitchingEventArgs
{
    public Scene Scene { get; set; }
    public SceneSwitchingEventArgs(Scene scene)
    {
        Scene = scene;
    }
}
