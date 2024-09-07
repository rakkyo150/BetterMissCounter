using Zenject;

namespace BetterMissCounter.Installers
{
    public class PlayerInstaller: Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerBest>().AsSingle();
        }
    }
}
