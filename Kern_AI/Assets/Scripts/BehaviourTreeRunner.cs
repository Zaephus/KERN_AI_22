using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour {

    [SerializeField]
    private BehaviourTree tree;

    private void Start() {
        tree = ScriptableObject.CreateInstance<BehaviourTree>();

        DebugLogNode log1 = ScriptableObject.CreateInstance<DebugLogNode>();
        log1.message  = "Testing 1.";
        DebugLogNode log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        log2.message  = "Testing 2.";
        DebugLogNode log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        log3.message  = "Testing 3.";

        WaitNode wait1 = ScriptableObject.CreateInstance<WaitNode>();
        wait1.duration = 1;
        WaitNode wait2 = ScriptableObject.CreateInstance<WaitNode>();
        wait2.duration = 1;
        WaitNode wait3 = ScriptableObject.CreateInstance<WaitNode>();
        wait3.duration = 1;

        CompositeNode sequence = ScriptableObject.CreateInstance<SequencerNode>();
        sequence.children.Add(log1);
        sequence.children.Add(wait1);
        sequence.children.Add(log2);
        sequence.children.Add(wait2);
        sequence.children.Add(log3);
        sequence.children.Add(wait3);

        DecoratorNode loop = ScriptableObject.CreateInstance<RepeaterNode>();
        loop.child = sequence;
        tree.rootNode = loop;
    }

    private void Update() {
        tree.Update();
    }

}
