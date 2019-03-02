using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{
    public float deadTime;

    void Awake()
    {
        Destroy (gameObject, deadTime);
    }

    public void ReSet(float time)
    {
        Destroy (gameObject, time);
    }
}
