using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<BirdControl> birds = new List<BirdControl>();
    public List<PigHurtControl> pigs = new List<PigHurtControl>();
    public List<Image> stars = new List<Image>();
    private Vector3 birdPos;
    private int number = 0;//�ܷ�
    public GameObject lose;//ʧ��
    public GameObject win;//ʤ��
    public Text NumText;//������ʾ
    void Awake()
    {
        Time.timeScale = 1;
        instance = this;
        if (birds.Count > 0) birdPos = birds[0].transform.position;
    }

    void Start()
    {
        Init();
    }

    /// <summary>
    /// �����ܷ�
    /// </summary>
    /// <param name="n"></param>
    public void AddNum(int n)
    {
        number += n;
        NumText.text = number.ToString();
    }
    /// <summary>
    /// ���Ǳ仯
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowStar()
    {
        int n = 1;
        if (number >= 30000)
        {
            n = 3;
        }
        else if (number >= 20000)
        {
            n = 2;
        }
        else
        {
            n = 1;
        }
        for (int i = 1; i <= n; i++)
        {
            yield return new WaitForSeconds(0.8f);
            stars[i - 1].gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// ��ʼ����
    /// </summary>
    void Init()
    {
        for (var i = 0; i < birds.Count; i++)
        {
            if (i == 0)
            {
                birds[i].gameObject.transform.position = birdPos;
                birds[i].enabled = true;
                birds[i].springJoint.enabled = true;
            }
            else
            {
                birds[i].enabled = false;
                birds[i].springJoint.enabled = false;
            }
        }
    }
    /// <summary>
    /// ʤ������
    /// </summary>
    public void WinCondition()
    {
        if (pigs.Count > 0)
        {
            //��Ϸ����
            if (birds.Count > 0)
            {
                //�����񣬿��Լ���
                Init();
            }
            else
            {
                //û���ˣ���Ϸʧ��
                lose.SetActive(true);
            }
        }
        else
        {
            //��Ϸʤ��
            win.SetActive(true);
            StartCoroutine(ShowStar());
        }
    }
    /// <summary>
    /// ��ͣ��
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// ������
    /// </summary>
    public void Continue()
    {
        Time.timeScale = 1;
    }
    /// <summary>
    /// ���¿�ʼ��
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(2);
    }
    /// <summary>
    /// ���ز˵���
    /// </summary>
    public void Menu()
    {
        SceneManager.LoadScene(1);
    }
}
