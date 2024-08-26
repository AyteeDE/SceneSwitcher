using System;
using AyteeDE.StreamAdapter.Core.Configuration;

namespace AyteeDE.SceneSwitcher.Configuration;

public class SceneSwitcherConfig
{
    public EndpointConfiguration EndpointConfiguration{ get; set; }
    public ApplicationSceneSwitcherConfig ApplicationSceneSwitcherConfig{ get; set; }
    public TimerSceneSwitcherConfig TimerSceneSwitcherConfig{ get; set; }
}
