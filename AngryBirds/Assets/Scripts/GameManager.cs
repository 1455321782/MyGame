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
    private int number = 0;//总分
    public GameObject lose;//失败
    public GameObject win;//胜利
    public Text NumText;//分数显示
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
    /// 计算总分
    /// </summary>
    /// <param name="n"></param>
    public void AddNum(int n)
    {
        number += n;
        NumText.text = number.ToString();
    }
    /// <summary>
    /// 星星变化
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
    /// 初始化鸟
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
    /// 胜利条件
    /// </summary>
    public void WinCondition()
    {
        if (pigs.Count > 0)
        {
            //游戏继续
            if (birds.Count > 0)
            {
                //还有鸟，可以继续
                Init();
            }
            else
            {
                //没鸟了，游戏失败
                lose.SetActive(true);
            }
        }
        else
        {
            //游戏胜利
            win.SetActive(true);
            StartCoroutine(ShowStar());
        }
    }
    /// <summary>
    /// 暂停键
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// 继续键
    /// </summary>
    public void Continue()
    {
        Time.timeScale = 1;
    }
    /// <summary>
    /// 重新开始键
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(2);
    }
    /// <summary>
    /// 返回菜单键
    /// </summary>
    public void Menu()
    {
        SceneManager.LoadScene(1);
    }
}
