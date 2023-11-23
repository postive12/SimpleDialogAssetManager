using System;
using System.Collections.Generic;
using DialogSystem.Attributes;
using UnityEngine;

#if HAS_LOCALIZATION
using UnityEngine.Localization;
using UnityEditor.Localization;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#endif

namespace DialogSystem.Structure
{
    [Serializable]
    public class DialogContent
    {
        public string Content {
            get {
                #if HAS_LOCALIZATION
                    //If engine has localization, load from string reference
                    if (_stringReference == "NONE") {
                        return "";
                    }
                    var keys = _stringReference.Split('/');
                    var localizedString = new LocalizedString();
                    localizedString.TableReference = keys[0];
                    localizedString.TableEntryReference = keys[1];
                    return localizedString.GetLocalizedString();
                #else
                    //If engine does not have localization, load from content
                    return _content;
                #endif

            }
        }
        //If engine has localization add string reference and preload function
        #if HAS_LOCALIZATION
            #if ODIN_INSPECTOR
                [LocalizationSelector]
                [ValueDropdown("GetLocalizationList",IsUniqueList = false,DrawDropdownForListElements = true)]
                [SerializeField]
                private string _stringReference = "NONE";
                private static IEnumerable<string> GetLocalizationList() {
                    var list = new List<string>();
                    list.Add("NONE");
                    var collections = LocalizationEditorSettings.GetStringTableCollections();
                    //parse to name list
                    foreach (var stringTableCollection in collections) {
                        var tableName = stringTableCollection.TableCollectionName;
                        foreach (var sharedDataEntry in stringTableCollection.SharedData.Entries) {
                            list.Add(tableName + "/" + sharedDataEntry.Key);
                        }
                    }
                    return list;
                }
            #else
                [LocalizationSelector]
                [SerializeField]
                private string _stringReference = "NONE";
            #endif
        #else
            [SerializeField][TextArea] private string _content;
        #endif
    }
}