using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    public Button buy;
    public Text gold;
    private float number = 1000;
    public Transform parent;
    [HideInInspector]
    public List<ItemView> items;

    public Transform bagParent;
    public void UpDatePanle(float price)
    {
        number -= price;
        gold.text ="Ç®°ü£º"+ (number).ToString();
    }
}
