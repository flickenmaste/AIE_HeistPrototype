using UnityEngine;
using System.Collections;

public class PlayerName : Bolt.IProtocolToken
{

    public string Username;

    public void Write(UdpKit.UdpPacket packet)
    {
        packet.WriteString(Username);
    }

    public void Read(UdpKit.UdpPacket packet)
    {
        Username = packet.ReadString();
    }
}
