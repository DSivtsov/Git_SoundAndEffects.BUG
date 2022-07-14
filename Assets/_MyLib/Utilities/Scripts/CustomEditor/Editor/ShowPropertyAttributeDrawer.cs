using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

//Attribute which implement CustomPropertyDrawer for Enum fields which values controlled by corresponding the Property
//PropertyDrawer for Attribute EnumShowAsPropertyAttribute
namespace GMTools
{
    [CustomPropertyDrawer(typeof(EnumShowAsPropertyAttribute))]
    public class ShowPropertyAttributeDrawer : PropertyDrawer
    {
        object parent;
        PropertyInfo info;
        Type typeofEnum;

        bool NotInit = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (NotInit)
            {
                InitAttributeForProperty(property);
            }
            Enum value = (Enum)Enum.ToObject(typeofEnum, property.enumValueIndex);
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            value = EditorGUI.EnumPopup(position, label, value);
            if (EditorGUI.EndChangeCheck())
            {
                if (info == null)
                    Debug.LogError("Init ShowProperty Attribute for Property was not successful");
                else
                {
                    //!!! Not activate the row below because the "set value to field which displayed in Inspector"
                    // - UnityEditor.EditorGUI/PopupCallbackInfo:SetEnumValueDelegate (object,string[],int)
                    //will be after the "Set value to Property" and will overwrite value set through Property
                    //!!! property.enumValueIndex = (int)newValue;
                    // int t = Convert.ToInt32(newValue);
                    //property.enumValueIndex = property.intValue;
                    //This Set value to Property which controlled the SetValue() method
                    info.SetValue(parent, value);
                }
            }
            EditorGUI.EndProperty();
        }

        //The access to setter of Property received through nameGetSetProperty parameter(string) of Attribute by Reflection
        private void InitAttributeForProperty(SerializedProperty property)
        {
            EnumShowAsPropertyAttribute myAttribute = attribute as EnumShowAsPropertyAttribute;
            if (myAttribute == null)
            {
                Debug.LogError("Invalid property attribute \"" + attribute + "\"");
            }
            else
            {
                string nameGetSetProperty = myAttribute.nameGetSetProperty;
                //typeofEnum = myAttribute.typeofEnum;
                parent = GetParentObject(property.propertyPath, property.serializedObject.targetObject);
                var type = parent.GetType();
                info = type.GetProperty(nameGetSetProperty);
                //Debug.Log($"info.PropertyType={info.PropertyType}");
                typeofEnum = info.PropertyType;
                if (info == null)
                    Debug.LogError("Invalid property name \"" + nameGetSetProperty + "\"");
                NotInit = false;
            }
        }

        public static object GetParentObject(string path, object obj)
        {
            var fields = path.Split('.');

            if (fields.Length == 1)
                return obj;

            FieldInfo info = obj.GetType().GetField(fields[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            obj = info.GetValue(obj);

            return GetParentObject(string.Join(".", fields, 1, fields.Length - 1), obj);
        }



    } 
}
