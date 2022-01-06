using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelect : MonoBehaviour
{
    [SerializeField]private int unlockStar = 0;//解锁需要的星星
    public GameObject sceneLock;
    public GameObject stars;
    private bool isSelect;//场景是否可选
    public GameObject scene;//对应的场景关卡
    public GameObject parent;
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("StarsNumber",0)>=unlockStar)
        {
            isSelect = true;//可选状态
        }

        if (isSelect)//可选 隐藏锁 打开星星数
        {
            sceneLock.SetActive(false);
            stars.SetActive(true);
        }
    }
    /// <summary>
    /// 选择此场景
    /// </summary>
    public void SelectScene()
    {
        if (isSelect)//可选 隐藏场景选择 打开关卡选择
        {
            scene.SetActive(true);
            parent.SetActive(false);
        }
    }
}
