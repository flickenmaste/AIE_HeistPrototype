using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerEntry
{
    public ServerEntry(UdpKit.UdpSession serverData)
    {
        ServerDesc = serverData;
    }

    public UdpKit.UdpSession ServerDesc;

	public void printServerDesc()
    {
        Debug.Log("Dedi: " + ServerDesc.IsDedicatedServer);
        Debug.Log("WAN End Point: " + ServerDesc.WanEndPoint);
        Debug.Log("Player Count: " + ServerDesc.ConnectionsCurrent + "/" + ServerDesc.ConnectionsMax);
    }


}
