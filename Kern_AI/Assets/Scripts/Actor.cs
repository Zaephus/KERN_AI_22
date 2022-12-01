using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    protected float moveSpeed;

    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private Transform holdTransform;

    protected void Move(Vector2 _dir) {
        transform.position += new Vector3(_dir.x, 0, _dir.y).normalized * moveSpeed * Time.deltaTime;
    }

    protected void LookAtTarget(Vector3 _target) {
        transform.LookAt(_target);
    }

    public void PickupItem() {
        if(weapon == null) {
            RaycastHit hit;
            if(Physics.SphereCast(transform.position, transform.localScale.y, transform.forward, out hit)) {
                if(hit.collider.GetComponent<Weapon>() != null) {
                    Weapon w = hit.collider.GetComponent<Weapon>();
                    //w.transform.parent = holdTransform;
                    w.transform.SetParent(holdTransform);
                    w.transform.localPosition = w.holdPositionOffset;
                    w.transform.localRotation = Quaternion.Euler(w.holdRotationOffset);
                    weapon = w;
                }
            }
            else {
                Debug.LogWarning("No weapon in range");
            }
        }
    }

    public void Attack() {
        if(weapon != null) {
            weapon.StartCoroutine(weapon.Attack(transform));
        }
        else {
            Debug.LogWarning("Actor has no weapon");
        }
    }

}