using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        MessageManager.Instance.OnAddListne(MessageID.C_Test,AAA);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AAA(object obj)
    {
        Debug.Log(" ’µΩ");
    }
}
