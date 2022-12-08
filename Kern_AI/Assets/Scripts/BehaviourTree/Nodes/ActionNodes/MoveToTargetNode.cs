using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetNode : ActionNode {

    public float stoppingDistance;

    private GameObject currentTarget;
    private Enemy agent;

    protected override void OnStart() {
        currentTarget = tree.blackboard.GetValue<GameObject>("CurrentTarget");
        agent = tree.blackboard.GetValue<Enemy>("Agent");
    }

    protected override NodeState Evaluate() {

        if(Vector3.Distance(agent.transform.position, currentTarget.transform.position) <= stoppingDistance) {
            return NodeState.Succes;
        }
        Vector3 moveVector = currentTarget.transform.position - agent.transform.position;
        agent.MoveEnemy(moveVector.normalized, currentTarget.transform.position);

        return NodeState.Running;

    }

    protected override void OnEnd() {}

}