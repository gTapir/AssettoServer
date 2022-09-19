using AssettoServer.Server.Plugin;
using Autofac;

namespace ServerRestartToastPlugin;

public class ServerRestartToastModule : AssettoServerModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ServerRestartToastPlugin>().AsSelf().AutoActivate().SingleInstance();
    }
}
