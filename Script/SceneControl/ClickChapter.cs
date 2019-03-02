using UnityEngine;
using System.Collections;

public class ClickChapter : MonoBehaviour
{
    
    private Camera MainCam = null;

    void Awake()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameApp.Instance.HomePageUI.IsSomeUIShowing())
                return;

            if(MainCam == null)
                MainCam = GameApp.Instance.HomePageSceneMgr.CameraNode.GetComponent<Camera>();

            Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    //Debug.DrawLine(MainCam.transform.position, hit.point);
                    //Debug.Log(gameObject.name);

                    int ChapterID = int.Parse(MyTools.GetLastString(gameObject.name, '_'));
                    GameApp.Instance.HomePageSceneMgr.RotateEarth(ChapterID,
                        () =>
                        {
                            if (ChapterID == 1)
                            {
                                GameApp.Instance.SoundInstance.PlaySe("DinosaurRoar");
                                GameApp.Instance.SoundInstance.PlaySe("Elephant");

                                GameApp.Instance.HomePageUI.StageMapUI.Show(true, ChapterID);
                            }
                            else
                            {
                                GameApp.Instance.CommonHintDlg.OpenHintBox("", EHintBoxStyle.eStyle_ComingSoon);
                            }
                        });
                }
            }
        }
    }
}
