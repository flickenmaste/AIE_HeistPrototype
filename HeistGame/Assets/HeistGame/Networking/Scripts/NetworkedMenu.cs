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

    public bool pinged;
    
    // Use this for initialization
	void Start () 
    {

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
        BoltLauncher.StartServer(new UdpKit.UdpEndPoint(UdpKit.UdpIPv4Address.Any, 27000));
    }

    public void PingMasterServer()
    {
        Bolt.Zeus.RequestSessionList();
        foreach (var server in BoltNetwork.SessionList)
        {
            ServerList.Add(server.Value);
        }

    }

    public void HostServer()
    {
        if(!BoltNetwork.isServer)
            BoltLauncher.StartServer(new UdpKit.UdpEndPoint(UdpKit.UdpIPv4Address.Any, 27000));

        BoltNetwork.RegisterTokenClass<HostInfo>();
        BoltNetwork.RegisterTokenClass<PlayerName>();

        var hostToken = new HostInfo();
        hostToken.maxConnections = 4;

        var serverName = new PlayerName();
        serverName.Username = Username.text;

        if (BoltNetwork.isServer)
            BoltNetwork.SetHostInfo(serverName.Username, hostToken);

        BoltNetwork.LoadScene("HostLobby");
    }

    public void JoinServer(UdpKit.UdpSession s)
    {
        // START CLIENT
        BoltLauncher.StartClient();
        BoltNetwork.Connect(s);
    }

    void OnGUI()
    {
        if (pinged)
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
}
