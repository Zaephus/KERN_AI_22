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

    public void Start() {
        moveSpeed = playerMoveSpeed;
    }

    public void Update() {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Move(new Vector2(horizontal, vertical));

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = transform.position.y;

        LookAtTarget(mousePos);

        cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, transform.position.z);

    }

}