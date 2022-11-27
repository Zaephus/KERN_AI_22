using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class BehaviourTreeGraph : GraphView {

    public new class UxmlFactory : UxmlFactory<BehaviourTreeGraph, GraphView.UxmlTraits> {}

    public BlackboardElement blackboardElement;

    private BehaviourTree tree;
    private bool hasTree;

    public BehaviourTreeGraph() {

        style.flexGrow = 1;
        Insert(0, new GridBackground());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

    }

    public void PopulateGraph(BehaviourTree _tree) {

        tree = _tree;
        tree.Initialize();

        blackboardElement.PopulateElement(tree.blackboard, this);

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        hasTree = tree != null;
        if(!hasTree) return;

        foreach(BehaviourNode node in tree.nodes) {
            CreateNodeGraph(node);
        }

        foreach(BlackboardNode node in tree.blackboard.nodes) {
            blackboardElement.CreateNodeGraph(node);
        }

        if(tree.nodes.Count > 0) {

            BehaviourNodeGraph rootNodeGraph = GetNodeByGuid(tree.rootNode.guid) as BehaviourNodeGraph;
            rootNodeGraph.input.SetEnabled(false);
            rootNodeGraph.inputContainer.Remove(rootNodeGraph.input);

            foreach(BehaviourNode node in tree.nodes) {

                BehaviourNodeGraph parentGraph = GetNodeByGuid(node.guid) as BehaviourNodeGraph;

                if(tree.GetChildren(node) != null) {
                    foreach(BehaviourNode child in tree?.GetChildren(node)) {
                        BehaviourNodeGraph childGraph = GetNodeByGuid(child.guid) as BehaviourNodeGraph;
                        Edge edge = parentGraph.output.ConnectTo(childGraph.input);
                        AddElement(edge);
                    }
                }

            }

        }
        
    }

    private BehaviourNodeGraph CreateNodeGraph(BehaviourNode _node) {
        BehaviourNodeGraph nodeGraph = new BehaviourNodeGraph(_node);
        AddElement(nodeGraph);
        return nodeGraph;
    }

    private void CreateNode(Type _type, Vector2 _nodePos) {

        if(!hasTree) {
            return;
        }

        BehaviourNode node = ScriptableObject.CreateInstance(_type) as BehaviourNode;
        node.name = _type.Name;
        node.guid = GUID.Generate().ToString();
        node.nodeGraphPosition = _nodePos;

        tree.nodes.Add(node);

        BehaviourNodeGraph nodeGraph = CreateNodeGraph(node);

        if(tree.rootNode == null) {
            tree.rootNode = node;
            nodeGraph.input.SetEnabled(false);
            nodeGraph.inputContainer.Remove(nodeGraph.input);
        }

        AssetDatabase.AddObjectToAsset(node, tree);
        AssetDatabase.SaveAssets();

    }

    private void DeleteNodeGraph(NodeGraph _node) {

        if(_node.behaviourNodeGraph != null) {

            BehaviourNode node = _node.behaviourNodeGraph.node;

                if(!hasTree) {
                return;
            }

            tree.nodes.Remove(node);
            if(tree.rootNode == node) {
                tree.rootNode = null;
                if(tree.nodes.Count > 0) {
                    SetRootNode(tree.nodes[0]);
                }
            }

            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
        else if(_node.blackboardNodeGraph != null) {  

            BlackboardNode node = _node.blackboardNodeGraph.node;
            tree.blackboard.nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();

        }

    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange _graphViewChange) {

        if(_graphViewChange.elementsToRemove != null) {
            foreach(GraphElement element in _graphViewChange.elementsToRemove) {
                
                NodeGraph nodeGraph = element as NodeGraph;
                if(nodeGraph != null) {
                    DeleteNodeGraph(nodeGraph);
                }

                Edge edge = element as Edge;
                if(edge != null) {
                    NodeGraph parentGraph = edge.output.node as NodeGraph;
                    NodeGraph childGraph = edge.input.node as NodeGraph;
                    if(parentGraph.behaviourNodeGraph != null && childGraph.behaviourNodeGraph != null) {
                        tree.RemoveChild(parentGraph.behaviourNodeGraph.node, childGraph.behaviourNodeGraph.node);
                    }
                }
            }
        }

        if(_graphViewChange.edgesToCreate != null && hasTree) {
            foreach(Edge edge in _graphViewChange.edgesToCreate) {
                NodeGraph parentGraph = edge.output.node as NodeGraph;
                NodeGraph childGraph = edge.input.node as NodeGraph;
                if(parentGraph.behaviourNodeGraph != null && childGraph.behaviourNodeGraph != null) {
                    tree.AddChild(parentGraph.behaviourNodeGraph.node, childGraph.behaviourNodeGraph.node);
                }
            }
        }

        return _graphViewChange;

    }

    private void SetRootNode(BehaviourNode _node) {

        BehaviourNodeGraph rootNodeGraph = GetNodeByGuid(tree.rootNode.guid) as BehaviourNodeGraph;
        rootNodeGraph.input.SetEnabled(true);
        rootNodeGraph.inputContainer.Add(rootNodeGraph.input);

        if(_node != null) {

            BehaviourNodeGraph nodeGraph = GetNodeByGuid(_node.guid) as BehaviourNodeGraph;

            foreach(Edge edge in nodeGraph.input.connections) {
                BehaviourNodeGraph parentGraph = edge.output.node as BehaviourNodeGraph;
                tree.RemoveChild(parentGraph.node, nodeGraph.node);
                parentGraph.output.Disconnect(edge);
            }

            DeleteElements(nodeGraph.input.connections);
            nodeGraph.input.DisconnectAll();
            nodeGraph.input.SetEnabled(false);
            nodeGraph.inputContainer.Remove(nodeGraph.input);
            tree.rootNode = _node;

        }
        else {
            Debug.LogWarning("Node was null.");
        }

    }

    public override List<Port> GetCompatiblePorts(Port _startPort, NodeAdapter _nodeAdapter) {
        return ports.ToList()!.Where(endPort => endPort.direction != _startPort.direction &&
                                     endPort.node != _startPort.node &&
                                     endPort.portType == _startPort.portType).ToList();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent _evt) {

        TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<BehaviourNode>();

        VisualElement contentViewContainer = ElementAt(1);
        Vector3 screenMousePosition = _evt.localMousePosition;
        Vector2 worldMousePosition = screenMousePosition - contentViewContainer.transform.position;
        worldMousePosition *= 1 / contentViewContainer.transform.scale.x;

        foreach(Type type in types) {

            if(type.IsAbstract) {
                continue;
            }

            if(_evt.target is Node) {
                Node node = _evt.target as Node;
                _evt.menu.AppendAction("Make root node", _ => SetRootNode(tree.GetNodeByGUID(node.viewDataKey)));
            }

            _evt.menu.AppendAction($"{type.BaseType.Name}/{type.Name}", _ => CreateNode(type, worldMousePosition));

        }

        base.BuildContextualMenu(_evt);

    }

}