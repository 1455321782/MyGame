using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using UnityEditor.SceneManagement;

public class ItemCube
{
    public string item_name;
    public string pos;
    public string scale;
    public string ang;
}
public class MyWindow : EditorWindow
{
    private string mapName;
    private string importMap;
    private List<ItemCube> cubes;
    private Vector3Int itemVector3;
    private Transform root;
    [MenuItem("Tools/�ؿ��༭",false,0)]
    static void Window()
    {
        GetWindow<MyWindow>().Show();
    }

    private void OnGUI()
    {
        #region ������ͼ
        itemVector3 = EditorGUILayout.Vector3IntField("����λ�ã�", itemVector3);
        if (GUILayout.Button("��")) { CreatItem("1001"); }
        if (GUILayout.Button("��")) { CreatItem("1002"); }
        if (GUILayout.Button("��")) { CreatItem("1003"); }
        if (GUILayout.Button("��")) { CreatItem("1004"); }
        if (GUILayout.Button("��")) { CreatItem("1005"); }
        if (GUILayout.Button("��")) { CreatItem("1006"); }
        GUILayout.Space(10);
        if (GUILayout.Button("�����ͼ"))
        {
            /*foreach (Transform item in root)
            {
                //Undo.DestroyObjectImmediate(item.gameObject);
                GameObject game = item.gameObject;
                DestroyImmediate(game);
            }*/
            Undo.DestroyObjectImmediate(root.gameObject);
            GameObject rootPoint = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("MapRoot")) as GameObject;
            root = rootPoint.transform;
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }
        #endregion

        #region �����ͼ
        GUILayout.Space(10);
        GUILayout.Label("�����ͼ���ƣ�");
        mapName = GUILayout.TextField(mapName);
        if (GUILayout.Button("�����ͼ"))
        {
            foreach (Transform item in root)
            {
                string item_name = item.name;
                string pos = GetVector3ToString(item.localPosition);
                string scale = GetVector3ToString(item.localScale);
                string ang = GetVector3ToString(item.localEulerAngles);

                cubes.Add(new ItemCube(){item_name = item_name,pos = pos,scale = scale,ang = ang});
            }

            string json =  JsonConvert.SerializeObject(cubes);
            File.AppendAllText("Assets/Maps/"+mapName+".json",json);
            AssetDatabase.Refresh();
            cubes.Clear();
            Debug.Log("����ɹ�");
        }
        #endregion

        #region �����ͼ
        GUILayout.Space(10);
        GUILayout.Label("�����ͼ���ƣ�");
        importMap = GUILayout.TextField(importMap);
        if (GUILayout.Button("�����ͼ"))
        {
            string json = File.ReadAllText("Assets/Maps/" + importMap + ".json");
            var items = JsonConvert.DeserializeObject<List<ItemCube>>(json);
            foreach (var item in items)
            {
                GameObject game = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>(item.item_name)) as GameObject;
                game.transform.parent = root;
                game.transform.localScale = GetStringToVector3(item.scale);
                game.transform.localEulerAngles = GetStringToVector3(item.ang);
                game.transform.localPosition = GetStringToVector3(item.pos);
            }
        }
        #endregion
    }

    Vector3 GetStringToVector3(string str)
    {
        var v = str.Split(',');
        return new Vector3(float.Parse(v[0]), float.Parse(v[1]), float.Parse(v[2]));
    }
    /// <summary>
    /// ��Vector3ר��String
    /// </summary>
    /// <param name="v3"></param>
    /// <returns></returns>
    string GetVector3ToString(Vector3 v3)
    {
        string str = string.Format("{0:F},{1:F},{2:F}", v3.x, v3.y, v3.z);
        return str;
    }
    /// <summary>
    /// ����Item
    /// </summary>
    /// <param name="item"></param>
    void CreatItem(string item)
    {
        var obj = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>(item));
        GameObject gameObject = obj as GameObject;
        gameObject.transform.parent = root;
        gameObject.transform.position = itemVector3;
        gameObject.transform.eulerAngles = Vector3.zero;
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }

    private void OnEnable()
    {
        root = GameObject.Find("MapRoot").transform;
        cubes = new List<ItemCube>();
        GetMapRootChil();
    }

    void GetMapRootChil()
    {

    }
}
