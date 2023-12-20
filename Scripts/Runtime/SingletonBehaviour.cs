using UnityEngine;

namespace NamelessGames.SingletonSystem
{
    /// <summary>
    /// Don't inherit from this. Inherit from SingletonBehaviour<T> instead
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class SingletonBehaviour : MonoBehaviour
    {
        private protected SingletonBehaviour() { }

        public abstract void Instantiated();
    }

    /// <summary>
    /// Inherit this class in order to have your MonoBehaviour as singleton for the entire execution of application.<br></br>
    /// </summary>
    /// <typeparam name="T">Type of SingletonBehaviour</typeparam>
    public abstract class SingletonBehaviour<T> : SingletonBehaviour where T : SingletonBehaviour<T>
    {
        static bool _hasBeenInstantiated = false;

        public static T Instance { get; private set; }

        /// <summary>
        /// Use this if you are calling this Singleton and you are not sure it's been initialized yet.<br></br>
        /// If you are sure it is, use Instance instead for better performace.
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
                        Debug.LogError("Non esiste nessun SingletonBehaviour di tipo " + typeof(T) + "\nMetti in scena il SingletonBehaviour desiderato oppure aggiungilo nel SingletonInstantiator.");
                        return null;
                    }
                    instance.Instantiated();
                }
                return Instance;
            }
        }

        public sealed override void Instantiated()
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

        /// <summary>
        /// Initialize here your SingletonBehaviour.<br></br>
        /// Always executed before everything in game and after this singleton Instance assignment.<br></br>
        /// Executed before every SingletonComponent initialization as well.
        /// </summary>
        public virtual void OnInstantiate() { }

        private void OnEnable()
        {
            Instantiated();
        }
    }
}