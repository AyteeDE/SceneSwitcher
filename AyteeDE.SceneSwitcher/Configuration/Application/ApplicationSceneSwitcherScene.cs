using AyteeDE.StreamAdapter.Core.Entities;

namespace AyteeDE.SceneSwitcher.Configuration.Application;

public class ApplicationSceneSwitcherScene : IEquatable<ApplicationSceneSwitcherScene>
{
    public Scene Scene { get; set; }
    private string _processName;
    public string ProcessName
    {
        get 
        {
            if(_processName == null)
            {
                return "";
            }
            return _processName;
        }
        set => _processName = value;
    }
    public int Priority { get; set; }
    public int SwitchingDelay { get; set; } = 0;
    public bool NeedsFocus { get; set; }
    public bool UseWindowTitleInsteadOfProcessName { get; set; }
    public int GPULoadLimit { get; set; } = 0;
    public string GPUName { get; set; } = string.Empty;
    public bool Equals(ApplicationSceneSwitcherScene? other)
    {
        if(other == null || other.Scene == null) return false;
        return ProcessName.Equals(other.ProcessName) && Scene.Equals(other.Scene) && Priority.Equals(other.Priority);
    }
}
