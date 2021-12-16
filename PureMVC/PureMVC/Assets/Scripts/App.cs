using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{

    private void Awake()
    {
        MyFacade.GetInstance().Launch();
    }

}
