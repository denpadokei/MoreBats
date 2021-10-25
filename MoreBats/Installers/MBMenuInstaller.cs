using MoreBats.Views;
using SiraUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace MoreBats.Installers
{
    public class MBMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<MoreBatsController>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
            this.Container.BindInterfacesAndSelfTo<SettingViewController>().FromNewComponentAsViewController().AsCached().NonLazy();
        }
    }
}
