using UnityEngine;
using System.Collections;

public class DragScript : MonoBehaviour
{
    public Transform sphere;

    void Start()
    {
        //originalColor = renderer.sharedMaterial.color;//开始时得到物体着色  
        UIEventListener el = UIEventListener.Get(gameObject);
        el.onDragStart = Drag_Start;
        el.onDrag = Drag_Update;
        el.onDragEnd = Drag_End;
    }

    //public Transform RecordTarget;
    Vector3 screenSpace;
    Vector3 offset;
    void Drag_Start(GameObject obj)
    {
        if (sphere == null)
            return;

        if (c == null)
        {
            Transform t = GameApp.Instance.FightUI.ChessCtrl.GetRole().transform;
            sphere.position = t.position;
        }

        //RecordTarget = GameApp.Instance.FightUI.ChessCtrl.CameraControl.CamTarget.Target;
        GameApp.Instance.FightUI.ChessCtrl.CameraControl.CamTarget.Target = sphere;

        screenSpace = Camera.main.WorldToScreenPoint(sphere.position);//三维物体坐标转屏幕坐标  
        //将鼠标屏幕坐标转为三维坐标，再计算物体位置与鼠标之间的距离  
        offset = sphere.position - Camera.main.ScreenToWorldPoint(new Vector3(-Input.mousePosition.x / 6f, -Input.mousePosition.y * 1.5f, screenSpace.z));
    }
    void Drag_Update(GameObject obj, Vector2 delta)
    {
        if (sphere == null)
            return;

        //Debug.Log("OnDrag:" + delta.x + "_" + delta.y);
        LastDragTime = Time.realtimeSinceStartup;
        if (c == null)
            c = StartCoroutine("Recover");

        Vector3 curScreenSpace = new Vector3(-Input.mousePosition.x / 6f, -Input.mousePosition.y * 1.5f, screenSpace.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
        float x = Mathf.Clamp(curPosition.x, -5f, 13f);
        float z = Mathf.Clamp(curPosition.z, -4f, 11f);

        sphere.position = new Vector3(x, 0, z);
    }
    void Drag_End(GameObject obj)
    {

    }

    Coroutine c = null;
    float LastDragTime;
    IEnumerator Recover()
    {
        while (LastDragTime + 3f > Time.realtimeSinceStartup)
            yield return new WaitForEndOfFrame();

        Transform t = GameApp.Instance.FightUI.ChessCtrl.GetRole().transform;
        sphere.position = t.position;
        GameApp.Instance.FightUI.ChessCtrl.CameraControl.CamTarget.Target = t;
        c = null;
    }
}