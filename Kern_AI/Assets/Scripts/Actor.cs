using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    protected float moveSpeed;

    //public weapon weapon;

    protected void Move(Vector2 _dir) {
        transform.position += new Vector3(_dir.x, 0, _dir.y).normalized * moveSpeed * Time.deltaTime;
    }

    protected void LookAtTarget(Vector3 _target) {
        transform.LookAt(_target);
    }

    //Add item as parameter
    public void PickupItem() {

    }

    public void Attack() {

    }

}