using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class EditorExtension : MonoBehaviour
{
    [InitializeOnLoadMethod]
    static void InitializeOnLoad()
    {
        #region 删除所选Object
        /*EditorApplication.projectWindowItemOnGUI += ((guid, rect) =>
        {
            if (Selection.activeObject != null)
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);
                string actiove_guid = AssetDatabase.AssetPathToGUID(path);
                if (actiove_guid == guid && !string.IsNullOrEmpty(actiove_guid))
                {
                    rect.x = rect.width - 50;
                    rect.width = 50;
                    if (GUI.Button(rect, "Del"))
                    {
                        AssetDatabase.DeleteAsset(path);
                    }
                }
            }
        }); */
        #endregion
    }
    [MenuItem("Tools/创建文件夹")]
    static void CreateDirectory()
    {
        string[] folder_name = { "Resources","Scripts","Materials","Util"};
        foreach (var item in folder_name)
        {
            string path = Application.dataPath+"/"+item;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        Color[] colors = {Color.black, Color.blue, Color.green, Color.red, Color.yellow, Color.gray};
        string[] names = {"black", "blue", "green", "red", "yellow", "gray"};
        for (int i = 0; i < colors.Length; i++)
        {
            var material = new Material(Shader.Find("Standard"));
            material.color = colors[i];
            AssetDatabase.CreateAsset(material, "Assets/Materials/" + names[i] + ".mat");
        }
        AssetDatabase.Refresh();
    }
}
