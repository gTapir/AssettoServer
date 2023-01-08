using AssettoServer.Server;
using AssettoServer.Server.GeoParams;
using AssettoServer.Server.Plugin;
using Grpc.Net.Client;
using Serilog;

namespace PlayerActionManager;

public class PlayerAction : IAssettoServerAutostart
{
    private EntryCarManager mEntryCarManager { get; set; }
    public GeoParamsManager mGeoParamsManager { get; set; }

    public PlayerAction(EntryCarManager entrycarManager, GeoParamsManager geoParamsManager)
    {
        mEntryCarManager = entrycarManager;
        mGeoParamsManager = geoParamsManager;

        mEntryCarManager.ClientConnected += mEntryCarManager_ClientConnected;
    }

    private async void mEntryCarManager_ClientConnected(AssettoServer.Network.Tcp.ACTcpClient sender, EventArgs args)
    {
        try
        {
            using (var channel = GrpcChannel.ForAddress("https://localhost:5001"))
            {
                var client = new EvaluationData.EvaluationDataClient(channel);
                _ = await client.UpdateEvaluationDataAsync(new EvaluationDataRequest()
                {
                    SteamID = sender.Guid.ToString(),
                    GamerTag = sender.Name,
                    CurrentServerIP = mGeoParamsManager.GeoParams.Ip,
                    CurrentCarName = sender.EntryCar.Model
                });
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Request to backend failed with error: {ex.Message}");
        }
        
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
