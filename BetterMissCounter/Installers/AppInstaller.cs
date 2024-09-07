using Zenject;

namespace BetterMissCounter.Installers
{
    internal class AppInstaller: Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<BloomFontAsset>().AsSingle();
        }
    }
}
