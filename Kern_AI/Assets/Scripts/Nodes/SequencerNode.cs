using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositeNode {

    private int current;

    protected override void OnStart() {
        current = 0;
    }

    protected override NodeState Evaluate() {

        if(GetChildren() == null && GetChildren().Count < 1) {
            Debug.LogWarning("Sequencer Node has no children.");
            return NodeState.Failure;
        }

        switch(GetChildren()[current]!.Update()) {

            case NodeState.Running:
                return NodeState.Running;

            case NodeState.Succes:
                current++;
                break;
            
            case NodeState.Failure:
                return NodeState.Failure;

            default:
                return NodeState.Failure;

        }

        if(current == GetChildren().Count) {
            return NodeState.Succes;
        }
        else {
            return NodeState.Running;
        }

    }

    protected override void OnEnd() {
        
    }
    
}