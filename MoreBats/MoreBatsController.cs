using MoreBats.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        }

        public IEnumerator GetBat()
        {
            if (this._bats != null) {
                yield break;
            }
            var neon = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == "SaberNeon");
            var neonRoot = neon.transform.parent;
            yield return SceneManager.LoadSceneAsync("HalloweenEnvironment", LoadSceneMode.Additive);
            try {
                var batsParticle = Resources.FindObjectsOfTypeAll<ParticleSystem>().FirstOrDefault(x => x.name == "Bats");
                this._bats = Instantiate(batsParticle);
                var main = this._bats.main;
                main.maxParticles = int.MaxValue;
                var em = this._bats.emission;
                em.rateOverTime = PluginConfig.Instance.BatsCount;
                this._bats.transform.SetParent(neonRoot, false);
                this._bats.transform.localPosition = new Vector3(-8, -3, 0);
                Plugin.Log.Debug($"{this._bats}");
            }
            catch (Exception e) {
                Plugin.Log.Error(e);
            }
            yield return SceneManager.UnloadSceneAsync("HalloweenEnvironment");
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
            if (this._bats != null) {
                Destroy(this._bats);
                this._bats = null;
            }
        }
        private void Start()
        {
            this.StartCoroutine(this.GetBat());
        }

        private ParticleSystem _bats;
        #endregion
    }
}
