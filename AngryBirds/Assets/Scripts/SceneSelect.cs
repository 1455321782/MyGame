using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelect : MonoBehaviour
{
    [SerializeField]private int unlockStar = 0;//������Ҫ������
    public GameObject sceneLock;
    public GameObject stars;
    private bool isSelect;//�����Ƿ��ѡ
    public GameObject scene;//��Ӧ�ĳ����ؿ�
    public GameObject parent;
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("StarsNumber",0)>=unlockStar)
        {
            isSelect = true;//��ѡ״̬
        }

        if (isSelect)//��ѡ ������ ��������
        {
            sceneLock.SetActive(false);
            stars.SetActive(true);
        }
    }
    /// <summary>
    /// ѡ��˳���
    /// </summary>
    public void SelectScene()
    {
        if (isSelect)//��ѡ ���س���ѡ�� �򿪹ؿ�ѡ��
        {
            scene.SetActive(true);
            parent.SetActive(false);
        }
    }
}
