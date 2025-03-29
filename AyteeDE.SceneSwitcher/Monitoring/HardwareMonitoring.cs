using System;
using System.ComponentModel;
using LibreHardwareMonitor.Hardware;

namespace AyteeDE.SceneSwitcher.Monitoring;

public class HardwareMonitoring : IDisposable
{
    private Computer _computer = new Computer();
    public HardwareMonitoring()
    {
        _computer.Open();
    }
    public void SetGPUMonitoring(bool enabled)
    {
        _computer.IsGpuEnabled = enabled;
    }
    public void SetCPUMonitoring(bool enabled)
    {
        _computer.IsCpuEnabled = enabled;
    }
    public void SetNetworkMonitoring(bool enabled)
    {
        _computer.IsNetworkEnabled = enabled;
    }
    public void Dispose()
    {
        _computer.Close();
    }
    private IHardware ValidateGPUMonitoring(string gpuName)
    {
        if(!_computer.IsGpuEnabled)
        {
            SetGPUMonitoring(true);
        }
        if(String.IsNullOrWhiteSpace(gpuName))
        {
            return null;
        }
        return _computer.Hardware.FirstOrDefault(h => h.Name == gpuName);
    }
    public List<string> GetAllGPUs()
    {
        SetGPUMonitoring(true);
        var gpuList = new List<string>();
        gpuList.AddRange(_computer.Hardware.Where(h => 
                            h.HardwareType == HardwareType.GpuAmd 
                            || h.HardwareType == HardwareType.GpuNvidia
                            || h.HardwareType == HardwareType.GpuIntel).Select(h => h.Name).ToList());
        return gpuList;
    }
    public async Task<int> GetGPUCoreLoad(string gpuName)
    {
        var gpu = ValidateGPUMonitoring(gpuName);
        if(gpu == null)
        {
            throw new Exception("GPU monitoring is not enabled or GPU name is invalid.");
        }

        gpu.Update();

        var loadSensor = gpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name.Contains("GPU Core"));

        if(loadSensor == null || loadSensor.Value == null)
        {
            throw new Exception("GPU Core load sensor not found.");
        }

        return (int)loadSensor.Value;
    }
}
