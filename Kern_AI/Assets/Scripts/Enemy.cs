using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {

    [SerializeField]
    private float enemyMoveSpeed = 10f;

    [SerializeField]
    private BehaviourTree tree;

    private void Start() {

        moveSpeed = enemyMoveSpeed;

        foreach(BehaviourNode node in tree.nodes) {
            node.hasStarted = false;
        }
        tree.treeState = NodeState.Running;

        tree.blackboard.SetValue<Enemy>("Agent", this);
        tree.blackboard.SetValue<bool>("HasWeapon", false);

        pickedUpItem += OnItemPickup;
        
    }

    private void Update() {
        tree.Update();
    }

    public void MoveEnemy(Vector3 _dir, Vector3 _target) {
        Move(new Vector2(_dir.x, _dir.z));
        LookAtTarget(_target);
    }

    private void OnItemPickup() {
        tree.blackboard.SetValue<bool>("HasWeapon", true);
    }
}
