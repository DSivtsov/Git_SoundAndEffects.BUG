using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GMTools
{

    /*  
        Example of usage:
        [SerializeField, EnumShowAsPropertyAttribute("CurrentJukeboxMode")]
        private JukeBoxMode _jukeboxMode;
        public JukeBoxMode CurrentJukeboxMode
        {
            get => _jukeboxMode;
            set
            {
                Debug.Log($"CurrentJukeboxMode.SET({value})");
                _jukeboxMode = value;
            }
        }
     */
    /// <summary>
    /// Attribute for Enum field which give a possibility to use that field as backing field for the Property the same Type
    /// </summary>
    public class EnumShowAsPropertyAttribute : PropertyAttribute
    {
        public readonly string nameGetSetProperty;
        public readonly Type typeofEnum;

        //As parameter must be given the name (string GetSetProperty) of corresponding Property {get public; set public}
        public EnumShowAsPropertyAttribute(string GetSetProperty)
        {
            this.nameGetSetProperty = GetSetProperty;
        }

        //public EnumShowAsPropertyAttribute(string GetSetProperty, Type typeofEnum)
        //{
        //    this.nameGetSetProperty = GetSetProperty;
        //    this.typeofEnum = typeofEnum;
        //}

    } 
}
