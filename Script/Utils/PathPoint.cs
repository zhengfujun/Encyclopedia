using UnityEngine;
using System.Collections;

//��ʶ���λ�õĵ�
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
