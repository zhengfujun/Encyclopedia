using UnityEngine;
using System.Collections;

//简单的按照结点组路径移动对象
public class SimplePath : MonoBehaviour
{
    //路径寻路中的所有点
    public Transform[] paths = null;
    public float speed = 10.0f;
    public float time = 0;
    public Color color = Color.yellow;
    public bool IsOrientToPath = false;
    private Hashtable args;
    private bool IsCurPause;

    private Camera MainCam = null;

    void Awake()
    {
        //iTween.Stop();
        IsCurPause = false;
    }

    void Start()
    {
        if (paths != null && paths.Length > 0)
        {
            Run();
        }
    }

    public void Run()
    {
        args = new Hashtable();
        args.Add("path", paths);
        args.Add("easeType", iTween.EaseType.linear);
        if (time > 0)
        {
            args.Add("time", time);
        }
        else
        {
            args.Add("speed", speed);
        }
        args.Add("movetopath", false);
        args.Add("orienttopath", IsOrientToPath);
        args.Add("oncomplete", "MoveEnd");

        iTween.MoveTo(gameObject, args);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (MainCam == null && GameApp.Instance.FightUI != null)
                MainCam = GameApp.Instance.FightUI.ChessCtrl.CameraControl.FightCamera;
            if (MainCam == null && GameApp.Instance.TravelUI != null)
                MainCam = GameApp.Instance.TravelUI.SceneCam;

            Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    //Debug.DrawLine(MainCam.transform.position, hit.point);
                    //Debug.Log(gameObject.name);
                    StartCoroutine("PlayClickAnim");
                }
            }
        }
    }

    void MoveEnd()
    {
        iTween.MoveTo(gameObject, args);
    }

    void OnDrawGizmos()
    {
        //iTween.DrawLine(paths, Color.yellow);
        iTween.DrawPath(paths, color);
    }

    public void StopMove()
    {
        try
        {
            if (!IsCurPause)
            {
                iTween.Pause(gameObject);
                IsCurPause = true;
            }
        }
        catch (System.Exception ex)
        {
            //Debug.Log("SimplePath StopMove iTween.Pause() ex.Message = " + ex.Message);
        } 
    }

    public void ContinueMove()
    {
        try
        {
            if (IsCurPause)
            {
                iTween.Resume(gameObject);
                IsCurPause = false;
            }
        }
        catch (System.Exception ex)
        {
            //Debug.Log("SimplePath ContinueMove iTween.Resume() ex.Message = " + ex.Message);
        }
    }

    IEnumerator PlayClickAnim()
    {
        StopMove();
        Animation anim = gameObject.GetComponent<Animation>();
        if (anim == null)
        {
            anim = gameObject.GetComponentInChildren<Animation>();
        }
        if (anim != null)
        {
            anim.CrossFade("click", 0.2f);
            yield return new WaitForSeconds(anim["click"].length);
            anim.CrossFade("move", 0.2f);
            ContinueMove();
        }
    }
}
