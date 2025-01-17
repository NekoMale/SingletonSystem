# SingletonSystem (ALPHA VERSION - DONT USE IN NON-TEST PROJECT)
 
Tool for Unity singleton management using:
- ScriptableSingleton (ScriptableObject who is singleton)
- SingletonBehaviour (MonoBehaviour who is singleton)
- SingletonComponent

## ScriptableSingleton<T>

Create a class and make it inherit from ScriptableSingleton insted from ScriptableObject.
Initialized when Unity call its "OnEnable()" method or when someone calls its "Instance" and it has not been initialized yet.

## SingletonBehaviour<T>

Create a class and make it inherit from SingletonBehaviour instead from MonoBehaviour.
SingletonBehaviour initialized at execution order of "-100", however if you are not sure that its "Instance" referral is after Singleton initialization, you can refer "LazyInstance" instead.

You can override Initialize() method on your T class in order to initialize your singleton class.

## SingletonComponent
Extend this interface on you singleton components. Initialized AFTER SingletonBehavior's OnInstantiate.
Called on GameObject childrens' SingletonComponent as well.

### SingletonBehaviour gameObject lifecycle:

| Method | Order |
| --- | --- |
| OnEnable() | 1: At DefaultExecutionOrder = -100. <br/>**Note**: it should be executed after every Unity's monobehaviour and before every other custom monobehaviour if you don't edit Unity Execution Order |
| Initialize() | 2: Called inside OnEnable |
| SingletonComponents.Initialize() | Called after SingletonBehaviour's Initialize inside OnEnable, so SingletonBehaviour's T has been initialized yet and it's called before every other custom monobehaviour as well (OnEnable notes still apply) |
