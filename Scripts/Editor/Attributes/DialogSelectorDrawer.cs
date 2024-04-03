﻿using System.Collections.Generic;
using DialogSystem.Runtime;
using DialogSystem.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace DialogSystem.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(DialogSelectorAttribute))]
    public class DialogSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);
                //generate the taglist + custom tags
                List<string> tagList = new List<string>();
                tagList.Add("NONE");
                tagList.AddRange(SDAManager.DialogPaths);
                string propertyString = property.stringValue;
                int index = 0;
                for (int i = 0; i < tagList.Count; i++) {
                    if (tagList[i] == propertyString) {
                        index = i;
                        break;
                    }
                }
                //Draw the popup box with the current selected index
                index = EditorGUI.Popup(position, label.text, index, tagList.ToArray());
     
                //Adjust the actual string value of the property based on the selection
                property.stringValue = tagList[index];
            }
     
            EditorGUI.EndProperty();
        }
    }
}