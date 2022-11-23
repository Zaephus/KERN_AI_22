using System;
using System.Linq;
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

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        hasTree = tree != null;
        if(!hasTree) return;
        tree.nodes.ForEach(CreateNodeGraph);

        foreach(BehaviourNode node in tree.nodes) {

            BehaviourNodeGraph parentGraph = GetNodeByGuid(node.guid) as BehaviourNodeGraph;

            foreach(BehaviourNode child in tree.GetChildren(node)) {
                BehaviourNodeGraph childGraph = GetNodeByGuid(child.guid) as BehaviourNodeGraph;
                Edge edge = parentGraph.output.ConnectTo(childGraph.input);
                AddElement(edge);
            }

        }

    }

    private void CreateNodeGraph(BehaviourNode _node) {
        BehaviourNodeGraph nodeGraph = new BehaviourNodeGraph(_node);
        AddElement(nodeGraph);
    }

    private void CreateNode(Type _type) {

        if(!hasTree) {
            return;
        }

        BehaviourNode node = ScriptableObject.CreateInstance(_type) as BehaviourNode;
        node.name = _type.Name;
        node.guid = GUID.Generate().ToString();

        tree.nodes.Add(node);

        CreateNodeGraph(node);

        if(tree.rootNode == null) {
            tree.rootNode = node;
        }

        AssetDatabase.AddObjectToAsset(node, tree);
        AssetDatabase.SaveAssets();

    }

    private void DeleteNodeGraph(BehaviourNode _node) {

        if(!hasTree) {
            return;
        }

        tree.nodes.Remove(_node);
        if(tree.rootNode == _node) {
            tree.rootNode = null;
            if(tree.nodes.Count > 0) {
                tree.rootNode = tree.nodes[0];
            }
        }

        AssetDatabase.RemoveObjectFromAsset(_node);
        AssetDatabase.SaveAssets();

    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange _graphViewChange) {

        if(_graphViewChange.elementsToRemove != null) {
            foreach(GraphElement element in _graphViewChange.elementsToRemove) {
                BehaviourNodeGraph nodeGraph = element as BehaviourNodeGraph;
                if(nodeGraph != null) {
                    DeleteNodeGraph(nodeGraph.node);
                }

                Edge edge = element as Edge;
                if(edge != null) {
                    BehaviourNodeGraph parentGraph = edge.output.node as BehaviourNodeGraph;
                    BehaviourNodeGraph childGraph = edge.input.node as BehaviourNodeGraph;
                    tree.RemoveChild(parentGraph.node, childGraph.node);
                }
            }
        }

        if(_graphViewChange.edgesToCreate != null && hasTree) {
            foreach(Edge edge in _graphViewChange.edgesToCreate) {
                BehaviourNodeGraph parentGraph = edge.output.node as BehaviourNodeGraph;
                BehaviourNodeGraph childGraph = edge.input.node as BehaviourNodeGraph;
                tree.AddChild(parentGraph.node, childGraph.node);
            }
        }

        return _graphViewChange;

    }

    public override List<Port> GetCompatiblePorts(Port _startPort, NodeAdapter _nodeAdapter) {

        return ports.ToList()!.Where(endPort => endPort.direction != _startPort.direction &&
                                     endPort.node != _startPort.node &&
                                     endPort.portType == _startPort.portType).ToList();
        
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