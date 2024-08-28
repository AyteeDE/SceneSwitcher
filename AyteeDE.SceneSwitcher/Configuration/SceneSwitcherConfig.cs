using AyteeDE.SceneSwitcher.Configuration.Application;
using AyteeDE.SceneSwitcher.Configuration.Timer;
using AyteeDE.StreamAdapter.Core.Configuration;

namespace AyteeDE.SceneSwitcher.Configuration;

public class SceneSwitcherConfig
{
    public EndpointConfiguration EndpointConfiguration{ get; set; } = new EndpointConfiguration();
    public ApplicationSceneSwitcherConfig ApplicationSceneSwitcherConfig{ get; set; } = new ApplicationSceneSwitcherConfig();
    public TimerSceneSwitcherConfig TimerSceneSwitcherConfig{ get; set; } = new TimerSceneSwitcherConfig();
}
