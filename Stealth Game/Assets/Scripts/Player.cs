using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    public float walk_speed = 5f;
    public float sprint_speed = 8f;
    public float move_speed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public float rotationOffset;
    public bool isMoving;

    void Start()
    {
        move_speed = walk_speed; //Set it to walk from the start
    }

    void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        if (isMoving)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            //movement.y = Input.GetAxisRaw("Vertical");

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + rotationOffset));
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * move_speed * Time.fixedDeltaTime);
    }
}
