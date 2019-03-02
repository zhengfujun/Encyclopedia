using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OpenHint : MonoBehaviour
{
    public GameObject Arrows;
    public TweenPosition ArrowsMove;

    public GameObject Finger;
    public TweenPosition FingerMove;
    public TweenAlpha FingerAlpha;

    void Start()
    {
        //InvokeRepeating("Play", 0, 1f);
    }

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            StartCoroutine("Play");
        }
    }*/

    IEnumerator Play()
    {
        //yield return new WaitForEndOfFrame();

        FingerMove.PlayForward();
        FingerAlpha.PlayForward();

        EventDelegate del_2 = new EventDelegate(() =>
        {
            FingerMove.PlayReverse();
            FingerAlpha.PlayReverse();

        });

        EventDelegate del_1 = new EventDelegate(() =>
        {
            ArrowsMove.PlayForward();
            ArrowsMove.SetOnFinished(del_2);
        });

        FingerMove.SetOnFinished(del_1);

        yield return new WaitForSeconds(2.0f);

        ArrowsMove.ResetToBeginning();
        //FingerMove.ResetToBeginning();
        //FingerAlpha.ResetToBeginning();

        FingerMove.RemoveOnFinished(del_1);
        ArrowsMove.RemoveOnFinished(del_2);
    }
}
