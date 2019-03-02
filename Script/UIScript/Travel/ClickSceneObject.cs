using UnityEngine;
using System.Collections;

public class ClickSceneObject : MonoBehaviour
{
    //private Camera MainCam = null;
    private int LastSugarIndex = -1;

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Debug.Log(gameObject.name + " GetMouseButton " + Time.time);

            if (GameApp.Instance.TravelUI.IsSomeUIShowing())
                return;

            Ray ray = GameApp.Instance.TravelUI.SceneCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    if (gameObject.name.Contains("Sugar_"))
                    {
                        int Index = int.Parse(MyTools.GetLastString(gameObject.name, '_'));

                        if (LastSugarIndex == Index)
                            return;

                        GameApp.Instance.TravelUI.PickSugar(Index);

                        LastSugarIndex = Index;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(gameObject.name + " GetMouseButtonDown " + Time.time);

            if (GameApp.Instance.TravelUI.IsSomeUIShowing())
                return;

            //if (MainCam == null)
            //    MainCam = GameApp.Instance.TravelUI.SceneCam;

            Ray ray = GameApp.Instance.TravelUI.SceneCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    //Debug.DrawLine(MainCam.transform.position, hit.point);
                    //Debug.Log(gameObject.name);

                    switch (gameObject.name)
                    {
                        case "SugarTree":
                            GameApp.Instance.TravelUI.OnClick_SugarTree();
                            break;
                        case "Gashapon":
                            GameApp.Instance.TravelUI.OnClick_Gashapon();
                            break;
                        case "Shop":
                            GameApp.Instance.TravelUI.OnClick_Shop();
                            break;
                        case "Room":
                            GameApp.Instance.TravelUI.OnClick_Room();
                            break;
                        case "LetterBox":
                            GameApp.Instance.TravelUI.OnClick_LetterBox();
                            break;
                        case "Bag":
                            GameApp.Instance.TravelUI.OnClick_Bag();
                            break;
                        case "Courtyard":
                            GameApp.Instance.TravelUI.OnClick_Courtyard();
                            break;
                        case "Item":
                            GameApp.Instance.TravelUI.OnClick_Item();
                            break;
                        case "Present":
                            GameApp.Instance.TravelUI.OnClick_Present();
                            break;
                        case "Bookrack":
                            GameApp.Instance.TravelUI.OnClick_Bookrack();
                            break;
                        case "Seafloor":
                            GameApp.Instance.TravelUI.OnClick_Seafloor();
                            break;
                        case "BaoBaoLong":
                            {
                                Animator anim = GetComponent<Animator>();
                                anim.CrossFade(StringBuilderTool.ToString("S_Idle_", Random.Range(1,4)), 0.1f);
                            }
                            break;
                    }

                    /*if(gameObject.name.Contains("Sugar_"))
                    {
                        int Index = int.Parse(MyTools.GetLastString(gameObject.name, '_'));
                        GameApp.Instance.TravelUI.PickSugar(Index);
                    }*/
                }
            }
        }
    }
}
