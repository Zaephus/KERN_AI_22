using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode {

    private int current;

    protected override void OnStart() {
        current = 0;
    }

    protected override NodeState Evaluate() {

        if(children == null && children.Count < 1) {
            Debug.LogWarning("Selector Node has no children.");
            return NodeState.Failure;
        }

        switch(children[current]!.Update()) {

            case NodeState.Running:
                return NodeState.Running;

            case NodeState.Succes:
                return NodeState.Succes;

            case NodeState.Failure:
                current++;
                break;

            default:
                return NodeState.Running;

        }

        if(current == children.Count) {
            return NodeState.Failure;
        }
        else {
            return NodeState.Running;
        }

    }

    protected override void OnEnd() {

    }
    
}
