using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuNetworked : MonoBehaviour
{
    public InputField ServerAddress;
    public InputField HostAddress;

    public void StartServer()
    {
        string address = HostAddress.text;

        // START SERVER
        BoltLauncher.StartServer(UdpKit.UdpEndPoint.Parse(address));
        BoltNetwork.LoadScene("EndpointNetworked");
    }

    public void JoinServer()
    {
        string address = ServerAddress.text;

        // START CLIENT
        BoltLauncher.StartClient();
        BoltNetwork.Connect(UdpKit.UdpEndPoint.Parse(address));
    }
}