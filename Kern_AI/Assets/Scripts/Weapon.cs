using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Vector3 holdPositionOffset;
    public Vector3 holdRotationOffset;

    [SerializeField]
    private Hitbox hitbox;

    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float attackTime = 0.3f;

    public IEnumerator Attack(Transform _root) {
        hitbox.transform.position = _root.position + _root.forward * attackRange;
        hitbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackTime);
        hitbox.gameObject.SetActive(false);
    }
}
