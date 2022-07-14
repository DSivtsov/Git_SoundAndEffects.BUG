
/*Another option might be to split the class into two parts: a regular static class for the Singleton component,
 * and a MonoBehaviour that acts as a controller for the singleton instance.This way you have full control over the singleton's construction, and it will persist across scenes.
 * This also lets you add controllers to any object that might need the singleton's data, instead of having to dig through the scene to find a particular component.
*/
namespace GMTools
{
    [System.Serializable]
    public class Singleton
    {
        //public CharacterData dwarfData;
        //public CharacterData robotfData;
        private Singleton()
        {
            //Class initialization goes here.
        }

        public void someSingletonMethod()
        {
            //Some method that acts on the Singleton.
        }

        private static Singleton _instance;
        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Singleton();
                return _instance;
            }
        }
    } 
}


