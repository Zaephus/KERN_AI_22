using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour {

    [SerializeField]
    private BehaviourTree tree;

    private void Start() {}

    private void Update() {
        tree.Update();
    }

    #if UNITY_EDITOR
    private void OnApplicationQuit() {
        foreach(BehaviourNode node in tree.nodes) {
            node.hasStarted = false;
        }
    }

    #endif

}
