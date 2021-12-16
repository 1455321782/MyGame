using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class MainMediator : PureMVC.Patterns.Mediator
{
    public new const string NAME = "MainMediator";
    public MainView mainView;
    public MainDataProxy mainDataProxy;
    private int index;
    public MainMediator(object viewComponent) : base(NAME, viewComponent)
    {
        mainView = ((GameObject) ViewComponent).GetComponent<MainView>();
        mainDataProxy = Facade.RetrieveProxy(MainDataProxy.NAME) as MainDataProxy;
        //¹ºÂò°´Å¥
        mainView.buy.onClick.AddListener((() =>
        {
            Debug.Log("¹ºÂòÁË"+mainDataProxy.mainData.goods[index].name+"£º"+mainDataProxy.mainData.goods[index].price+"Ôª");
            mainView.UpDatePanle(mainDataProxy.mainData.goods[index].price);
            AddItem();
        }));
    }

    void AddItem()
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Dish"), mainView.bagParent);
        obj.GetComponent<Text>().text = mainDataProxy.mainData.goods[index].name;
    }
    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>() { MyFacade.INIT_DATA};
        return list;
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case MyFacade.INIT_DATA:
                mainView.items = new List<ItemView>();
                foreach (var good in mainDataProxy.mainData.goods)
                {
                    GameObject obj = Object.Instantiate(Resources.Load<GameObject>("Item"), mainView.parent);
                    var item = obj.GetComponent<ItemView>();
                    item.UpData_Item(good);
                    mainView.items.Add(item);

                    var button = obj.GetComponent<Button>();
                    button.onClick.AddListener((() =>
                    {
                        index = good.index;
                    }));

                    //Facade.RegisterMediator(new ItemMediator(obj));
                }
                
                break;;
        }
    }
}
