using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if HAS_LOCALIZATION
using UnityEditor.Localization;
#endif
namespace DialogSystem.Attributes
{
    [CustomPropertyDrawer(typeof(LocalizationSelectorAttribute))]
    public class LocalizationSelectorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            #if HAS_LOCALIZATION
                if (property.propertyType == SerializedPropertyType.String)
                {
                    EditorGUI.BeginProperty(position, label, property);
            
                    var attrib = attribute as LocalizationSelectorAttribute;
                    if (attrib.UseDefaultTagFieldDrawer) {
                        property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
                    }
                    else {
                        //generate the taglist + custom tags
                        List<string> tagList = new List<string>();
                        tagList.Add("NONE");
                        var collections = LocalizationEditorSettings.GetStringTableCollections();
                        //parse to name list
                        foreach (var stringTableCollection in collections) {
                            var tableName = stringTableCollection.TableCollectionName;
                            foreach (var sharedDataEntry in stringTableCollection.SharedData.Entries) {
                                tagList.Add(tableName + "/" + sharedDataEntry.Key);
                            }
                        }
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
            #endif
        }
    }
}