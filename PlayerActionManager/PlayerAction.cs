using AssettoServer.Server;
using AssettoServer.Server.Plugin;
using AssettoServer.Utils;
using Microsoft.Extensions.Hosting;
using RestSharp;
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
        var client = new RestClient(/*Hier Base URL einfügen*/);
        var request = new RestRequest(/*Hier konkreten Pfad angeben zum Controller und zur Methode*/);
        request.AddObject(/*Hier muss das Requestmodel gebaut werden*/);
        var response = client.Post</*ResponseModel einfügen*/>(request);
    }
}
