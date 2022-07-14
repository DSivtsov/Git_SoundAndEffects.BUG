using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GMTools
{

    // Color list was taken from https://docs.unity3d.com/ScriptReference/ColorUtility.TryParseHtmlString.html
    public enum BaseColor : byte
    {
        red, cyan, blue, darkblue, lightblue, purple, yellow, lime, fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy, teal, aqua, magenta
    }

    public class ReadOnlyAttribute : PropertyAttribute
    {
        private Color _valueColor;
        public Color ValueColor { get => _valueColor; }
        public bool IsColorNonStandard { get; private set; } = false;

        // In this case will be used the standard Color for EditorStyles.label
        public ReadOnlyAttribute()
        {
            //Will use IsColorNonStandard = false
        }

        public ReadOnlyAttribute(BaseColor enumValueColor)
        {
            if (ColorUtility.TryParseHtmlString(enumValueColor.ToString(), out this._valueColor))
                IsColorNonStandard = true;
        }

        /* As strValueColor must be used the next Color Format:
         * Strings that begin with '#' will be parsed as hexadecimal in the following way:
         * #RGB (becomes RRGGBB)#RRGGBB #RGBA (becomes RRGGBBAA) #RRGGBBAA
         * or
         * Strings that do not begin with '#' will be parsed as literal colors the Color name as Display values in BaseColor
         */
        public ReadOnlyAttribute(string strValueColor)
        {
            if (ColorUtility.TryParseHtmlString(strValueColor.ToString(), out this._valueColor))
                IsColorNonStandard = true;
        }
    } 
}