using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDFPS : MonoBehaviour
{
	public float updateInterval = 0.5F;

	private float accum = 0; // FPS�ڼ��ʱ�����ۻ�
	private int frames = 0; // �ڼ���ϻ��Ƶ�֡
	private float timeleft; // ��ǰ�����ʣ��ʱ��
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

		//�������-����GUI�ı�����ʼ�µļ��
		if (timeleft <= 0.0)
		{
			// ��ʾ��λС��(f2��ʽ)
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