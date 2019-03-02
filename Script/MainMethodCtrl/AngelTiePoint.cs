using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelTiePoint : MonoBehaviour
{
    public float smoothing = 1;

    public Transform Target;

    public Transform ModelRoot;

    /*void Start()
    {

    }*/

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, smoothing * Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, smoothing * Time.deltaTime);
    }

    public void ClearModel()
    {
        MyTools.DestroyChildNodes(ModelRoot);
    }
}
