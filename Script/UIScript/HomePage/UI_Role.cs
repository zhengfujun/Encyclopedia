using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using common;

public class UI_Role : MonoBehaviour
{
    public bool isShow = false;

    public UIAppearEffect AppearEffect;
    private Action CloseCB = null;

    public GameObject Horizontal_Player;
    public GameObject Horizontal_Pet;

    public ModelUnit[] mu;

    public GameObject[] Hide4LoginScene;

    public GameObject GuideRoot;
    public GameObject[] StepObjs;
    public UIInput PlayerNameInp;
    public UIInput PlayerPetNameInp;
    public UILabel GoodNameDes;

    public Transform PlayerModelRoot;
    public Transform PetModelRoot;

    void Awake()
    {
        /*
         * act_m01_M07_v2
         * act_f01_F08
         * act_f01_UTC_Default
         * act_f01_elf demo
         * act_f01_Yuko_SchoolUniform_summer
         * 
         * idle
         * win
         * attack
         */
    }

    /*void Start()
    {
        
    }*/

    /*void OnDestroy()
    {

    }*/

    /*void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {

        }
    }*/

    public void Show(bool isShow, Action CloseCallback = null)
    {
        if (gameObject.activeSelf == isShow)
            return;

        if (isShow)
        {
            if (GameApp.Instance.UILogin == null)
            {
                GameApp.Instance.SoundInstance.PlayBgm("BGM_Role");
            }
            else
            {
                for (int i = 0; i < Hide4LoginScene.Length; i++)
                {
                    Hide4LoginScene[i].SetActive(false);
                }
            }

            gameObject.SetActive(true);

            for (int i = 0; i < mu.Length; i++)
                mu[i].Refresh();

            AppearEffect.transform.localScale = Vector3.one * 0.01f;
            AppearEffect.Open(AppearType.Popup, AppearEffect.gameObject, 0, () =>
                {
                    if (GameApp.Instance.UILogin != null)
                    {
                        GuideRoot.SetActive(true);
                        PlayerNameInp.value = GameApp.Instance.UILogin.GetNickName();
                        StartCoroutine("_Guide");
                    }
                });

            isShow = true;

            CloseCB = CloseCallback;
        }
        else
        {
            GameApp.Instance.SoundInstance.PlayBgm("BGM_HomePage");
            AppearEffect.Close(AppearType.Popup, () =>
            {
                if (CloseCB != null)
                {
                    CloseCB();
                    CloseCB = null;
                }

                gameObject.SetActive(false);

                isShow = false;
            });
        }
    }
    
    #region _按钮
    /// <summary> 点击退出 </summary>
    public void OnClick_Back()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");
        Debug.Log("点击【退出】");

