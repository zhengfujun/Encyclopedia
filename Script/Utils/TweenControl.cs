using UnityEngine;
using System.Collections;

public class TweenControl : SimpleSingleton<TweenControl> 
{
	/// <summary>
	/// 缩放效果，有接受消息的物体和回调方法名
	/// </summary>
	public void TweenScaleEffect(GameObject go,GameObject rece,string cbstr,float duration,float from_rate,float to_rate,bool isfrom = false,float delaytime = 0.0f)
	{
		if(go == null || !go.activeInHierarchy)
			return;

		TweenScale Tw = TweenScale.Begin(go,duration,Vector3.one * to_rate);
		if(isfrom){ Tw.from = Vector3.one * from_rate; }
		Tw.callWhenFinished = cbstr;
		Tw.eventReceiver = rece;
		Tw.delay = delaytime;
		BegainTween(Tw);
	}

	/// <summary>
	/// 缩放效果
	/// </summary>
	public void TweenScaleEffect(GameObject go,float duration,float from_rate,float to_rate,bool isfrom = false,float delaytime = 0.0f)
	{
		if(go == null || !go.activeInHierarchy)
			return;
		
		TweenScale Tw = TweenScale.Begin(go,duration,Vector3.one * to_rate);
		if(isfrom){ Tw.from = Vector3.one * from_rate; }
		Tw.delay = delaytime;
		BegainTween(Tw);
	}

	/// <summary>
	/// 缩放效果
	/// </summary>
	public void TweenScaleEffect(GameObject go,float duration,Vector3 from_vc,Vector3 to_vc,bool isfrom = false,float delaytime = 0.0f)
	{
		if(go == null || !go.activeInHierarchy)
			return;
		
		TweenScale Tw = TweenScale.Begin(go,duration,to_vc);
		if(isfrom){ Tw.from = from_vc; }
		Tw.delay = delaytime;
		BegainTween(Tw);
	}

	/// <summary>
	/// 渐隐渐现效果，有接受消息的物体和回调方法名
	/// </summary>
	public void TweenAlphaEffect(GameObject go,GameObject rece,string cbstr,float duration,float from_rate,float to_rate,bool isfrom = false,float delaytime = 0.0f)
	{
		if(go == null || !go.activeInHierarchy)
			return;
		
		TweenAlpha Tw = TweenAlpha.Begin(go,duration,to_rate);
		if(isfrom){ Tw.from = from_rate; }
		Tw.callWhenFinished = cbstr;
		Tw.eventReceiver = rece;
		Tw.delay = delaytime;
		BegainTween(Tw);
	}

	/// <summary>
	/// 渐隐渐现效果
	/// </summary>
	public void TweenAlphaEffect(GameObject go,float duration,float from_rate,float to_rate,bool isfrom = false,float delaytime = 0.0f,UITweener.Style style = UITweener.Style.Once)
	{
		if(go == null)
			return;
        
		TweenAlpha Tw = TweenAlpha.Begin(go,duration,to_rate);
		if(isfrom){ Tw.from = from_rate; }
		Tw.delay = delaytime;
        Tw.style = style;
		BegainTween(Tw);
	}

	/// <summary>
	/// 颜色效果，有接受消息的物体和回调方法名
	/// </summary>
	public void TweenColorEffect(GameObject go,GameObject rece,string cbstr,float duration,Color from_co,Color to_co,bool isfrom = false,UITweener.Style style = UITweener.Style.Once)
	{
		if(go == null || !go.activeInHierarchy)
			return;
		
		TweenColor Tw = TweenColor.Begin(go,duration,to_co);
		if(isfrom){ Tw.from = from_co; }
		Tw.callWhenFinished = cbstr;
		Tw.eventReceiver = rece;
		Tw.style = style;
		BegainTween(Tw);
	}
	
	/// <summary>
	/// 颜色效果
	/// </summary>
	public void TweenColorEffect(GameObject go,float duration,Color from_co,Color to_co,bool isfrom = false,UITweener.Style style = UITweener.Style.Once)
	{
		if(go == null || !go.activeInHierarchy)
			return;
		
		TweenColor Tw = TweenColor.Begin(go,duration,to_co);
		if(isfrom){ Tw.from = from_co; }
		Tw.style = style;
		BegainTween(Tw);
	}

	/// <summary>
	/// 位移效果
	/// </summary>
	public void TweenPositionEffect(GameObject go,GameObject rece,string cbstr,float duration,Vector3 from_pos,Vector3 to_pos,bool isfrom = false,float delaytime = 0.0f)
	{
		if(go == null || !go.activeInHierarchy)
			return;

		TweenPosition Tw = TweenPosition.Begin(go,duration,to_pos);
		if(isfrom){ Tw.from = from_pos; }
		Tw.callWhenFinished = cbstr;
		Tw.eventReceiver = rece;
		Tw.delay = delaytime;
		BegainTween(Tw);
	}

	/// <summary>
	/// 位移效果
	/// </summary>
	public void TweenPositionEffect(GameObject go,float duration,Vector3 from_pos,Vector3 to_pos,bool isfrom = false,float delaytime = 0.0f)
	{
		if(go == null || !go.activeInHierarchy)
			return;
		
		TweenPosition Tw = TweenPosition.Begin(go,duration,to_pos);
		if(isfrom){ Tw.from = from_pos; }
		Tw.delay = delaytime;
		BegainTween(Tw);
	}

	/// <summary>
	/// 旋转效果
	/// </summary>
    public void TweenRotationEffect(GameObject go, float duration, Quaternion from_rot, Quaternion to_rot, bool isfrom = false, float delaytime = 0.0f)
	{
		if(go == null || !go.activeInHierarchy)
			return;
		
		TweenRotation Tw = TweenRotation.Begin(go,duration,to_rot);
		if(isfrom){ Tw.from = from_rot.eulerAngles; }
		Tw.delay = delaytime;
		BegainTween(Tw);
	}

    /// <summary>
    /// 旋转效果
    /// </summary>
    public void TweenRotationEffect(GameObject go, float duration, Vector3 from_rot, Vector3 to_rot, float delaytime = 0.0f,UITweener.Style style = UITweener.Style.Once)
    {
        if (go == null)
            return;

        TweenRotation tr = go.GetComponent<TweenRotation>();
        if(tr == null)tr = go.AddComponent<TweenRotation>();

        tr.from = from_rot;
        tr.to = to_rot;
        tr.duration = duration;
        tr.delay = delaytime;
        tr.style = style;
        
        tr.PlayForward();
    }

	void BegainTween(UITweener Tw)
	{
		Tw.ignoreTimeScale = false;
		Tw.ResetToBeginning();
		Tw.PlayForward();
	}

    /// <summary>
    /// 动画停止
    /// </summary>
    /// <param name="go"></param>
    public void TweenStop(GameObject go)
    {
        if (go == null) return;

        UITweener[] tweener = go.GetComponents<UITweener>();
        if (tweener != null && tweener.Length > 0)
        {
            for (int i = 0; i < tweener.Length; i++)
            {
                tweener[i].enabled = false;
            }
        }
    }
}
