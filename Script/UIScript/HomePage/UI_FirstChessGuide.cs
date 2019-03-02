using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FirstChessGuide : MonoBehaviour
{
    public Transform PetRoot;
    private GameObject PetModel = null;
    public RuntimeAnimatorController PetFlyAnimCrl;
    private Animator PetAnim;

    public GameObject[] StepObjs;

    void Start()
    {
        GameApp.Instance.FirstChessGuideUI = this;

        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        GameApp.Instance.FirstChessGuideUI = null;
    }

    void Update()
    {

    }

    public void SetGuideState(int stateIdx)
    {
        if (PetModel == null)
        {
            RoleConfig rc = null;
            if (CsvConfigTables.Instance.RoleCsvDic.TryGetValue((int)GameApp.Instance.MainPlayerData.PetID, out rc))
            {
                GameObject model = Resources.Load<GameObject>(StringBuilderTool.ToString("Prefabs/Actor/", rc.ModelName));
                if (model != null)
                {
                    PetModel = GameObject.Instantiate(model);
                    MyTools.setLayerDeep(PetModel, LayerMask.NameToLayer("Guide"));
                    PetModel.transform.parent = PetRoot;
                    PetModel.transform.localPosition = new Vector3(-110, -73, -84);
                    PetModel.transform.localEulerAngles = new Vector3(0, -140, 0);
                    PetModel.transform.localScale = Vector3.one * 220;

                    PetAnim = PetModel.GetComponent<Animator>();
                    PetAnim.runtimeAnimatorController = PetFlyAnimCrl;
                }
            }
        }

        switch(stateIdx)
        {
            case 0:
                GameApp.Instance.SoundInstance.PlayVoice("0_HomePageEntrance");
                PetModel.transform.localPosition = new Vector3(-110, -73, -84);
                PetModel.transform.localEulerAngles = new Vector3(0, -140, 0);
                StepObjs[0].SetActive(true);
                break;
            case 1:
                GameApp.Instance.SoundInstance.StopSe("0_HomePageEntrance");
                GameApp.Instance.SoundInstance.PlayVoice("1_IntroToDinosaurLevel");
                StepObjs[0].SetActive(false);
                StepObjs[1].SetActive(true);
                break;
            case 2:
                GameApp.Instance.SoundInstance.StopSe("1_IntroToDinosaurLevel");
                GameApp.Instance.SoundInstance.PlayVoice("2_FromTriassicPeriod");
                PetModel.transform.localPosition = new Vector3(111, -73, -84);
                StepObjs[1].SetActive(false);
                StepObjs[2].SetActive(true);
                break;
            case 3:
                GameApp.Instance.SoundInstance.StopSe("2_FromTriassicPeriod");
                GameApp.Instance.SoundInstance.PlayVoice("3_CreateAdventureTeams");
                StepObjs[2].SetActive(false);
                StepObjs[3].SetActive(true);
                break;
            case 4:
                GameApp.Instance.SoundInstance.StopSe("3_CreateAdventureTeams");
                GameApp.Instance.SoundInstance.PlayVoice("4_CheckReward");
                PetModel.transform.localPosition = new Vector3(-187, -275, -84);
                StepObjs[3].SetActive(false);
                StepObjs[4].SetActive(true);
                break;
            case 5:
                GameApp.Instance.SoundInstance.StopSe("4_CheckReward");
                GameApp.Instance.SoundInstance.PlayVoice("5_InvitePartner");
                PetModel.transform.localPosition = new Vector3(300, -112, -84);
                PetModel.transform.localEulerAngles = new Vector3(0, 140, 0);
                StepObjs[4].SetActive(false);
                StepObjs[5].SetActive(true);
                break;
            case 6:
                GameApp.Instance.SoundInstance.StopSe("5_InvitePartner");
                GameApp.Instance.SoundInstance.PlayVoice("6_InvitePet");
                PetModel.transform.localPosition = new Vector3(166, -69, -84);
                PetModel.transform.localEulerAngles = new Vector3(0, -140, 0);
                StepObjs[5].SetActive(false);
                StepObjs[6].SetActive(true);
                break;
            case 7:
                GameApp.Instance.SoundInstance.StopSe("6_InvitePet");
                GameApp.Instance.SoundInstance.PlayVoice("7_BeganToAdventure");
                PetModel.transform.localPosition = new Vector3(218, -256, -84);
                PetModel.transform.localEulerAngles = new Vector3(0, 140, 0);
                StepObjs[6].SetActive(false);
                StepObjs[7].SetActive(true);
                break;
            case 8:
                GameApp.Instance.SoundInstance.StopSe("7_BeganToAdventure");
                PetModel.SetActive(false);
                StepObjs[7].SetActive(false);
                break;
            case 9:
                GameApp.Instance.SoundInstance.PlayVoice("8_Throwing");
                PetModel.SetActive(true);
                StepObjs[8].SetActive(true);
                break;
            case 10:
                GameApp.Instance.SoundInstance.StopSe("8_Throwing");
                StepObjs[8].SetActive(false);

                string key = StringBuilderTool.ToString(SerPlayerData.GetAccountID(), "_NeedShow_FirstChessGuide");
                PlayerPrefs.SetInt(key, 0);

                Destroy(gameObject);
                break;
        }
    }
}
