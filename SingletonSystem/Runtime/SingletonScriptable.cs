using System.Linq;
using UnityEngine;

namespace NamelessGames.SingletonSystem
{
    public abstract class SingletonScriptable : ScriptableObject 
    {
        public abstract void Instantiated();

        private void OnEnable()
        {
            Instantiated();
        }
    }

    public abstract class SingletonScriptable<T> : SingletonScriptable where T : SingletonScriptable<T>
    {
        public static T Instance { get; private set; }

        public override sealed void Instantiated()
        {
            if (Instance != null) { return; }
            Instance = this as T;

            OnInstantiate();
        }

        public virtual void OnInstantiate() { }

#if UNITY_EDITOR
        public static void CreateAsset()
        {
            var path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save " + typeof(T) + " creation", typeof(T).ToString(), "asset", string.Empty);
            if (string.IsNullOrEmpty(path))
                return;

            var configObject = CreateInstance<T>();
            UnityEditor.AssetDatabase.CreateAsset(configObject, path);

            // Add the config asset to the build
            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.Add(configObject);
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }
#endif
    }
}