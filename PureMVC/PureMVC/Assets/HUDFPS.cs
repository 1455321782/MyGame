using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDFPS : MonoBehaviour
{
	public float updateInterval = 0.5F;

	private float accum = 0; // FPS在间隔时间内累积
	private int frames = 0; // 在间隔上绘制的帧
	private float timeleft; // 当前间隔的剩余时间
	[SerializeField]private Text guiText;
	void Start()
	{
		if (!guiText)
		{
			Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
			enabled = false;
			return;
		}
		timeleft = updateInterval;
	}

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		//间隔结束-更新GUI文本并开始新的间隔
		if (timeleft <= 0.0)
		{
			// 显示两位小数(f2格式)
			float fps = accum / frames;
			string format = string.Format("{0:F2} FPS", fps);
			guiText.text = format;

			if (fps < 30)
				guiText.material.color = Color.yellow;
			else if (fps < 10)
				guiText.material.color = Color.red;
			else
				guiText.material.color = Color.green;
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}