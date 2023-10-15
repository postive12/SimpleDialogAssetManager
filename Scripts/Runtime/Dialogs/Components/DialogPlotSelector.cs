﻿using System;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Dialogs.Interfaces;
using UnityEngine;

namespace DialogSystem.Dialogs.Components
{
    public class DialogPlotSelector : DialogTargetComponent, IEventInvoker
    {
        private const string INVOKER_TAG = "DialogPlotSelector";
        public void Invoke(string eventName) {
            DialogManager.Instance.SelectDialogPlot(eventName);
        }
        private void OnValidate() {
            _targetTag = INVOKER_TAG;
        }

    }
}