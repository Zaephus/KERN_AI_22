using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {

    [SerializeField]
    private float playerMoveSpeed = 10f;

    private float horizontal;
    private float vertical;

    public void Start() {
        moveSpeed = playerMoveSpeed;
    }

    public void Update() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Move(new Vector2(horizontal, vertical));
    }

}