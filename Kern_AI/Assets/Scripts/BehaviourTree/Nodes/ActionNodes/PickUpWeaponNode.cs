using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeaponNode : ActionNode {

    private GameObject currentTarget;
    private Enemy agent;

    protected override void OnStart() {
        currentTarget = tree.blackboard.GetValue<GameObject>("CurrentTarget");
        agent = tree.blackboard.GetValue<Enemy>("Agent");
    }

    protected override NodeState Evaluate() {

        if(currentTarget.GetComponent<Weapon>() != null) {
            agent.PickupItem();
            return NodeState.Succes;
        }

        return NodeState.Failure;

    }

    protected override void OnEnd() {}
    
}
