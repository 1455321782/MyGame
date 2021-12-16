using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        MessageManager.Instance.OnAddListne(MessageID.S_Test,AAA);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            string str = "123";
            ClientManager.Instance.OnSendTo(MessageID.C_Test, System.Text.UTF8Encoding.UTF8.GetBytes(str));
            Debug.Log("·¢ËÍ");
        }*/
    }

    void AAA(object obj)
    {
        object[] o = obj as object[];
        byte[] bytes = o[0] as byte[];
        string str = UTF8Encoding.UTF8.GetString(bytes);
        Debug.Log(str);
    }
}
