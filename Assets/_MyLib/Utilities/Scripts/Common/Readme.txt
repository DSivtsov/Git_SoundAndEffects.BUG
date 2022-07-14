Singleton.cs & SingletonController.cs - singleton which based on C# class. It doesn't exist not in Run mode, threfore can store data from Editor and must initializate after "Run"

SingletonGameObject.cs based on GameObject threfore exist in Editor also and can store data from Editor

SingletonController<T>.cs (abstract class SingletonController<T> : MonoBehaviour where T : UnityEngine.Object)
Can be used as base class for any Singleton GameObject (T can be any Component or UnityEngine.Object), as:

public class SingletonController : SingletonController<SingletonController>
{
	#region SingletonExternalPart
	/* 
	 * Heare place to put the variables or methods
	 * Also recomended to set for this script "Script Execution Order" higher then "Default Time"
	*/
	[SerializeField] private MyCharacterController characterController;
	#endregion

}
After that they can be accessed from every script as SingletonController.Instance.<variable>

