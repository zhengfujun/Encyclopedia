using UnityEngine;
using System.Collections;

public class UI_Wait : MonoBehaviour
{
	public GameObject RoundImg = null;

	float DelayTime = 0f;
	float DurationTime = 0f;

	void OnEnable()
	{
		RoundImg.SetActive(false);
		DurationTime = 5.0f;
		DelayTime = 0.5f;
	}

	void Update()
	{
		DelayTime -= Time.deltaTime;
		if(DelayTime > 0)
			return;
        if (RoundImg.activeSelf == false) {
            RoundImg.SetActive(true);
        }
		DurationTime -= Time.deltaTime;
		if(DurationTime > 0)
			return;
        GameApp.SendMsg.EndWaitUI();
	}
}