        Show(false);
    }
    #endregion

    #region _引导
    IEnumerator _Guide()
    {
        StepObjs[0].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        StepObjs[1].SetActive(true);
        StepObjs[0].SetActive(false); 

        while (!IsSelOver)
            yield return new WaitForEndOfFrame();

        StepObjs[2].SetActive(true);
        StepObjs[3].SetActive(true); 
        StepObjs[1].SetActive(false);
        yield return new WaitForSeconds(3.0f);
        StepObjs[4].SetActive(true);
        StepObjs[3].SetActive(false);

        while (!IsCreateNameOver)
            yield return new WaitForEndOfFrame();

        GoodNameDes.text = StringBuilderTool.ToInfoString("原来你叫", PlayerNameInp.value, "啊，真是好名字！");
        StepObjs[5].SetActive(true);
        StepObjs[4].SetActive(false);

        yield return new WaitForSeconds(3.0f);
        StepObjs[2].SetActive(false);
        StepObjs[6].SetActive(true);
        StepObjs[5].SetActive(false);
        Horizontal_Player.SetActive(false);
        Horizontal_Pet.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        StepObjs[7].SetActive(true);
        StepObjs[6].SetActive(false);
        IsSelOver = false;

        while (!IsSelOver)
            yield return new WaitForEndOfFrame();

        StepObjs[2].SetActive(true);
        StepObjs[8].SetActive(true);
        StepObjs[7].SetActive(false);
        IsCreateNameOver = false;

        while (!IsCreateNameOver)
            yield return new WaitForEndOfFrame();

        Horizontal_Pet.SetActive(false);
        StepObjs[2].SetActive(false);
        StepObjs[9].SetActive(true);
        StepObjs[8].SetActive(false);

        yield return new WaitForSeconds(3.0f);

        GameApp.SendMsg.CreatePlayer(PlayerNameInp.value);
    }

    private bool IsSelOver = false;
    public void SelOver(uint RoleID, Transform Model)
    {
        IsSelOver = true;

        if (RoleID > 1000)
        {
            GameApp.Instance.MainPlayerData.PetID = RoleID;

            GameObject pm = GameObject.Instantiate(Model.gameObject);
            pm.transform.parent = PetModelRoot;
            pm.transform.localPosition = Vector3.zero;
            pm.transform.localEulerAngles = Model.transform.localEulerAngles;
            pm.transform.localScale = Model.transform.localScale;
        }
        else
        {
            GameApp.Instance.MainPlayerData.RoleID = RoleID;

            GameObject pm = GameObject.Instantiate(Model.gameObject);
            pm.transform.parent = PlayerModelRoot;
            pm.transform.localPosition = Vector3.zero;
            pm.transform.localEulerAngles = Model.transform.localEulerAngles;
            pm.transform.localScale = Model.transform.localScale;
        }
    }

    public void ClickRandomNameBtn()
    {
        if (Loading.InLoading)
            return;

        GameApp.Instance.SoundInstance.PlaySe("button");

        PlayerNameInp.value = GameApp.Instance.RandomNameInstance.GenerateRandomName();
    }
    public void ClickRandomPetNameBtn()
    {
        if (Loading.InLoading)
            return;

        GameApp.Instance.SoundInstance.PlaySe("button");

        PlayerPetNameInp.value = GameApp.Instance.RandomPetNameInstance.GenerateRandomName();
    }

    public void OnPlayerNameInpChange()
    {
        if (GameApp.Instance.FilterSWInstance.FilterBl(PlayerNameInp.value))
        {
            PlayerNameInp.value = GameApp.Instance.FilterSWInstance.Filter(PlayerNameInp.value);
        }
    }
    public void OnPetNameInpChange()
    {
        if (GameApp.Instance.FilterSWInstance.FilterBl(PlayerPetNameInp.value))
        {
            PlayerPetNameInp.value = GameApp.Instance.FilterSWInstance.Filter(PlayerPetNameInp.value);
        }
    }

    public void ClickConfirmCreatePlayerNameBtn()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (Loading.InLoading)
            return;

        if (PlayerNameInp.value.Length == 0 ||
            PlayerNameInp.value == "玩家昵称七个字")
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("玩家昵称不能为空！");
            return;
        }

        if (PlayerNameInp.value.Contains("*"))
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("玩家昵称中含有非法字符！");
            return;
        }

        GameApp.SendMsg.CheckPlayerName(PlayerNameInp.value);
    }
    /// <summary> 检测玩家名结果 </summary>
    private bool IsCreateNameOver = false;
    public void CheckNameRes(LogicRes res, string name)
    {
        if (res == LogicRes.CheckName_Success)
        {
            IsCreateNameOver = true;
        }
        else if (res == LogicRes.CheckName_Error)
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("玩家昵称重复不可用！请修改后再试。");
        }
    }

    public void ClickConfirmCreatePetNameBtn()
    {
        GameApp.Instance.SoundInstance.PlaySe("button");

        if (Loading.InLoading)
            return;

        if (PlayerPetNameInp.value.Length == 0 ||
            PlayerPetNameInp.value == "宝宝龙名七个字")
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("宝宝龙名不能为空！");
            return;
        }

        if (PlayerPetNameInp.value.Contains("*"))
        {
            GameApp.Instance.CommonHintDlg.OpenHintBox("宝宝龙名中含有非法字符！");
            return;
        }

        PetInfo.PetInfoInstance.Name = PlayerPetNameInp.value;
        IsCreateNameOver = true;
    }
    #endregion
}
