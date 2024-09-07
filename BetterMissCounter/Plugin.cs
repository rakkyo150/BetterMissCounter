using BetterMissCounter.Installers;
using IPA;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using System.Runtime.CompilerServices;
using IPALogger = IPA.Logging.Logger;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace BetterMissCounter
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        public Plugin(IPALogger logger, IPA.Config.Config config, Zenjector zenjector)
        {
            Instance = this;
            Log = logger;
            PluginConfig.Instance = config.Generated<PluginConfig>();
            zenjector.Install<PlayerInstaller>(Location.Player);
            zenjector.Install<AppInstaller>(Location.App);
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Plugin.Log.Info("meow");
        }

        [OnExit]
        public void OnApplicationQuit()
        {

        }
    }
}
