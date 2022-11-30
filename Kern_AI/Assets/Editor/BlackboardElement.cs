using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

public class BlackboardElement : VisualElement {

    public new class UxmlFactory : UxmlFactory<BlackboardElement, VisualElement.UxmlTraits> {}

    private Blackboard blackboard;

    private BehaviourTreeGraph treeGraph;

    public BlackboardElement() {
        style.flexGrow = 1;
    }

    public void PopulateElement(Blackboard _blackboard, BehaviourTreeGraph _treeGraph) {
        
        blackboard = _blackboard;
        treeGraph = _treeGraph;

        if(blackboard == null) {
            return;
        }

        Clear();

        ScrollView scrollView = new ScrollView();
        contentContainer.Add(scrollView);

        foreach(BlackboardField field in blackboard.fields) {
            field.OnClick += CreateNode;
            scrollView.Add(field.CreateBlackboardElement());
        }

    }

    public void CreateNodeGraph(BlackboardNode _node) {
        BlackboardNodeGraph blackboardNodeGraph = new BlackboardNodeGraph(_node);
        treeGraph.contentViewContainer.Add(blackboardNodeGraph);
    }

    private void CreateNode(string _name, BlackboardField _field) {

        BlackboardNode node = ScriptableObject.CreateInstance<BlackboardNode>();
        node.name = "Blackboard " + _name;
        node.field = _field;
        node.guid = GUID.Generate().ToString();

        blackboard.nodes.Add(node);

        CreateNodeGraph(node);

        AssetDatabase.AddObjectToAsset(node, treeGraph.tree);
        AssetDatabase.SaveAssets();

    }

}