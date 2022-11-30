using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    protected float moveSpeed;

    //public weapon weapon;

    public void Move(Vector2 _dir) {
        transform.position += new Vector3(_dir.x, 0, _dir.y).normalized * moveSpeed * Time.deltaTime;
    }

    //Add item as parameter
    public void PickupItem() {

    }

    public void Attack() {

    }

}