using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : MonoBehaviour
{
    public float spinSpeed;
    public float distance;

    public LineRenderer lineOfSight;
    public Gradient detectColor;
    public Gradient scanColor;

    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);

        RaycastHit2D hitInformation = Physics2D.Raycast(transform.position, transform.right, distance);
        if (hitInformation.collider != null) //checks if it hit something
        {
            lineOfSight.SetPosition(1, hitInformation.point);
            lineOfSight.colorGradient = detectColor;

            if (hitInformation.collider.CompareTag("Player")) //player makes contact with the sentry laser
            {
                Destroy(hitInformation.collider.gameObject);
            }
        }
        else
        {
            lineOfSight.SetPosition(1, transform.position + transform.right * distance);
            lineOfSight.colorGradient = scanColor;
        }

        lineOfSight.SetPosition(0, transform.position);
    }
}
