using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Vector3 holdPositionOffset;
    public Vector3 holdRotationOffset;

    [SerializeField]
    private float attackRange;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackTime = 0.3f;

    public IEnumerator Attack(Transform _root, Hitbox _hitbox) {
        Hitbox hitbox = _hitbox;
        hitbox.transform.position = _root.position + _root.forward * attackRange;
        hitbox.damage = damage;
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackTime);
        hitbox.gameObject.SetActive(false);
    }
}
