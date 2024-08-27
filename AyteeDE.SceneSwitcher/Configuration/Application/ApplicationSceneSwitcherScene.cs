using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.Configuration.Application;

public class ApplicationSceneSwitcherScene : IEquatable<ApplicationSceneSwitcherScene>
{
    public Scene Scene { get; set; }
    public string ProcessName { get; set; }
    public int Priority { get; set; }
    public int SwitchingDelay { get; set; } = 0;
    public bool NeedsFocus { get; set; }
    public bool UseWindowTitleInsteadOfProcessName { get; set; }

    public bool Equals(ApplicationSceneSwitcherScene? other)
    {
        if(other == null)
        {
            return false;
        }
        return Scene.Equals(other.Scene) && ProcessName.Equals(other.ProcessName) && Priority.Equals(other.Priority);
    }
}
