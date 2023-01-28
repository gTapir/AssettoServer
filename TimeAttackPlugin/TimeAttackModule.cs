using AssettoServer.Server.Plugin;
using Autofac;

namespace TimeAttackPlugin;

public class TimeAttackModule : AssettoServerModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TimeAttack>().AsSelf().AutoActivate().SingleInstance();
    }
}
