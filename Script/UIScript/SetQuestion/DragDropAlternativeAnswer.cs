using UnityEngine;

public class DragDropAlternativeAnswer : UIDragDropItem
{
    private UI_AlternativeAnswerUnit aau = null;

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();

    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        if (surface != null)
        {
            if(aau == null)
            {
                aau = gameObject.GetComponent<UI_AlternativeAnswerUnit>();
            }

            switch (surface.name)
            {
                case "Answer_Right":
                    Debug.Log("设置正确答案");
                    GameApp.Instance.HomePageUI.SetQuestionUI.SetAnswer(-1, aau.Name.text);                    
                    break;
                case "Answer_Error_1":
                    Debug.Log("设置错误答案1");
                    GameApp.Instance.HomePageUI.SetQuestionUI.SetAnswer(0, aau.Name.text);                    
                    break;
                case "Answer_Error_2":
                    Debug.Log("设置错误答案2");
                    GameApp.Instance.HomePageUI.SetQuestionUI.SetAnswer(1, aau.Name.text);
                    break;
                case "Answer_Error_3":
                    Debug.Log("设置错误答案3");
                    GameApp.Instance.HomePageUI.SetQuestionUI.SetAnswer(2, aau.Name.text);
                    break;
            }
        }

        base.OnDragDropRelease(surface);
    }
}
