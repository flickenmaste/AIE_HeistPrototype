using UnityEngine;
using System.Collections;

public class HostInfo : Bolt.IProtocolToken
{
    public int maxConnections;

    public void Write(UdpKit.UdpPacket packet)
    {
        packet.WriteInt(maxConnections);
    }

    public void Read(UdpKit.UdpPacket packet)
    {
        maxConnections = packet.ReadInt();
    }
}
