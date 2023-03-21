using AssettoServer.Network.Packets;
using AssettoServer.Network.Packets.Incoming;
using AssettoServer.Network.Packets.Outgoing;
using AssettoServer.Server.GeoParams;
using AssettoServer.Server;
using AssettoServer.Server.Plugin;
using AssettoServer.Network.Tcp;
using Serilog;
using System.Text;

namespace TimeAttackPlugin;

public class TimeAttack : IAssettoServerAutostart
{
    public TimeAttack(CSPClientMessageTypeManager clientMessageTypeManager)
    {
        Log.Debug("Initializing TimeAttackPlugin");
        clientMessageTypeManager.RegisterClientMessageType(0x91C1F6D7, IncomingTimeAttackResult);
        Log.Debug("Successfully registered ClientMessageType for TimeAttack");
    }

    private static void IncomingTimeAttackResult(ACTcpClient client, PacketReader reader)
    {
        Log.Debug($"Recieved a message from {client.Name}. Trying to read it now...");
        string value = reader.ReadStringFixed(Encoding.UTF8, 31);
        Log.Debug($"Read content of TimeAttack message. It contained the following: {value}");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
