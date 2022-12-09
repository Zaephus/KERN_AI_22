using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashNode : ActionNode {

    [SerializeField]
    private string dir;

    private Enemy agent;

    protected override void OnStart() {
        agent = tree.blackboard.GetValue<Enemy>("Agent");
    }

    protected override NodeState Evaluate() {

        switch(dir) {
            case "Right":
                agent.Dash(agent.transform.right);
                break;
            
            case "Left":
                agent.Dash(-agent.transform.right);
                break;

            case "Back":
                agent.Dash(-agent.transform.forward);
                break;
        }

        return NodeState.Succes;

    }

    protected override void OnEnd() {}
    
}