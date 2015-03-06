using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkedMenu : MonoBehaviour {

    public Canvas MainMenu;
    public Canvas MultiLobby;
    public InputField Username;
    public Button ServerButton;

    public List<UdpKit.UdpSession> ServerList;
    
    // Use this for initialization
	void Start () 
    {
        ServerList = new List<UdpKit.UdpSession>(); // Fuck
	}
	
	// Update is called once per frame
	void Update () 
    {

    }

    public void GoLobbyToMenu()
    {
        MainMenu.gameObject.SetActive(true);
        MultiLobby.gameObject.SetActive(false);
    }

    public void GoToMultiplayerLobby()
    {
        MainMenu.gameObject.SetActive(false);
        MultiLobby.gameObject.SetActive(true);
        BoltLauncher.StartServer(new UdpKit.UdpEndPoint(UdpKit.UdpIPv4Address.Any, 27000)); // Start server
    }

    public void PingMasterServer()
    {
        Bolt.Zeus.RequestSessionList(); // Get server list
        foreach (var server in BoltNetwork.SessionList)
        {
            ServerList.Add(server.Value);
        }
    }

    public void HostServer()
    {
        if(!BoltNetwork.isServer)
            BoltLauncher.StartServer(new UdpKit.UdpEndPoint(UdpKit.UdpIPv4Address.Any, 27000));

        BoltNetwork.RegisterTokenClass<HostInfo>(); // packets!
        BoltNetwork.RegisterTokenClass<PlayerName>();

        var hostToken = new HostInfo(); // Setting packet info
        hostToken.maxConnections = 4;

        var serverName = new PlayerName();
        serverName.Username = Username.text;

        if (BoltNetwork.isServer)
            BoltNetwork.SetHostInfo(serverName.Username, hostToken);    // Server name, etc

        BoltNetwork.LoadScene("HostLobby");
    }

    public void JoinServer(UdpKit.UdpSession s)
    {
        BoltNetwork.RegisterTokenClass<HostInfo>(); // packets!
        BoltNetwork.RegisterTokenClass<PlayerName>();

        var clientToken = new PlayerName();
        clientToken.Username = Username.text;

        // START CLIENT
        BoltLauncher.StartClient();
        BoltNetwork.Connect(s, clientToken); // Connect to server, send along players name
    }

    void OnGUI()    // This sucks, do something better
    {
        foreach (var server in ServerList)
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 150, 50), server.HostName.ToString()))
            {
                JoinServer(server);
            }
        }
    }
}
