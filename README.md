# SingletonSystem (ALPHA VERSION - DONT USE IN NON-TEST PROJECT)
 
Tool for Unity singleton management using:
- ScriptableSingleton (ScriptableObject who is singleton)
- SingletonBehaviour (MonoBehaviour who is singleton)
- AppSingletonInstantiator
- SingletonComponent

## ScriptableSingleton

Create a class and make it inherit from ScriptableSingleton insted from ScriptableObject.

## SingletonBehaviour

Create a class and make it inherit from SingletonBehaviour instead from MonoBehaviour
When referring a SingletonBehaviour class who exists only in a scene, you can refer "LazyInstance" in early phases of a application lifetime. You'll be sure that if Singleton has not been initialized yet, it will be.
If you are referring a SingletonBehaviour class who exist for entire application lifetime, read below.

You can override OnInstantiate method if you have to execute scripts after its creation.

## AppSingletonInstantiator

You can create this Scriptable Object Singleton (it doesn't inherit from ScriptableSingleton) in order to create SingletonBehaviours who have to exist for entire application lifecycle.

It creates everyone of them when application starts. When referring a SingletonBehaviour class instantiated in this way you can avoid using LazyIstance.

## SingletonComponent
Let your class extends this interface in order to use OnInstantiate method called AFTER SingletonBehavior's OnInstantiate.
Called on GameObject childrens' SingletonComponent as well.