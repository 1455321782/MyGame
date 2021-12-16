using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class MainDataProxy : PureMVC.Patterns.Proxy
{
    public new const string NAME = "MainDataProxy";
    public MainDataModel mainData;

    public MainDataProxy() : base(NAME, null)
    {
        mainData = new MainDataModel();
    }
    //todo 方法 

    public override void OnRegister()
    {
        //初始化面板内容
        string json = File.ReadAllText("Assets/Scripts/Json.json");
        mainData.goods = JsonConvert.DeserializeObject<List<Good>>(json);
    }
}
