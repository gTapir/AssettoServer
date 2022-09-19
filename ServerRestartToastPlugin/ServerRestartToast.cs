using AssettoServer.Network.Packets;
using AssettoServer.Network.Packets.Outgoing;

namespace ServerRestartToastPlugin;

public class ServerRestartToast : IOutgoingNetworkPacket
{
    public int TimeUntilRestartInMin { get; set; }

    public void ToWriter(ref PacketWriter writer)
    {
        writer.Write<byte>(0xAB);
        writer.Write<byte>(0x03);
        writer.Write<byte>(255);
        writer.Write<ushort>(60000);
        writer.Write(0xC069E2E7);
        writer.Write(TimeUntilRestartInMin);
    }
}
