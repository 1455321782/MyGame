using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : Singleton<MessageManager>
{
    private Dictionary<int, Action<object>> messages = new Dictionary<int, Action<object>>();

    public void OnAddListne(int id, Action<object> action)
    {
        if (messages.ContainsKey(id))
        {
            messages[id] += action;
        }
        else
        {
            messages.Add(id,action);
        }
    }

    public void OnSend(int id, params object[] data)
    {
        if (messages.ContainsKey(id))
        {
            messages[id](data);
        }
    }
}
