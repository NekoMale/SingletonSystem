using UnityEngine;

namespace NamelessGames.SingletonSystem
{
    [CreateAssetMenu(fileName = "Singleton Instantiator", menuName = "Nameless Games/Singleton System/New Singleton Instantiator")]
    public class SingletonInstantiator : ScriptableObject
    {
        [Tooltip("Drag here Singleton GameObjects who has to exists for all application lifecycle")]
        [SerializeField] SingletonBehaviour[] _singletons;

        private static SingletonInstantiator instance;

        /// <summary>
        /// Instantiate every singletons has to exists for all application lifecycle
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InstantiateSingletons()
        {
            GameObject root = new GameObject("Singleton Root");
            root.SetActive(false);
            DontDestroyOnLoad(root);

            SingletonComponent[] singletonComponents;
            for (int singletonIndex = 0; singletonIndex < instance._singletons.Length; singletonIndex++)
            {
                SingletonBehaviour singletonInstance = Instantiate(instance._singletons[singletonIndex], root.transform);
                singletonInstance.Instantiated();

                singletonComponents = singletonInstance.GetComponentsInChildren<SingletonComponent>();
                for (int componentIndex = 0; componentIndex < singletonComponents.Length; componentIndex++)
                {
                    singletonComponents[componentIndex].OnInstantiate();
                }

                root.transform.DetachChildren();
            }

            Destroy(root);
        }

        private void OnEnable()
        {
            instance = this;
        }
    }
}