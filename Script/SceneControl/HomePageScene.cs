using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePageScene : MonoBehaviour
{
    public GameObject Earth;

    private Quaternion targetRotation;

    private Dictionary<int, Vector3> RotateAngleDic = new Dictionary<int, Vector3>()
    {
        {1, new Vector3(0, 0, 0)},
        {2, new Vector3(0, 0, 0f)},
        {3, new Vector3(11.04f, 0, -17.5f)}
    };

    private Action RotOverCallbackFun = null;

    void Start()
    {
        GameApp.Instance.HomePageSceneMgr = this;
    }

    void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.A))
        {
            
        }*/

        Earth.transform.rotation = Quaternion.Slerp(Earth.transform.rotation, targetRotation, Time.deltaTime * 3);

        /*if (Earth.transform.localEulerAngles.x < 338 && Earth.transform.localEulerAngles.x > 180)
        {
            Earth.transform.localEulerAngles = new Vector3(338, 0, Earth.transform.localEulerAngles.z);
            //return;
        }*/
    }

    public void RotateEarth(int toChapterID, Action RotOverCB)
    {
        if (toChapterID == 2)
        {
            if (RotOverCB != null)
            {
                RotOverCB();
            }
            return;
        }
            
        lastAngle = RotateAngleDic[toChapterID];

        targetRotation = Quaternion.Euler(lastAngle) * Quaternion.identity;

        RotOverCallbackFun = RotOverCB;
        if (Quaternion.Angle(targetRotation, Earth.transform.rotation) < 0.2f)
        {
            if (RotOverCallbackFun != null)
            {
                RotOverCallbackFun();
                RotOverCallbackFun = null;
            }
        }
    }

    public GameObject CameraNode;//3D相机
    public Transform SkyBgPlane;//天空背景面片
    public Transform CenterObj;//

    private Vector3 direction;
    public Transform camMaxTran;
    public Transform camMinTran;

    void OnPinch(PinchGesture gesture)
    {
        /*if (GameApp.Instance.HomePageUI.IsSomeUIShowing())
            return;

        direction = Vector3.Normalize(camMaxTran.position - camMinTran.position);
        //ContinuousGesturePhase phase = gesture.Phase;

        float delta = -gesture.Delta * 0.03f;

        if (delta < 0 && CameraNode.transform.position.z <= camMinTran.position.z)
            return;
        Vector3 temppos = CameraNode.transform.position + delta * direction;
        if (temppos.z < camMinTran.position.z)
        {
            temppos = CameraNode.transform.position + ((camMinTran.position.z - CameraNode.transform.position.z) / direction.z) * direction;
        }

        if (temppos.z > camMaxTran.position.z)
            temppos = camMaxTran.position;

        CameraNode.transform.position = temppos;

        float scale = Vector3.Distance(camMinTran.position, CameraNode.transform.position) / Vector3.Distance(camMinTran.position, camMaxTran.position);
        SkyBgPlane.localScale = Vector3.one * (0.66f + scale);*/
    }

    Vector3 lastAngle = Vector3.zero;
    void OnDrag(DragGesture gesture)
    {
        if (GameApp.Instance.HomePageUI.IsSomeUIShowing())
            return;

        //ContinuousGesturePhase phase = gesture.Phase;

        Vector2 deltaMove = gesture.DeltaMove * 0.03f;
        //Vector2 totalMove = gesture.TotalMove * 0.5f;

        lastAngle += new Vector3(-deltaMove.y, 0, deltaMove.x);
        //Debug.Log(lastAngle.x);

        if (lastAngle.x < -15)
            lastAngle = new Vector3(-15, 0, lastAngle.z);
        if (lastAngle.x > 15)
            lastAngle = new Vector3(15, 0, lastAngle.z);

        targetRotation = Quaternion.Euler(lastAngle) * Quaternion.identity;
    }
}
