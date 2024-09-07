using IPA;
using IPALogger = IPA.Logging.Logger;
using IPA.Config.Stores;
using System.Runtime.CompilerServices;
using SiraUtil.Zenject;
using BetterMissCounter.Installers;

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
