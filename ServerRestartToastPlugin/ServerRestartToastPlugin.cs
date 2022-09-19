using System.Reflection;
using AssettoServer.Server;
using AssettoServer.Server.Configuration;
using Serilog;

namespace ServerRestartToastPlugin;

public class ServerRestartToastPlugin
{
    public ServerRestartToastPlugin(EntryCarManager entryCarManager, CSPServerScriptProvider scriptProvider)
    {
        ServerRestartToastController.EntryCarManager = entryCarManager;
        Log.Debug("EntryCarManager gespeichert.");

        scriptProvider.AddScript(
            new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ServerRestartToastPlugin.lua.serverrestarttoast.lua")!).ReadToEnd(),
            "serverrestarttoast.lua"
        );
        Log.Debug("Toast Script wurde geladen :)");
    }
}
