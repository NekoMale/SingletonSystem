using UnityEngine;

namespace NamelessGames.SingletonSystem
{
    /// <summary>
    /// Don't inherit from this. Inherit from SingletonBehaviour<T> instead
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    public abstract class SingletonBehaviour : MonoBehaviour
    {
        private protected SingletonBehaviour() { }

        protected abstract void OnEnable();

        public abstract void Instantiated();
    }

    /// <summary>
    /// Inherit this class in order to have your MonoBehaviour as singleton for the entire execution of application.<br></br>
    /// </summary>
    /// <typeparam name="T">Type of SingletonBehaviour</typeparam>
    public abstract class SingletonBehaviour<T> : SingletonBehaviour where T : SingletonBehaviour<T>
    {
        [SerializeField] bool _dontDestroyOnLoad;
        static bool _initialized = false;

        public static T Instance { get; private set; }

        /// <summary>
        /// Use this if you are calling this Singleton and you are not sure it's been initialized yet.<br></br>
        /// If you are sure it is, use Instance instead for better performace.
        /// </summary>
        public static T LazyInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<T>();
                    if (Instance == null)
                    {
                        Debug.LogError($"Singleton<{typeof(T)} could not be instantiated: no object of type {typeof(T)} found.");
                        return null;
                    }
                    Instance.Instantiated();
                }
                return Instance;
            }
        }

        public sealed override void Instantiated()
        {
            if(Instance == null)
            {
                Instance = (T)this;
            }
            else if (Instance != this)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                return;
            }
            if (!_initialized) // if Instance is this and it is not initialized yet
            {
                _initialized = true;
                if(_dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
                
                Initialize();
                InitializeComponents();
            }
        }

        /// <summary>
        /// Initialize here your SingletonBehaviour.<br></br>
        /// Always executed before everything in game and after this singleton Instance assignment.<br></br>
        /// Executed before every SingletonComponent initialization as well.
        /// </summary>
        public virtual void Initialize() { }

        private void InitializeComponents()
        {
            SingletonComponent[] singletonComponents = GetComponentsInChildren<SingletonComponent>();
            for (int componentIndex = 0; componentIndex < singletonComponents.Length; componentIndex++)
            {
                singletonComponents[componentIndex].Initialize();
            }
        }

        protected sealed override void OnEnable()
        {
            Instantiated();
        }
    }
}