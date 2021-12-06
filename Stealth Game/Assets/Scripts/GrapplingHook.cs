using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private SpringJoint2D grappling_hook;
    public LineRenderer rope_render;
    private Vector3 grapple_point;
    public Transform player;

    void Awake()
    {
        rope_render = GetComponent<LineRenderer>();
        grappling_hook = transform.gameObject.AddComponent<SpringJoint2D>();
        grappling_hook.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrappling();
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopGrappling();
        }
    }

    void StartGrappling()
    {
        Vector3 point1 = player.position;
        Vector3 point2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Linecast(point1, point2);

        if (hit.collider.tag == "Hook")
        {
            grapple_point = hit.transform.position;
            grappling_hook.enabled = true;
            grappling_hook.connectedAnchor = grapple_point;
            grappling_hook.autoConfigureConnectedAnchor = false;
            float distanceBetweenPoint = Vector3.Distance(transform.position, grapple_point);
            grappling_hook.distance = distanceBetweenPoint * Time.deltaTime;
            grappling_hook.dampingRatio = 7f;
            rope_render.positionCount = 2;
        }
    }

    void StopGrappling()
    {
        grappling_hook.enabled = false;
        rope_render.positionCount = 0;
    }

    void LateUpdate()
    {
        Draw();
    }

    void Draw()
    {
        if (grappling_hook.isActiveAndEnabled)
        {
            if (rope_render.positionCount > 0)
            {
                rope_render.SetPosition(0, transform.position);
                rope_render.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
}
