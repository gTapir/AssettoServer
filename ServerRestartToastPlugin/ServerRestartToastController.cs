using AssettoServer.Server;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ServerRestartToastPlugin;

[ApiController]
public class ServerRestartToastController : ControllerBase
{
    private readonly ACServer _server;
    public static EntryCarManager EntryCarManager;
    
    public ServerRestartToastController(ACServer server)
    {
        _server = server;
    }

    [HttpGet("/serverrestarttoast")]
    public string ShowRestartToast()
    {
        Log.Debug("API request erhalten!");
        if (EntryCarManager != null)
        {
            Log.Debug("EntryCarManager ist nicht null --> Sende jetzt Pakete an Clients");
            Log.Debug($"Es sind {EntryCarManager.EntryCars.Count()} auf dem Server.");
            foreach (var entryCar in EntryCarManager.EntryCars)
            {
                if (entryCar.Client != null)
                {
                    Log.Debug($"Der aktuelle Client ist: {entryCar.Client}");
                    entryCar.Client.SendPacket(new ServerRestartToast
                    {
                        TimeUntilRestartInMin = 5
                    });
                }
            }
        }
        return "Hello from sample plugin!";
    }
}
