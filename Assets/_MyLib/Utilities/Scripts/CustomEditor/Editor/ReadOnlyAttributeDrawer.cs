using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GMTools
{
    //ReadOnly Attribute fully work only for enum
    //For other types it works as standard field of coresponding type
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        private bool _notInit = true;
        GUIStyle _myStyle;
        string currentValue = default;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_notInit)
            {
                ReadOnlyAttribute parameters = (ReadOnlyAttribute)attribute;
                if (parameters.IsColorNonStandard)
                    _myStyle = new GUIStyle(EditorStyles.label) { normal = new GUIStyleState() { textColor = parameters.ValueColor } };
                else
                    _myStyle = new GUIStyle(EditorStyles.label);
                _notInit = false;
                //Debug.Log($"_myStyle({(_myStyle == null ? "_myStyle == null":_myStyle.ToString())})");
            }
            switch (property.propertyType)
            {
                //case SerializedPropertyType.Generic:
                //    break;
                case SerializedPropertyType.Integer:
                    currentValue = property.intValue.ToString();
                    break;
                case SerializedPropertyType.Boolean:
                    currentValue = property.boolValue.ToString();
                    break;
                case SerializedPropertyType.Float:
                    currentValue = property.floatValue.ToString();
                    break;
                case SerializedPropertyType.String:
                    currentValue = property.stringValue;
                    break;
                case SerializedPropertyType.Color:
                    currentValue = property.colorValue.ToString();
                    break;
                //case SerializedPropertyType.ObjectReference:
                //    break;
                //case SerializedPropertyType.LayerMask:
                //    break;
                case SerializedPropertyType.Enum:
                    currentValue = property.enumDisplayNames[property.enumValueIndex];
                    break;
                case SerializedPropertyType.Vector2:
                    currentValue = property.vector2Value.ToString();
                    break;
                case SerializedPropertyType.Vector3:
                    currentValue = property.vector3Value.ToString();
                    break;
                case SerializedPropertyType.Vector4:
                    currentValue = property.vector4Value.ToString();
                    break;
                case SerializedPropertyType.Rect:
                    currentValue = property.rectValue.ToString();
                    break;
                //case SerializedPropertyType.ArraySize:
                //    break;
                //case SerializedPropertyType.Character:
                //    break;
                //case SerializedPropertyType.AnimationCurve:
                //    break;
                case SerializedPropertyType.Bounds:
                    currentValue = property.boundsValue.ToString();
                    break;
                //case SerializedPropertyType.Gradient:
                //    break;
                case SerializedPropertyType.Quaternion:
                    currentValue = property.quaternionValue.ToString();
                    break;
                //case SerializedPropertyType.ExposedReference:
                //    break;
                //case SerializedPropertyType.FixedBufferSize:
                //    break;
                case SerializedPropertyType.Vector2Int:
                    currentValue = property.vector2IntValue.ToString();
                    break;
                case SerializedPropertyType.Vector3Int:
                    currentValue = property.vector3IntValue.ToString();
                    break;
                case SerializedPropertyType.RectInt:
                    currentValue = property.rectIntValue.ToString();
                    break;
                case SerializedPropertyType.BoundsInt:
                    currentValue = property.boundsIntValue.ToString();
                    break;
                //case SerializedPropertyType.ManagedReference:
                //    break;
                default:
                    EditorGUI.PropertyField(position, property, label, true);
                    return;
            }
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, label, new GUIContent(currentValue), _myStyle);
            EditorGUI.EndProperty();
        }
    } 
}