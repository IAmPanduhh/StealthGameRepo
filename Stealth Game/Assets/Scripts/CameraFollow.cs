using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float followSpeed;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, followSpeed);
    }
}
