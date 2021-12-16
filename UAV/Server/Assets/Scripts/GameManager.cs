using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        NetManager.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
