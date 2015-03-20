using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net;

public class NetworkedMenu : MonoBehaviour {

    public Canvas MainMenu;
    public Canvas MultiLobby;
    public Canvas JoinLobby;
    public InputField HostUsername;
    public InputField ClientUsername;
    public Button ServerButton;

    public List<UdpKit.UdpSession> ServerList;

    public string HostServerAddress;

    public ServerList ServerListing;

    public GameObject ServerButtonPrefab;

    public bool LegacyGUI;

    public InputField DirectIP;

    // Use this for initialization
	void Start () 
    {
        ServerList = new List<UdpKit.UdpSession>(); // Fuck
	}
	
	// Update is called once per frame
	void Update () 
    {

    }

    public void GoLobbyToMenu() // Main Menu
    {
        StartCoroutine(ShutdownBolt());
        MainMenu.gameObject.SetActive(true);
        MultiLobby.gameObject.SetActive(false);
        JoinLobby.gameObject.SetActive(false);
        ServerList.Clear();
    }

    public void GoToMultiplayerLobby()  // Pick to host or join lobby
    {
        MainMenu.gameObject.SetActive(false);
        MultiLobby.gameObject.SetActive(true);
        HostServerAddress = Dns.GetHostName();
        IPHostEntry ip = Dns.GetHostEntry(HostServerAddress);
        HostServerAddress = ip.AddressList[0].ToString() + ":27000";
        //BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse(HostServerAddress)); // Start server
    }

    public void GoToClientLobby()   // Start client lobby
    {
        if (!BoltNetwork.isRunning)
            BoltLauncher.StartClient();

        MainMenu.gameObject.SetActive(false);
        MultiLobby.gameObject.SetActive(false);
        JoinLobby.gameObject.SetActive(true);
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
        if (!BoltNetwork.isRunning)
            BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse(HostServerAddress)); // Start server

        BoltNetwork.RegisterTokenClass<HostInfo>(); // packets!
        BoltNetwork.RegisterTokenClass<PlayerName>();

        var hostToken = new HostInfo(); // Setting packet info
        hostToken.maxConnections = 4;

        var serverName = new PlayerName();
        serverName.Username = HostUsername.text;

        if (BoltNetwork.isServer)
            BoltNetwork.SetHostInfo(serverName.Username, hostToken);    // Server name, etc

        BoltNetwork.LoadScene("TestNetworkedLevel");
    }

    public void JoinServer(UdpKit.UdpSession s)
    {
        //StartCoroutine(HostToClient(s));

        BoltNetwork.RegisterTokenClass<HostInfo>(); // packets!
        BoltNetwork.RegisterTokenClass<PlayerName>();

        var clientToken = new PlayerName();
        clientToken.Username = ClientUsername.text;

        BoltNetwork.Connect(s, clientToken); // Connect to server, send along players name
    }

    IEnumerator HostToClient(UdpKit.UdpSession s)  // Shut down server
    {
        if (BoltNetwork.isRunning)
        {
            BoltLauncher.Shutdown();

            yield return new UnityEngine.WaitForSeconds(3);
        }

        // START CLIENT
        BoltLauncher.StartClient();

        BoltNetwork.RegisterTokenClass<HostInfo>(); // packets!
        BoltNetwork.RegisterTokenClass<PlayerName>();

        var clientToken = new PlayerName();
        clientToken.Username = ClientUsername.text;

        BoltNetwork.Connect(s, clientToken); // Connect to server, send along players name
    }

    IEnumerator RestartServer()  // Shut down server
    {
        if (BoltNetwork.isRunning)
        {
            BoltLauncher.Shutdown();

            yield return new UnityEngine.WaitForSeconds(3);
        }

        BoltLauncher.StartServer();
    }

    IEnumerator ShutdownBolt()  // Shut down server
    {
        if (BoltNetwork.isRunning)
        {
            BoltLauncher.Shutdown();

            yield return new UnityEngine.WaitForSeconds(3);
        }
    }

    void OnGUI()    // This sucks, do something better
    {
        // exit if we arne't trying to use Legacy OnGUI bs
        if (!LegacyGUI)
            return;

        foreach (var server in ServerList)
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 150, 50), server.HostName.ToString()))
            {
                JoinServer(server);
            }
        }
    }

    public void HostDirectIP()  // Do a server without Zeus
    {
        BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse(HostServerAddress)); // Start server

        BoltNetwork.LoadScene("TestNetworkedLevel");
    }

    public void JoinDirectIP()
    {
        string address = DirectIP.text;

        // START CLIENT
        BoltLauncher.StartClient();
        BoltNetwork.Connect(UdpKit.UdpEndPoint.Parse(address));
    }
}
