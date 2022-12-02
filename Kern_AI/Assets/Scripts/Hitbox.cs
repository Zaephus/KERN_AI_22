using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    [HideInInspector]
    public int damage;

    public void OnTriggerEnter(Collider _other) {

        if(_other.GetComponent<Actor>() != null) {
            _other.GetComponent<Actor>().TakeDamage(damage);
        }
    }

}
