using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace DialogSystem.Nodes.Lines
{
    public abstract class SingleChildNode : DialogBaseNode
    {
        [HideInInspector] public DialogBaseNode Child = null;
    }
}