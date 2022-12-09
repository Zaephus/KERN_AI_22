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
    private float dashDist;

    [SerializeField]
    private float punchAttackRange;
    [SerializeField]
    private int punchDamage;
    [SerializeField]
    private float punchAttackTime;

    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private Transform holdTransform;

    public void LookAtTarget(Vector3 _target) {
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
                    w.tag = "PickedUpWeapon";
                    weapon = w;
                    pickedUpItem?.Invoke();
                    break;
                }
            }
        }
    }

    public void Attack() {
        if(weapon != null) {
            weapon.StartCoroutine(weapon.Attack(transform, hitbox));
        }
        else {
            StartCoroutine(Punch());
        }
    }

    public void TakeDamage(int _dmg) {
        health -= _dmg;

        if(health <= 0) {
            Die();
        }

    }

    public void Dash(Vector2 _dir) {
        transform.position += new Vector3(_dir.x, 0, _dir.y).normalized * dashDist;
    }

    protected void Move(Vector2 _dir) {
        transform.position += new Vector3(_dir.x, 0, _dir.y).normalized * moveSpeed * Time.deltaTime;
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

    private IEnumerator Punch() {
        hitbox.transform.position = transform.position + transform.forward * punchAttackRange;
        hitbox.damage = punchDamage;
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(punchAttackTime);
        hitbox.gameObject.SetActive(false);
    }

}