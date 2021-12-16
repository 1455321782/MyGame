using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        ClientManager.Instance.Init();

        
    }

    // Update is called once per frame
    void Update()
    {
        ClientManager.Instance.UpDate();
    }
}
