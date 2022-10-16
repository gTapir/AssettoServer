using AssettoServer.Server;
using Microsoft.AspNetCore.Mvc;

namespace PlayerActionManager;

[ApiController]
public class PlayerActionController : ControllerBase
{
    private readonly ACServer _server;
    
    public PlayerActionController(ACServer server)
    {
        _server = server;
    }

    [HttpGet("/sampleplugin")]
    public string Sample()
    {
        return "Hello from sample plugin!";
    }
}
