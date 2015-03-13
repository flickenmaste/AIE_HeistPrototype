using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerEntryView : MonoBehaviour
{
    public NetworkedMenu masterMenu;

    ServerEntry serverData;
    public ServerEntry serverEntryInformation
    {
        get
        {
            return serverData;
        }

        set
        {
            serverData = value;

            ServerLabel.text = serverData.ServerDesc.HostName;
            PlayerCount.text = serverData.ServerDesc.ConnectionsCurrent + " / " + serverData.ServerDesc.ConnectionsMax;
            DedicatedLabel.text = serverData.ServerDesc.IsDedicatedServer ? "Y" : "N";
        }
    }
    
    public void ConnectToServer()
    {
        masterMenu.JoinServer(serverData.ServerDesc);
    }

    public Text ServerLabel;
    public Text PlayerCount;
    public Text DedicatedLabel;
}
