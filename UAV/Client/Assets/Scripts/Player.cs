using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using MyPlane;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        MessageManager.Instance.OnAddListne(MessageID.S_PlaneMove,PlaneMove);
        MessageManager.Instance.OnAddListne(MessageID.S_AirDrop,AirDrop);
    }

    void AirDrop(object obj)
    {
        Debug.Log("-------------�ӿ�Ͷ");
        GameObject box = Instantiate(Resources.Load<GameObject>("Box"));
        box.transform.position = transform.position;
    }
    void PlaneMove(object obj)
    {
        object[] objects = obj as object[];
        byte[] bytes = objects[0] as byte[];
        AirPlane airPlane = AirPlane.Parser.ParseFrom(bytes);
        CalibrationPlane(airPlane.Px,airPlane.Py,airPlane.Pz,airPlane.Rx,airPlane.Ry,airPlane.Rz);
    }
    /// <summary>
    /// У׼�ɻ�λ��
    /// </summary>
    /// <param name="px"></param>
    /// <param name="py"></param>
    /// <param name="pz"></param>
    /// <param name="rx"></param>
    /// <param name="ry"></param>
    /// <param name="rz"></param>
    void CalibrationPlane(float px,float py,float pz,float rx,float ry,float rz)
    {
        transform.position = new Vector3(px, py, pz);
        transform.eulerAngles = new Vector3(rx, ry, rz);
        Debug.Log("����λ�ã�"+new Vector3(px, py, pz));
        Debug.Log("���½Ƕȣ�"+new Vector3(rx, ry, rz));
    }
    void Update()
    {
        
    }
}
