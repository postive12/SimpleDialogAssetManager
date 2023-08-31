using DialogSystem.Runtime.Dialogs;
using XNodeEditor;

namespace DialogSystem.Editor
{
    [CustomNodeGraphEditor(typeof(DialogPlotGraph))]
    public class DialoguePlotWindow : NodeGraphEditor
    {
        public override void OnOpen(){
            base.OnOpen();
            window.titleContent.text = target.name;
        }
        public override void OnWindowFocus()
        {
            base.OnWindowFocus();
            window.titleContent.text = target.name;
        }

    }
}