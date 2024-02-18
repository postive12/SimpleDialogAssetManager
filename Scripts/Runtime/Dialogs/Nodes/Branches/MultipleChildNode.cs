using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace DialogSystem.Nodes.Branches
{
    public abstract class MultipleChildNode : DialogBaseNode
    {
        [HideInInspector] public List<DialogBaseNode> Children = new List<DialogBaseNode>();
    }
}