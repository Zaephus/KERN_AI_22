using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {

    [SerializeField]
    private float playerMoveSpeed = 10f;

    [SerializeField]
    private Camera cam;

    private float horizontal;
    private float vertical;

    public GenericObject<Transform> test;

    public void Start() {

        test = new GenericObject<Transform>(transform);
        moveSpeed = playerMoveSpeed;

        Debug.Log(test.Data);
        Debug.Log(test.GetType());
        Debug.Log(test.GetObjectType());
    }

    public void Update() {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Move(new Vector2(horizontal, vertical));

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = transform.position.y;

        LookAtTarget(mousePos);

        cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, transform.position.z);

        if(Input.GetButtonDown("Interact")) {
            PickupItem();
        }

        if(Input.GetButtonDown("Attack")) {
            Attack();
        }

    }

}