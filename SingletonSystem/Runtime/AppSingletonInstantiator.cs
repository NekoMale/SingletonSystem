using System.Linq;
using UnityEngine;

namespace NamelessGames.SingletonSystem
{
    [CreateAssetMenu(fileName = "App Singleton Instantiator", menuName = "Nameless Games/Singleton System/New App Singleton Instantiator")]
    public class AppSingletonInstantiator : ScriptableObject
    {
        [Tooltip("Drag here Singleton GameObjects who has to exists for all application lifecycle")]
        [SerializeField] SingletonBehaviour[] _singletonBehaviours;

        private static AppSingletonInstantiator _instance;

        /// <summary>
        /// Instantiate every singletons has to exists for all application lifecycle
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InstantiateSingletons()
        {
            if(_instance == null) { return; }
            GameObject root = new GameObject("Singleton Root");
            root.SetActive(false);
            DontDestroyOnLoad(root);

            for (int singletonIndex = 0; singletonIndex < _instance._singletonBehaviours.Length; singletonIndex++)
            {
                Instantiate(_instance._singletonBehaviours[singletonIndex], root.transform);

                root.transform.DetachChildren();
            }

            Destroy(root);
        }

        private void OnEnable()
        {
            _instance = this;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Nameless Games/Singleton System/New Singleton Instantiator")]
        public static void CreateAsset()
        {
            var path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Config", "config", "asset", string.Empty);
            if (string.IsNullOrEmpty(path))
                return;

            var configObject = CreateInstance<AppSingletonInstantiator>();
            UnityEditor.AssetDatabase.CreateAsset(configObject, path);

            // Add the config asset to the build
            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.Add(configObject);
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }
#endif
    }
}