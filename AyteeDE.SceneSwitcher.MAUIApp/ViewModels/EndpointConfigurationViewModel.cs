using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AyteeDE.StreamAdapter.Core.Configuration;

namespace AyteeDE.SceneSwitcher.MAUIApp.ViewModels;

public class EndpointConfigurationViewModel : INotifyPropertyChanged
{
    private EndpointConfiguration _config;
    public EndpointConfigurationViewModel(EndpointConfiguration config)
    {
        _config = config;
    }
    public List<string> ConnectionTypeCollection
    {
        get
        {
            var list = new List<string>();
            list.Add("OBS Studio Websocket 5");
            list.Add("Streamlabs Websocket");
            return list;
        }
    }
    public string ConnectionType
    {
        get 
        {
            var configName = String.IsNullOrWhiteSpace(_config.ConnectionTypeName) ? typeof(AyteeDE.StreamAdapter.OBSStudioWebsocket5.Communication.OBSStudioWebsocket5Adapter).FullName : _config.ConnectionTypeName;

            switch(configName)
            {
                case "AyteeDE.StreamAdapter.OBSStudioWebsocket5.Communication.OBSStudioWebsocket5Adapter":
                    return "OBS Studio Websocket 5";
                case "AyteeDE.StreamAdapter.StreamlabsWebsocket.Communication.StreamlabsWebsocketAdapter":
                    return "Streamlabs Websocket";
                default:
                    return null;
            }
        }
        set
        {
            switch(value)
            {
                case "OBS Studio Websocket 5":
                    _config.ConnectionType = typeof(StreamAdapter.OBSStudioWebsocket5.Communication.OBSStudioWebsocket5Adapter);
                    break;
                case "Streamlabs Websocket":
                    _config.ConnectionType = typeof(StreamAdapter.StreamlabsWebsocket.Communication.StreamlabsWebsocketAdapter);
                    break;
            }
        }
    }
    public string Host
    {
        get => _config.Host;
        set
        {
            if(_config.Host != value)
            {
                _config.Host = value;
                OnPropertyChanged(nameof(Host));
            }
        }
    }
    public int Port
    {
        get => _config.Port == null ? 0 : _config.Port.Value;
        set
        {
            if(_config.Port != value)
            {
                _config.Port = value;
                OnPropertyChanged(nameof(Port));
            }
        }
    }
    public string Token
    {
        get => _config.Token;
        set
        {
            if(_config.Token != value)
            {
                _config.Token = value;
                OnPropertyChanged(nameof(Token));
            }
        }
    }
    public bool AuthenticationEnabled
    {
        get => _config.AuthenticationEnabled;
        set
        {
            if(_config.AuthenticationEnabled != value)
            {
                _config.AuthenticationEnabled = value;
                OnPropertyChanged(nameof(AuthenticationEnabled));
                if(!value)
                {
                    Token = string.Empty;
                }
            }
        }
    }
    public ICommand SaveCommand => new Command(SaveConfig);
    public async void SaveConfig()
    {
        ConfigurationManager configurationManager = new ConfigurationManager();
        configurationManager.UpdateEndpointConfiguration(_config);

        await Application.Current.MainPage.DisplayAlert("Saved", "Connection settings saved!", "OK");
    }
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
