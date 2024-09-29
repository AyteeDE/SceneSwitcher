using System;
using System.Text.Json;
using AyteeDE.SceneSwitcher.Configuration;
using AyteeDE.SceneSwitcher.Configuration.Application;
using AyteeDE.SceneSwitcher.Configuration.Timer;
using AyteeDE.StreamAdapter.Core.Communication;
using AyteeDE.StreamAdapter.Core.Configuration;

namespace AyteeDE.SceneSwitcher.MAUIApp;

public class ConfigurationManager
{
    private static SceneSwitcherConfig _configuration;
    public SceneSwitcherConfig Configuration 
    {
        get 
        {
            TryLoadConfig();
            return _configuration;
        }
    }
    public void TryLoadConfig()
	{
        var configJson = Preferences.Default.Get("SceneSwitcherConfigJson", "");
		try
		{
			_configuration = JsonSerializer.Deserialize<SceneSwitcherConfig>(configJson);
            //var x = AdapterFactory.CreateInstance(_configuration.EndpointConfiguration);
		}
		catch (Exception ex)
		{
			_configuration = new SceneSwitcherConfig();
            _configuration.EndpointConfiguration.ConnectionType = typeof(AyteeDE.StreamAdapter.OBSStudioWebsocket5.Communication.OBSStudioWebsocket5Adapter);
		}
	}
    private void TrySaveConfig()
    {
        var json = JsonSerializer.Serialize(_configuration);
        Preferences.Default.Set("SceneSwitcherConfigJson", json);
    }
    public void UpdateEndpointConfiguration(EndpointConfiguration endpointConfiguration)
    {
        if(endpointConfiguration != null)
        {
            _configuration.EndpointConfiguration = endpointConfiguration;
        }
        TrySaveConfig();
    }
    public void UpdateApplicationConfiguration(ApplicationSceneSwitcherConfig applicationConfiguration)
    {
        if(applicationConfiguration != null)
        {
            _configuration.ApplicationSceneSwitcherConfig = applicationConfiguration;
        }
        TrySaveConfig();
    }
    public void UpdateTimerConfiguration(TimerSceneSwitcherConfig timerConfiguration)
    {
        if(timerConfiguration != null)
        {
            _configuration.TimerSceneSwitcherConfig = timerConfiguration;
        }
        TrySaveConfig();
    }
}
