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
        if (EntryCarManager != null)
        {
            foreach (var entryCar in EntryCarManager.EntryCars)
            {
                if (entryCar.Client != null)
                {
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
