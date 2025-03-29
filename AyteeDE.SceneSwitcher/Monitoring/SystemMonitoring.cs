using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AyteeDE.SceneSwitcher.Monitoring;

public class SystemMonitoring
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int length);
    public readonly static PlatformID OS = Environment.OSVersion.Platform; 
    public List<string> GetProcesses()
    {
        List<string> processList = new List<string>();
        var processes = Process.GetProcesses();
        foreach(var process in processes)
        {
            processList.Add(process.ProcessName.ToLower());
        }
        return processList;
    }
    public string GetFocussedWindowProcessName()
    {
        IntPtr handle = GetForegroundWindow();
        GetWindowThreadProcessId(handle, out uint processId);
        var process = Process.GetProcessById((int)processId);
        return process.ProcessName.ToLower();
    }
    public string GetFocussedWindowTitle()
    {
        IntPtr handle = GetForegroundWindow();
        const int nChars = 256;
        StringBuilder stringBuilder = new StringBuilder(nChars);
        if(GetWindowText(handle, stringBuilder, nChars) > 0)
        {
            return stringBuilder.ToString().ToLower();
        }
        return String.Empty;
    }
}
