using System;
using DialogSystem.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Dialogs.Interfaces;
using UnityEngine;

namespace DialogSystem.Dialogs.Components
{
    public class DialogTargetComponent : MonoBehaviour,IDialogTarget
    {
        string IDialogTarget.TargetTag {
            get {
                return _targetTag;
            }
            set {
                _targetTag = value;
            }
        }
        [DialogTagSelector][SerializeField] protected string _targetTag = "NONE";
        [SerializeField] protected bool _useDefaultDialogManager = true;
        protected virtual void Awake() {
            if (_useDefaultDialogManager) {
                DialogManager.AddDialogTarget(this);
            }
        }
        public string GetTargetTag() => _targetTag;
    }
}