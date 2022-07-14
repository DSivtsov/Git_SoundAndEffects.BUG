using System;
using UnityEngine;
using UnityEngine.Events;

namespace GMTools
{
    /// <summary>
    /// Singleton for GameObject Unity
    /// Give a possibility to set Values to common variables in Inspector and receive access to it through static property
    /// </summary>
    public abstract class SingletonController<T> : MonoBehaviour where T : UnityEngine.Object
    {
        #region SingletonInternalPart
        private static T _instance;

        /// <summary>
        /// Static property to access the Singleton _instance
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    //If it the first time of access to the property and it occurred before Awake() of this class
                    //If raise the priority of execution of this script related to other, this check will not demanded
                    //But it will not work in Editor mode at Load because FindObjectOfType can found only active objects, which will be after Enter in Play mode

                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this as T)
            {
                //If was created many objects of this type, the attempt to execute the Awake() for second and others object of this type will issue the error message
                //It's not possible to estimate which object (of one type) will be executed first, therefore the simple Delete(GameObject) is not correct for this type
                //  of the Singleton, and how it used

                //Debug.LogError($"Find dubplicate of SingletonController [{this.name}]");
            }
            else
            {
                //Initialize the static variable by reference to current object
                //It will occur only for first object of this type
                _instance = this as T;
            }
        }
        #endregion
    } 
}



