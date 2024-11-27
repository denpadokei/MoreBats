using MoreBats.Configuration;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        [Inject]
        private readonly ZenjectSceneLoader zenjectSceneLoader;
        [Inject]
        private readonly GameScenesManager gameScenesManager;
        private readonly Material material;

        public void Initialize()
        {
        }

        public IEnumerator GetBat()
        {
            if (this._bats != null)
            {
                yield break;
            }
            var neon = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == "SaberNeon");
            var neonRoot = neon.transform.parent;
            var i = this.zenjectSceneLoader.LoadSceneFromAddressablesAsync("HalloweenEnvironment", LoadSceneMode.Additive, true, 0, null, null, LoadSceneRelationship.None, null);
            yield return i;
            //yield return Addressables.LoadSceneAsync("HalloweenEnvironment", LoadSceneMode.Additive);
            //yield return SceneManager.LoadSceneAsync(@"Scenes/HalloweenEnvironment", LoadSceneMode.Additive);
            try
            {
                var batsParticle = Resources.FindObjectsOfTypeAll<ParticleSystem>().FirstOrDefault(x => x.name == "Bats");
                this._batsGO = Instantiate(batsParticle.gameObject);
                this._bats = this._batsGO.GetComponent<ParticleSystem>();

                var orgMat = batsParticle.gameObject.GetComponent<ParticleSystemRenderer>().sharedMaterial;
                this.CopyTexture(orgMat.mainTexture, out var dest);
                this._batMaterial = Instantiate(orgMat);
                this._batMaterial.mainTexture = dest;

                this._batsRenderer = this._batsGO.GetComponent<ParticleSystemRenderer>();
                this._batsRenderer.material = this._batMaterial;

                var main = this._bats.main;
                main.maxParticles = int.MaxValue;
                var em = this._bats.emission;
                em.rateOverTime = PluginConfig.Instance.BatsCount;
                this._bats.transform.SetParent(neonRoot, false);
                this._bats.transform.localPosition = new Vector3(-8, -3, 0);
                Plugin.Log.Debug($"{this._bats}");
            }
            catch (Exception e)
            {
                Plugin.Log.Error(e);
            }
            yield return Addressables.UnloadSceneAsync(i, true);
        }

        private void CopyTexture(Texture src, out Texture2D dest)
        {
            dest = null;
            if (!src)
            {
                return;
            }
            if (src is Texture2D src2D)
            {

                dest = new Texture2D(src2D.width, src2D.height, TextureFormat.ARGB32, false);
                var renderTexture = new RenderTexture(dest.width, dest.height, 32);

                Graphics.Blit(src2D, renderTexture);
                var orgActive = RenderTexture.active;
                RenderTexture.active = renderTexture;

                dest.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                dest.Apply();
                RenderTexture.active = orgActive;

                Destroy(renderTexture);
            }
        }

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            Plugin.Log?.Debug($"{this.name}: Awake()");
        }
        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{this.name}: OnDestroy()");
            if (this._batsGO != null)
            {
                Destroy(this._batsGO);
                this._batsGO = null;
            }
            if (_batMaterial)
            {
                Destroy(_batMaterial);
                this._batMaterial = null;
            }
        }
        private void Start()
        {
            _ = this.StartCoroutine(this.GetBat());
        }

        private GameObject _batsGO;
        private ParticleSystem _bats;
        private ParticleSystemRenderer _batsRenderer;
        private Material _batMaterial;
        #endregion
    }
}
