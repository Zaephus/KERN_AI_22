using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[System.Serializable]
public class BehaviourTreeEditor : EditorWindow {

    private BehaviourTree tree;
    private BehaviourTreeGraph treeGraph;

    [MenuItem("Window/Behaviour Tree/Editor")]
    public static void OpenTreeEditor() {
        GetWindow<BehaviourTreeEditor>("Behaviour Tree Editor");
    }

    public void CreateGUI() {

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(rootVisualElement);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        rootVisualElement.styleSheets.Add(styleSheet);
        
        treeGraph = rootVisualElement.Q<BehaviourTreeGraph>();
        
        if(tree != null) {
            PopulateWindow(tree);
        }

    }

    public void PopulateWindow(BehaviourTree _tree) {

        tree = _tree;

        SerializedObject so = new SerializedObject(tree);
        rootVisualElement.Bind(so);
        if(treeGraph != null) {
            treeGraph.PopulateGraph(tree);
        }

    }

    [OnOpenAsset]
    public static bool OpenBehaviourTreeWindow(int _instanceID) {
        
        BehaviourTree staticTree = EditorUtility.InstanceIDToObject(_instanceID) as BehaviourTree;

        if(staticTree is not BehaviourTree) {
            return false;
        }

        foreach(BehaviourTreeEditor w in Resources.FindObjectsOfTypeAll<BehaviourTreeEditor>()) {
            if(w.tree == staticTree) {
                w.Focus();
                w.PopulateWindow(staticTree);
                return false;
            }
        }

        BehaviourTreeEditor window = EditorWindow.CreateWindow<BehaviourTreeEditor>(staticTree.name, typeof(SceneView));
        window.PopulateWindow(staticTree);

        return false;

    }

}