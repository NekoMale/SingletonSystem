using UnityEngine;

namespace NamelessGames.SingletonSystem
{
    /// <summary>
    /// Use this class for every class unique in scene and that exists only in specific scene.<br></br>
    /// It's not a real singleton that will exists all game duration and it can have different settings scene by scene.<br></br>
    /// Every GameObject can have only a SingletonBehaviour component. Every other component can inherit SingletonComponent interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DisallowMultipleComponent]
    public abstract class SceneSingletonBehaviour<T> : MonoBehaviour where T : SceneSingletonBehaviour<T>
    {
        static bool _hasBeenInstantiated = false;

        /// <summary>
        /// Use this if you are calling this Singleton and you are sure it has already been initialized.<br></br>
        /// Use LazyInstance if you are not.
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// Use this if you are calling this Singleton when you are not sure about calling when it's been initialized yet.<br></br>
        /// If you are sure, use Instance instead for better performace.
        /// </summary>
        public static T LazyInstance
        {
            get
            {
                if (!_hasBeenInstantiated)
                {
                    T instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        Debug.LogError("Non esiste nessun SceneSingletonBehaviour di tipo " + typeof(T) + "\nMetti in scena lo SceneSingletonBehaviour desiderato.");
                        return null;
                    }
                    instance.Initialize();
                }
                return Instance;
            }
        }

        private void Initialize()
        {
            if (!_hasBeenInstantiated)
            {
                Instance = (T)this;
                OnInstantiate();

                SingletonComponent[] singletonComponents = GetComponentsInChildren<SingletonComponent>();
                for (int componentIndex = 0; componentIndex < singletonComponents.Length; componentIndex++)
                {
                    singletonComponents[componentIndex].OnInstantiate();
                }
                _hasBeenInstantiated = true;
            }
            else if (Instance != this)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            Initialize();
        }

        /// <summary>
        /// Called when this singleton instance has been set. You can set your singleton initialization here<br></br>
        /// Always called before SingletonComponent.OnInstantiate()<br></br>
        /// If this Singleton is active when Scene is loaded, it's always called before of MonoBehaviour's OnEnable() and Awake() using LazyInstance.
        /// </summary>
        public virtual void OnInstantiate() { }
    }
}