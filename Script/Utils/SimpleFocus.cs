using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFocus : MonoBehaviour
{
    private int curIdx = 0;
    public Transform[] focus;
    public Color color = Color.yellow;

    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();

        StartCoroutine("Run");
    }

    IEnumerator Run()
    {
        //StartCoroutine("ILookToTarget");
        Quaternion dir = Quaternion.LookRotation((focus[curIdx].localPosition - transform.localPosition).normalized);
        while (Vector3.Distance(focus[curIdx].localPosition, transform.localPosition) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, dir, Time.deltaTime * 4);
            yield return new WaitForEndOfFrame();
        }

        int rIdx = Random.Range(1, 4);
        string amimStr = "S_Idle_" + rIdx;
        anim.Play(amimStr);
        yield return new WaitForSeconds(AnimatorLength(rIdx));
        curIdx++;
        if (curIdx >= focus.Length)
            curIdx = 0;

        StartCoroutine("Run");
    }

    IEnumerator ILookToTarget()
    {
        Quaternion dir = Quaternion.LookRotation((focus[curIdx].localPosition - transform.localPosition).normalized);
        float looktime = 2f;
        while (true)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,dir,Time.deltaTime * 4);

            looktime -= Time.deltaTime;
            if (looktime <= 0)
                break;

            if (transform.rotation == dir)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public float AnimatorLength(int idx)
    {
        AnimationClip[] anis = anim.runtimeAnimatorController.animationClips;
        float atime = 0;
        for (int i = 0; i < anis.Length; ++i)
        {
            if ((idx == 1 && anis[i].name == "Idle Search") ||
                (idx == 2 && anis[i].name == "Idle Smell") ||
                (idx == 3 && anis[i].name == "Idle Yaw"))
            {
                atime = anis[i].length;
                break;
            }
        }
        return atime;
    }

    void OnDrawGizmos()
    {
        iTween.DrawPath(focus, color);
    }
}
