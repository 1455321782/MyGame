using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFacade : PureMVC.Patterns.Facade
{
    public const string START_UP = "start_up";//��ʼ��Ϸ
    public const string INIT_DATA = "init_data";//��ʼ������
    public const string SHOW_PANLE = "show_panle";//��ʾ�������
    static MyFacade()
    {
        m_instance = new MyFacade();
    }
    public static MyFacade GetInstance()
    {
        return m_instance as MyFacade;
    }

    public void Launch()
    {
        SendNotification(MyFacade.START_UP);
    }
    protected override void InitializeController()
    {
        base.InitializeController();
        RegisterCommand(START_UP,typeof(StartUpCommand));
    }

    protected override void InitializeModel()
    {
        base.InitializeModel();
        RegisterProxy(new MainDataProxy());
    }

    protected override void InitializeView()
    {
        base.InitializeView();
    }

    protected override void InitializeFacade()
    {
        base.InitializeFacade();
    }
}
