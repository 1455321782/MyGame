using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class ItemMediator : PureMVC.Patterns.Mediator
{
    public new const string NAME = "ItemMediator";
    public ItemView itemView;
    public ItemDataProxy itemData;

    public ItemMediator(object viewComponent) : base(NAME, viewComponent)
    {
        itemView = ((GameObject) ViewComponent).GetComponent<ItemView>();
        itemData = Facade.RetrieveProxy(ItemDataProxy.NAME)as ItemDataProxy;
        //todo 点击事件？
        itemView.gameObject.GetComponent<Button>().onClick.AddListener((() =>
        {
            Debug.Log("选中了"+itemData.itemData.good.name);
        }));
    }

    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>() { };
        return list;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            
        }
    }
}
