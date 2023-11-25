using UnityEngine;

public static class TransformExtensions
{
    public static Vector3 GetDirection(this Transform transform, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return transform.up;
            case Direction.Down:
                return -transform.up;
            case Direction.Left:
                return -transform.right;
            case Direction.Right:
                return transform.right;
            case Direction.Forward:
                return transform.forward;
            case Direction.Back:
                return -transform.forward;
            default:
                return Vector3.zero;
        }
    }
}