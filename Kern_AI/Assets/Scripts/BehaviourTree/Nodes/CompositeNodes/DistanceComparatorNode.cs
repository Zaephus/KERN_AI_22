using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceComparatorNode : CompositeNode {

    [SerializeField]
    private string objectOnePath;
    [SerializeField]
    private string objectTwoPath;

    private GameObject objectOne;
    private GameObject objectTwo;

    private Enemy agent;

    protected override void OnStart() {
        objectOne = tree.blackboard.GetValue<GameObject>(objectOnePath);
        objectTwo = tree.blackboard.GetValue<GameObject>(objectTwoPath);

        agent = tree.blackboard.GetValue<Enemy>("Agent");
    }

    protected override NodeState Evaluate() {

        if(objectOne == null || objectTwo == null) {
            return NodeState.Failure;
        }

        float distOne = Vector3.Distance(agent.transform.position, objectOne.transform.position);
        float distTwo = Vector3.Distance(agent.transform.position, objectTwo.transform.position);

        if(distOne > distTwo) {
            tree.blackboard.SetValue<GameObject>("CurrentTarget", objectTwo);
            return GetChildren()[1]!.Update();
        }
        else {
            tree.blackboard.SetValue<GameObject>("CurrentTarget", objectOne);
            return GetChildren()[0]!.Update();
        }

    }

    protected override void OnEnd() {}

}
