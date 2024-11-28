using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private CharacterController controller;

    private Vector3 MoveDirection;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        MoveDirection = new Vector3(horizontal, 0.0f, vertical).normalized;

        if(MoveDirection.magnitude > 0.1f)
        {
            transform.forward = MoveDirection;
            controller.Move(MoveDirection * moveSpeed * Time.deltaTime);
        }
    }


}
