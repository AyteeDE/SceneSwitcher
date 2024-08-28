using System;
using System.Text.Json;
using AyteeDE.SceneSwitcher.Configuration;
using AyteeDE.StreamAdapter.Core.Configuration;

namespace AyteeDE.SceneSwitcher.MAUIApp;

public class ConfigurationManager
{
    private const string CONFIG_FILE_NAME = "config.json";
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
		}
		catch
		{
			_configuration = new SceneSwitcherConfig();
            _configuration.EndpointConfiguration.ConnectionType = typeof(AyteeDE.StreamAdapter.OBSStudioWebsocket5.Communication.OBSStudioWebsocket5Adapter);
		}
	}
    private void TrySaveConfig()
    {
        var json = JsonSerializer.Serialize(Configuration);
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

}
