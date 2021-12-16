using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class StartUpCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        //打开主面板
        GameObject obj = Object.Instantiate(Resources.Load<GameObject>("BackGround"), GameObject.Find("Canvas").transform);
        Facade.RegisterMediator(new MainMediator(obj));
        //发消息显示面板内容
        SendNotification(MyFacade.INIT_DATA);
    }
}
