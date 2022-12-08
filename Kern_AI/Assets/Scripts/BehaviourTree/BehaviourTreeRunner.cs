using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BehaviourTreeRunner : MonoBehaviour {

    public int testint = 10;

    [SerializeField]
    private BehaviourTree tree;

    private void Start() {
        tree.treeState = NodeState.Running;
    }

    private void Update() {
        //tree.blackboard.SetValue<int>("Test Int", testint);
        tree.Update();
    }

    #if UNITY_EDITOR
    private void OnApplicationQuit() {
        foreach(BehaviourNode node in tree.nodes) {
            node.hasStarted = false;
            tree.treeState = NodeState.Running;
        }
    }

    #endif

}