using UnityEngine;
public class AreaBounds : MonoBehaviour
{
    public Transform leftEdge;
    public Transform rightEdge;

    public bool IsValid
    {
        get { return leftEdge != null && rightEdge != null; }
    }

    public float LeftX
    {
        get { return Mathf.Min(leftEdge.position.x, rightEdge.position.x); }
    }

    public float RightX
    {
        get { return Mathf.Max(leftEdge.position.x, rightEdge.position.x); }
    }

    public float CenterX
    {
        get { return (LeftX + RightX) * 0.5f; }
    }
}