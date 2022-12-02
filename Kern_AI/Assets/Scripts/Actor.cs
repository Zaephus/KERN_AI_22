using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    public int health;

    public Hitbox hitbox;

    protected float moveSpeed;

    protected Action pickedUpItem;

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
            foreach(Collider c in Physics.OverlapSphere(transform.position, 1)) {
                if(c.GetComponent<Weapon>() != null) {
                    Weapon w = c.GetComponent<Weapon>();
                    w.transform.SetParent(holdTransform);
                    w.transform.localPosition = w.holdPositionOffset;
                    w.transform.localRotation = Quaternion.Euler(w.holdRotationOffset);
                    weapon = w;
                    pickedUpItem?.Invoke();
                    break;
                }
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

    public void TakeDamage(int _dmg) {
        health -= _dmg;

        if(health <= 0) {
            Die();
        }

    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

}