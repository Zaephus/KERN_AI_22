using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[CreateAssetMenu(fileName = "BehaviourTree", menuName = "Behaviour Tree/Behaviour Tree")]
public class BehaviourTree : ScriptableObject {

    public BehaviourNode rootNode;
    public List<BehaviourNode> nodes = new List<BehaviourNode>();
    public NodeState treeState = NodeState.Running;

    public Blackboard blackboard;

    public void Initialize() {

        if(blackboard == null) {
            Debug.LogWarning("No blackboard is not assigned");
        }
        else {
            blackboard.SetValue<int>("Test Int", 14);
            blackboard.SetValue<string>("Test String", "Hoi");
            blackboard.SetValue<Vector3>("Test Position", new Vector3(1, 3, 4));
        }

    }

    public NodeState Update() {

        if(rootNode == null) {
            Debug.LogWarning($"{name} needs a root node in order to properly run. Please add one.", this);
        }

        if(rootNode != null) {
            if(treeState == NodeState.Running) {
                treeState = rootNode.Update();
            }
        }
        else {
            treeState = NodeState.Failure;
        }

        return treeState;

    }

    public void AddChild(BehaviourNode _parent, BehaviourNode _child) {

        if(!nodes.Contains(_parent)) {
            return;
        }

        nodes[nodes.IndexOf(_parent)].AddChild(_child);

        if(nodes.Contains(_child)) {
            return;
        }

        nodes.Add(_child);

    }

    public void RemoveChild(BehaviourNode _parent, BehaviourNode _child) {

        if(!nodes.Contains(_parent)) {
            return;
        }

        nodes[nodes.IndexOf(_parent)].RemoveChild(_child);

    }

    public List<BehaviourNode> GetChildren(BehaviourNode _parent) {
        return !nodes.Contains(_parent) ? new List<BehaviourNode>() : nodes[nodes.IndexOf(_parent)].GetChildren();
    }

    public BehaviourNode GetNodeByGUID(string _guid) {
        foreach(BehaviourNode node in nodes) {
            if(node.guid == _guid) {
                return node;
            }
        }
        return null;
    }

}
