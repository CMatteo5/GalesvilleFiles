using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public float smoothTime = 0.25f;
    public float maxSpeed = 8f;
    public float stopDistance = 0.02f;

    public AreaBounds area;
    public float edgePadding = 0.5f;

    float targetX;
    float velocityX;  
    bool isMoving;

    public bool IsMoving { get { return isMoving; } }
    public void MoveTo(float worldX)
    {
        targetX = ClampX(worldX);
        isMoving = true;
    }

    public void Stop()
    {
        isMoving = false;
        velocityX = 0f;
    }

    void Update()
    {
        if (!isMoving)
        {
            return;
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.SmoothDamp(pos.x, targetX, ref velocityX, smoothTime, maxSpeed);

        if (Mathf.Abs(targetX - pos.x) <= stopDistance)
        {
            pos.x = targetX;
            Stop();
        }

        transform.position = pos;
    }

    float ClampX(float x)
    {
        if (area == null || !area.IsValid)
        {
            return x;
        }

        float minX = area.LeftX + edgePadding;
        float maxX = area.RightX - edgePadding;

        if (minX > maxX)
        {
            return area.CenterX;
        }

        return Mathf.Clamp(x, minX, maxX);
    }
}