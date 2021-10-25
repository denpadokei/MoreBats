using MoreBats.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MoreBats
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class MoreBatsController : MonoBehaviour, IInitializable
    {
        public void Initialize()
        {
            this._bats = Resources.FindObjectsOfTypeAll<ParticleSystem>().FirstOrDefault(x => x.name == "Bats");
            var main = this._bats.main;
            main.maxParticles = int.MaxValue;
            var em = this._bats.emission;
            em.rateOverTime = PluginConfig.Instance.BatsCount;
        }

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            Plugin.Log?.Debug($"{name}: Awake()");
        }
        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            this._bats = null;
        }

        private ParticleSystem _bats;
        #endregion
    }
}
