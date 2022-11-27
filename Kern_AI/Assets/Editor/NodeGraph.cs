using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public abstract class NodeGraph : Node {

    public BehaviourNodeGraph behaviourNodeGraph = null;
    public BlackboardNodeGraph blackboardNodeGraph = null;

}