using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BehaviourTree", menuName = "Behaviour Tree")]
public class BehaviourTree : ScriptableObject {

    public BehaviourNode rootNode;
    public List<BehaviourNode> nodes = new List<BehaviourNode>();
    public NodeState treeState = NodeState.Running;

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

}
