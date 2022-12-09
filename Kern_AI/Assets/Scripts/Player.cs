using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {

    [SerializeField]
    private Vector3 cameraOffset;

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

        Move(new Vector2(vertical, -horizontal));

        Vector3 mousePos = new Vector3();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
            mousePos = hit.point;
        }
        mousePos.y = transform.position.y;

        LookAtTarget(mousePos);

        cam.transform.position = new Vector3(transform.position.x + cameraOffset.x,
                                             transform.position.y + cameraOffset.y,
                                             transform.position.z + cameraOffset.z);

        if(Input.GetButtonDown("Interact")) {
            PickupItem();
        }

        if(Input.GetButtonDown("Attack")) {
            Attack();
        }

    }

}