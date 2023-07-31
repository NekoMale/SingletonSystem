namespace NamelessGames.SingletonSystem
{
    /// <summary>
    /// Use this for components on GameObjects who have a SingletonBehaviour or a SceneSingletonBehaviour.<br></br>
    /// Useful in order to inizialize singleton's component.
    /// </summary>
    public interface SingletonComponent
    {
        /// <summary>
        /// Initialize MonoBehaviour whene singleton GameObject is instantiated.<br></br>
        /// Always executed after SingletonMonoBehaviour or SceneSingletonMonobehaviour initialization.
        /// </summary>
        void OnInstantiate();
    }
}