using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataProxy : PureMVC.Patterns.Proxy
{
    public new const string NAME = "ItemDataProxy";
    public ItemDataModel itemData;

    public ItemDataProxy() : base(NAME, null)
    {
        itemData = new ItemDataModel();
    }

}
