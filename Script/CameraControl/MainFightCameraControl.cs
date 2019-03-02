using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityStandardAssets.CinematicEffects;

/// <summary>
/// 常规战斗相机控制
/// </summary>
public class MainFightCameraControl : MonoBehaviour
{
    public SphericalVector Data = new SphericalVector(0, 0, 1);

    public Camera FightCamera;

    public CameraTarget CamTarget;//相机目标

    public Vector3 TargetOffset = Vector3.zero;
    public float CameraLength;
    private float zoomTemp;
    private float zoomVal;

    //public float SmoothDampValue = 0.1f;

    void Awake()
    {
        FightCamera = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        //Data.Zenith = -0.4f;
        //Data.Length = CameraLength;
        zoomTemp = CameraLength;

        if (GameApp.Instance.GetParameter("EnableDepthOfField") > 0)
        {
            DepthOfField dof = FightCamera.gameObject.GetComponent<DepthOfField>();
            if (dof != null)
                dof.enabled = true;
        }
        if (GameApp.Instance.GetParameter("EnableBloom") > 0)
        {
            Bloom b = FightCamera.gameObject.GetComponent<Bloom>();
            if (b != null)
                b.enabled = true;
        }
        if (GameApp.Instance.GetParameter("EnableAmbientOcclusion") > 0)
        {
            AmbientOcclusion ao = FightCamera.gameObject.GetComponent<AmbientOcclusion>();
            if (ao != null)
                ao.enabled = true;
        }
    }

    //Vector3 currentVelocity;
    void LateUpdate()
    {
        //Data.Zenith = Mathf.Clamp(Data.Zenith, -0.8f, 0f);
        Data.Length += (zoomVal - Data.Length) / 10;

        //Time.timeScale += (1 - Time.timeScale) / 10f;
        Vector3 lookAt = TargetOffset;

        if (CamTarget != null)
        {
            lookAt += CamTarget.transform.position;

            Vector3 oldPos = gameObject.transform.position;
            Vector3 newPos = Data.Position;
            newPos += lookAt;
            //gameObject.transform.position = Vector3.SmoothDamp(oldPos, newPos, ref currentVelocity, 1.0f);
            gameObject.transform.position = Vector3.Lerp(oldPos, newPos, 10.2f);
            gameObject.transform.LookAt(lookAt);
        }

        zoomVal = CameraLength;
    }

    public float GetAzimuth()
    {
        return Data.Azimuth;
    }
    public void ChangeAzimuth(/*float fromAzimuth,*/ float toAzimuth/*, float time*/)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
                            "name", "ChangeAzimuth",
                            "from", Data.Azimuth/*fromAzimuth*/,
                            "to", toAzimuth,
                            "easetype", iTween.EaseType.easeInOutQuad,
                            "loopType", iTween.LoopType.none,
                            "onupdate", "_ChangeAzimuth",
                            "time", Const.ChangeAzimuthDuration/*time*/));
    }
    void _ChangeAzimuth(float v)
    {
        Data.Azimuth = v;
    }
}
