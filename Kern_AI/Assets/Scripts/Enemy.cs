using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : Actor {

    [SerializeField]
    private float enemyMoveSpeed = 10f;

    [SerializeField]
    private BehaviourTree tree;

    [SerializeField]
    private Canvas stateCanvas;
    [SerializeField]
    private Vector3 stateCanvasOffset;

    private void Start() {

        moveSpeed = enemyMoveSpeed;

        foreach(BehaviourNode node in tree.nodes) {
            node.hasStarted = false;
        }
        tree.treeState = NodeState.Running;

        tree.blackboard.SetValue<Enemy>("Agent", this);
        tree.blackboard.SetValue<bool>("HasWeapon", false);
        tree.blackboard.SetValue<string>("CurrentState", "Initializing");

        pickedUpItem += OnItemPickup;
        
    }

    private void Update() {
        stateCanvas.transform.position = new Vector3(transform.position.x + stateCanvasOffset.x,
                                                     transform.position.y + stateCanvasOffset.y,
                                                     transform.position.z + stateCanvasOffset.z);
        stateCanvas.GetComponentInChildren<TMP_Text>().text = tree.blackboard.GetValue<string>("CurrentState");
        tree.Update();
    }

    public void MoveEnemy(Vector3 _dir, Vector3 _target) {
        Move(new Vector2(_dir.x, _dir.z));
        LookAtTarget(_target);
    }

    private void OnItemPickup() {
        tree.blackboard.SetValue<bool>("HasWeapon", true);
    }

    protected override void Die() {
        stateCanvas.enabled = false;
        base.Die();
    }
}
