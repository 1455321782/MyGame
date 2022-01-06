using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    private int n = 1;
    public List<Image> lecels = new List<Image>();
    public Sprite lockSprite;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        Init();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        for (var i = 0; i < lecels.Count; i++)
        {
            lecels[i].name = n.ToString();
            lecels[i].GetComponentInChildren<Text>().text = n.ToString();
            n += 1;
            if (i != 0)
            {
                lecels[i].sprite = lockSprite;
                lecels[i].transform.Find("Star").gameObject.SetActive(false);
                lecels[i].transform.Find("Text").gameObject.SetActive(false);
            }
        }
    }
}
