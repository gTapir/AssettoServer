using AssettoServer.Commands;
using Qmmands;
using Serilog;

namespace PlayerActionManager;

public class PlayerActionCommandModule : ACModuleBase
{
    [Command("sampleplugin")]
    public void SamplePlugin()
    {
        Reply("Hello from sample plugin!");
    }
}
