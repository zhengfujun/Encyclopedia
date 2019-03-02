using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public float smoothing = 1;

    public Transform Target;

    private MainFightCameraControl mfcc;

    void Start()
    {
        if (GameApp.Instance.FightUI != null)
        {
            mfcc = GameApp.Instance.FightUI.ChessCtrl.CameraControl;

            for (int i = 0; i < StandbyAngle_0.Length; i++)
            {
                StandbyAngle_0[i] = i * 2 + 1;
            }
            for (int i = 0; i < StandbyAngle_90.Length; i++)
            {
                StandbyAngle_90[i] = i * 2 + 1.5f;
            }
            for (int i = 0; i < StandbyAngle_180.Length; i++)
            {
                StandbyAngle_180[i] = (i - 1) * 2 + 2;
            }
            for (int i = 0; i < StandbyAngle_270.Length; i++)
            {
                StandbyAngle_270[i] = (i - 1) * 2 + 2.5f;
            }
        }
    }

    void Update()
    {
        if (mfcc == null)
            return;

        transform.position = Vector3.Lerp(transform.position, Target.position, smoothing * Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, smoothing * Time.deltaTime);

        if(Time.frameCount % 5 == 0)
        {
            CurOrientationType = GetOrientationType();
            if (CurOrientationType != LastOrientationType && CurOrientationType != -1)
            {
                switch(CurOrientationType)
                {
                    case 0:
                        mfcc.ChangeAzimuth(FindNearestAngle(StandbyAngle_0, mfcc.GetAzimuth()));
                        break;
                    case 90:
                        mfcc.ChangeAzimuth(FindNearestAngle(StandbyAngle_90, mfcc.GetAzimuth()));
                        break;
                    case 180:
                        mfcc.ChangeAzimuth(FindNearestAngle(StandbyAngle_180, mfcc.GetAzimuth()));
                        break;
                    case 270:
                        mfcc.ChangeAzimuth(FindNearestAngle(StandbyAngle_270, mfcc.GetAzimuth()));
                        break;
                }

                //Debug.Log(CurOrientationType);
            }


            LastOrientationType = CurOrientationType;
        }
    }

    float[] StandbyAngle_0 = new float[32];
    float[] StandbyAngle_90 = new float[32];
    float[] StandbyAngle_180 = new float[32];
    float[] StandbyAngle_270 = new float[32];
    private int CurOrientationType = -1;
    private int LastOrientationType = -1;
    int GetOrientationType()
    {
        if(Mathf.Abs(Target.localEulerAngles.y - 0) < 30f)
        {
            return 0;
        }
        else if (Mathf.Abs(Target.localEulerAngles.y - 90) < 30f)
        {
            return 90;
        }
        else if (Mathf.Abs(Target.localEulerAngles.y - 180) < 30f)
        {
            return 180;
        }
        else if (Mathf.Abs(Target.localEulerAngles.y - 270) < 30f)
        {
            return 270;
        }
        return -1;
    }

    float FindNearestAngle(float[] StandbyAngle, float Reduced)
    {
        float difference = float.MaxValue;
        int index = 0;
        for (int i = 0; i < StandbyAngle.Length; i++)
        {
            float r = Mathf.Abs(StandbyAngle[i] - Reduced);
            if(r < difference)
            {
                index = i;
                difference = r;
            }
        }
        return StandbyAngle[index];
    }
}
