using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Angel : MonoBehaviour
{
    public UILabel ResidueRoundCntLab;

    public UILabel HasAngelHint;

    public GameObject Model;
    public UILabel DesLab;

    public UIFollowTarget ft;

    public Animator animModel;
    public Animation animFly;

    /*void Start()
    {

    }*/

    /*void Update()
    {

    }*/

    public void Show(Action ShowEndCB)
    {
        Model.SetActive(true);
        TweenPosition tp = TweenPosition.Begin(Model, 0.5f, new Vector3(0, -80, 400));
        tp.onFinished.Add(new EventDelegate(() =>
        {
            StartCoroutine("DelayClose", ShowEndCB); 

            GameApp.Instance.SoundInstance.PlayVoice("AngelEffectDes");
        }));

        GameApp.Instance.SoundInstance.PlaySe("AngelEnter");
    }

    IEnumerator DelayClose(Action ShowEndCB)
    {
        DesLab.gameObject.SetActive(true);

        animModel.Play("CastSpell");
        yield return new WaitForSeconds(1f);
        animModel.Play("Idle");

        yield return new WaitForSeconds(3f);

        DesLab.gameObject.SetActive(false);

        ft.enabled = true;

        animFly.Play("fly");
        yield return new WaitForSeconds(animFly["fly"].length);

        ft.enabled = false;
        ft.gameObject.transform.parent = ft.target;
        yield return new WaitForEndOfFrame();
        MyTools.setLayerDeep(ft.gameObject,LayerMask.NameToLayer("Default"));
        ft.gameObject.transform.localPosition = Vector3.zero;
        ft.gameObject.transform.localEulerAngles = Vector3.zero;
        ft.gameObject.transform.localScale = Vector3.one * 0.67f;

        ResidueRoundCntLab.gameObject.SetActive(true);
        //UIFollowTarget LabFT = ResidueRoundCntLab.gameObject.GetComponent<UIFollowTarget>();
        //LabFT.target = MyTools.DeepinGetChild(LabFT.target, "Head");

        if(ShowEndCB != null)
        {
            ShowEndCB();
        }
    }

    public void SetResidueRoundCount(int cnt)
    {
        if(cnt == 0)
        {
            ResidueRoundCntLab.gameObject.SetActive(false);
        }
        else
        {
            ResidueRoundCntLab.text = cnt.ToString();
        }
    }

    public void ShowHasAngelHint(bool isShow)
    {
        HasAngelHint.gameObject.SetActive(isShow);
    }
}
