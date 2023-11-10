using System;
using DialogSystem.Attributes;
using UnityEngine;

#if HAS_LOCALIZATION
using UnityEngine.Localization;
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
            [LocalizationSelector] public string _stringReference = "NONE";
        #else
            [SerializeField][TextArea] private string _content;
        #endif
    }
}