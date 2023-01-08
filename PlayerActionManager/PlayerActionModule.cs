using AssettoServer.Server.Plugin;
using Autofac;
using Serilog;

namespace PlayerActionManager;

public class PlayerActionModule : AssettoServerModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PlayerAction>().AsSelf().As<IAssettoServerAutostart>().SingleInstance();
    }
}
