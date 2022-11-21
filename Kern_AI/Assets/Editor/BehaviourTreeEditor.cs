using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
public class BehaviourTreeEditor : EditorWindow {

    private BehaviourTreeGraph treeGraph;

    [MenuItem("Window/Behaviour Tree/Editor")]
    public static void OpenTreeEditor()
    {
        GetWindow<BehaviourTreeEditor>("Behaviour Tree Editor");
    }

    public void CreateGUI()
    {
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(rootVisualElement);
        treeGraph = rootVisualElement.Q<BehaviourTreeGraph>();
    }

    private void OnSelectionChange() {

        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        if(tree != null) {
            SerializedObject so = new SerializedObject(tree);
            rootVisualElement.Bind(so);
            if(treeGraph != null) {
                treeGraph.PopulateGraph(tree);
            }
        }
        else {
            rootVisualElement.Unbind();

            TextField textField = rootVisualElement.Q<TextField>("BehaviourTreeName");
            if(textField != null) {
                textField.value = string.Empty;
            }
        }

    }
}