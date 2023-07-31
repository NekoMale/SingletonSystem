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
        public static T Instance { get; private set; }

        public sealed override void Instantiated()
        {
            Instance = (T)this;
            OnInstantiate();
        }

        /// <summary>
        /// Initialize here your SingletonBehaviour.<br></br>
        /// Always executed before everything in game and after this singleton Instance assignment.<br></br>
        /// Executed before every SingletonComponent initialization as well.
        /// </summary>
        public virtual void OnInstantiate() { }
    }
}