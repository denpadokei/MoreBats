using IPA;
using IPA.Config;
using IPA.Config.Stores;
using MoreBats.Installers;
using SiraUtil.Zenject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using IPALogger = IPA.Logging.Logger;

namespace MoreBats
{
#if DEBUG

    internal class TestClass : IInitializable
    {
        public void Initialize()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var s = SceneManager.GetSceneAt(i);
                Plugin.Log.Info($"{s.buildIndex}, {s.name}, {s.path}");
            }
        }

        [Inject]
        public TestClass()
        {
            
        }
    }
#endif


    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, Zenjector zenjector, Config conf)
        {
            Instance = this;
            Log = logger;
            Log.Info("MoreBats initialized.");
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
            zenjector.Install<MBMenuInstaller>(Location.Menu);
#if DEBUG
            zenjector.Install(Location.Player, d => d.BindInterfacesAndSelfTo<TestClass>().AsSingle());
#endif
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");
        }
    }
}
