using UnityEngine;
 

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public AreaBounds area;

    public float smoothTime = 0.5f;

    float velocityX;
    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            Debug.LogWarning("CameraFollow: this camera is set to Perspective. The edge limits assume Orthographic. Change Projection on the Camera component.");
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.SmoothDamp(pos.x, ClampX(target.position.x), ref velocityX, smoothTime);
        pos.x = ClampX(pos.x);
        transform.position = pos;
    }

    public void SnapToTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector3 pos = transform.position;
        pos.x = ClampX(target.position.x);
        transform.position = pos;
        velocityX = 0f;
    }


    float ClampX(float x)
    {
        if (area == null || !area.IsValid)
        {
            return x;
        }

        float halfWidth = cam.orthographicSize * cam.aspect;
        float minX = area.LeftX + halfWidth;
        float maxX = area.RightX - halfWidth;


        if (minX > maxX)
        {
            return area.CenterX;
        }

        return Mathf.Clamp(x, minX, maxX);
    }
}