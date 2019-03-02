using UnityEngine;
using System.Collections;

//标识结点位置的点
public class PathPoint : MonoBehaviour
{
    public Color color = Color.yellow;
    public float size = 0.3f;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, size);
    }
}
