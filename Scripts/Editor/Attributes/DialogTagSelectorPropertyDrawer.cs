using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace DialogSystem.Attributes
{
    [CustomPropertyDrawer(typeof(DialogTagSelectorAttribute))]
    public class DialogTagSelectorPropertyDrawer : PropertyDrawer
    {
        public static readonly List<string> CustomTags = new List<string>() {
            "====DialogCustomTag====",
            "DialogPlotSelector",
        };
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CheckTags();
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);
     
                var attrib = attribute as DialogTagSelectorAttribute;
                if (attrib.UseDefaultTagFieldDrawer) {
                    property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
                }
                else {
                    //generate the taglist + custom tags
                    List<string> tagList = new List<string>();
                    tagList.Add("NONE");
                    tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);
                    tagList.AddRange(CustomTags);
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
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
        private void CheckTags()
        {
            //Check DialogPlotSelector tag is in the unity tag list
            var tagList = UnityEditorInternal.InternalEditorUtility.tags;
            foreach (var tag in CustomTags) {
                if (tagList.Any(t => t == tag)) {
                    UnityEditorInternal.InternalEditorUtility.RemoveTag(tag);
                    Debug.LogError("You should not \"DialogPlotSelector\" as a tag, it is used by DialogPlotSelector.");
                    Debug.LogError("Automatically remove \"DialogPlotSelector\" tag from tag list.");
                }
            }
        }
    }
}