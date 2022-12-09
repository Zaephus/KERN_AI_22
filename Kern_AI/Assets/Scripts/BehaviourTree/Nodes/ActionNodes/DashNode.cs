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
                agent.Dash(new Vector2(agent.transform.right.x, agent.transform.right.z));
                break;
            
            case "Left":
                agent.Dash(new Vector2(-agent.transform.right.x, -agent.transform.right.z));
                break;

            case "Back":
                agent.Dash(new Vector2(-agent.transform.forward.x, -agent.transform.forward.z));
                break;
        }

        return NodeState.Succes;

    }

    protected override void OnEnd() {}
    
}