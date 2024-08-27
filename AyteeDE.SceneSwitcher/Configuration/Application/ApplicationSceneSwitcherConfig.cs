namespace AyteeDE.SceneSwitcher.Configuration.Application;

public class ApplicationSceneSwitcherConfig
{
    public int PollingInterval { get; set; } = 500;
    public List<ApplicationSceneSwitcherScene> Scenes { get; set; } = new List<ApplicationSceneSwitcherScene>();
}
