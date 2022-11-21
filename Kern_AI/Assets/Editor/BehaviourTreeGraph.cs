using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class BehaviourTreeGraph : GraphView {

    public new class UxmlFactory : UxmlFactory<BehaviourTreeGraph, GraphView.UxmlTraits> {}

    private BehaviourTree tree;
    private bool hasTree;

    public BehaviourTreeGraph() {

        style.flexGrow = 1;
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

    }

    public void PopulateGraph(BehaviourTree _tree) {
        tree = _tree;
        DeleteElements(graphElements);
        hasTree = tree != null;
        if(!hasTree) return;
        tree.nodes.ForEach(CreateNodeGraph);
    }

    private void CreateNode(Type _type) {

        if(!hasTree) {
            return;
        }

        BehaviourNode node = ScriptableObject.CreateInstance(_type) as BehaviourNode;
        node.name = _type.Name;

        tree.nodes.Add(node);

        CreateNodeGraph(node);

        if(tree.rootNode == null) {
            tree.rootNode = node;
        }

        AssetDatabase.AddObjectToAsset(node, tree);
        AssetDatabase.SaveAssets();

    }

    private void CreateNodeGraph(BehaviourNode _node) {
        BehaviourNodeGraph nodeGraph = new BehaviourNodeGraph(_node);
        AddElement(nodeGraph);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent _evt) {

        TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<BehaviourNode>();

        foreach(Type type in types) {

            if(type.IsAbstract) {
                continue;
            }

            _evt.menu.AppendAction($"{type.BaseType.Name}/{type.Name}", _ => CreateNode(type));

        }

        base.BuildContextualMenu(_evt);

    }

}