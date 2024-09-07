using Zenject;

namespace BetterMissCounter.Installers
{
    internal class PlayerInstaller: Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerInstaller>().AsSingle();
            Container.Bind<BloomFontAsset>().AsSingle();
        }
    }
}
