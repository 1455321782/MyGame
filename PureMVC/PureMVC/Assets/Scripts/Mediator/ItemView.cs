using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    public Text theName;
    public Text thePrice;
    public Image theIcon;
    public int index;

    public void UpData_Item(Good good)
    {
        theName.text = good.name;
        thePrice.text = good.price.ToString();
        theIcon.sprite = Resources.Load<Sprite>((good.icon-1).ToString());
        index = good.index;
    }
}
