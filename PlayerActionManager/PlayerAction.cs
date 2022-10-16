using AssettoServer.Server;
using AssettoServer.Server.Plugin;
using AssettoServer.Utils;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace PlayerActionManager;

public class PlayerAction
{
    private EntryCarManager mEntryCarManager { get; set; }

    public PlayerAction(EntryCarManager entrycarManager)
    {
        mEntryCarManager = entrycarManager;
        mEntryCarManager.ClientConnected += mEntryCarManager_ClientConnected;
    }

    private void mEntryCarManager_ClientConnected(AssettoServer.Network.Tcp.ACTcpClient sender, EventArgs args)
    {
        //Hier müsste ein Request an das API Backend gesendet werden, der dafür sorgt, dass die richtigen Daten in die Datenbank geschrieben werden.
    }
}
