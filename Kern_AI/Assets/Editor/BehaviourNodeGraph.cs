using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourNodeGraph : UnityEditor.Experimental.GraphView.Node {

    private BehaviourNode node;

    public BehaviourNodeGraph(BehaviourNode _node) {
        node = _node;
        if(node == null) {
            return;
        }
        base.title = node.name;
    }
    
}
