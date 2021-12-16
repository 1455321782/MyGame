using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class StartUpCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        //�������
        GameObject obj = Object.Instantiate(Resources.Load<GameObject>("BackGround"), GameObject.Find("Canvas").transform);
        Facade.RegisterMediator(new MainMediator(obj));
        //����Ϣ��ʾ�������
        SendNotification(MyFacade.INIT_DATA);
    }
}
