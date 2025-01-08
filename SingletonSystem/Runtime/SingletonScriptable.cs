using System.Linq;
using UnityEngine;

namespace NamelessGames.SingletonSystem
{
    /// <summary>
    /// Don't inherit from this. Inherit from SingletonScriptable<T> instead
    /// </summary>
    public abstract class SingletonScriptable : ScriptableObject 
    {
        public abstract void Instantiated();

        protected abstract void OnEnable();
    }

    /// <summary>
    /// Inherit this class in order to have your ScriptableObject as singleton.<br></br>
    /// </summary>
    /// <typeparam name="T">Type of SingletonScriptable</typeparam>
    public abstract class SingletonScriptable<T> : SingletonScriptable where T : SingletonScriptable<T>
    {
        static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] instances = Resources.LoadAll<T>("");
                    if (instances.Length == 0)
                    {
                        Debug.LogError("No instance of " + typeof(T) + " found in Resources folder.");
                        return null;
                    }
                    if (instances.Length > 1)
                    {
                        Debug.LogError("Multiple instances of " + typeof(T) + " found in Resources folder.");
                        return null;
                    }
                    _instance = instances[0];
                }
                return _instance;
            }
        }

        protected sealed override void OnEnable()
        {
            Instantiated();
        }

        public override sealed void Instantiated()
        {
            if (Instance != null && Instance != this)
                DestroyImmediate(this);
            else if (Instance == this)
                return;

            _instance = this as T;
            Initialize();
        }

        public virtual void Initialize() { }
    }
}