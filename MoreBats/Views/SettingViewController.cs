using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using MoreBats.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MoreBats.Views
{
    [HotReload]
    internal class SettingViewController : BSMLAutomaticViewController, IInitializable
    {
        [UIValue("bats-count")]
        public int BatsCount
        {
            get => PluginConfig.Instance.BatsCount;
            set => PluginConfig.Instance.BatsCount = value;
        }

        // For this method of setting the ResourceName, this class must be the first class in the file.
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);

        public void Initialize()
        {
            BSMLSettings.instance.AddSettingsMenu("More Bats", ResourceName, this);
        }
    }
}
