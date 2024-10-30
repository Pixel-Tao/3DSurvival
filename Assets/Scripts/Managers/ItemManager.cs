using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemManager : ManagerSingleton<ItemManager>
{
    Dictionary<int, ItemData> itemDict = new Dictionary<int, ItemData>();

    public void InitData()
    {
        string[] files = System.IO.Directory.GetFiles("Assets/Scripts/ScriptableObjects/Data", "*.asset");
        foreach (string file in files)
        {
            ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(file);
            if (itemData != null)
            {
                itemDict.Add(itemData.id, itemData);
            }
        }
    }

    public GameObject LoadItem(int id, Vector3 position, Transform parent = null)
    {
        if (itemDict.TryGetValue(id, out ItemData itemData))
        {
            GameObject go = Instantiate(itemData.dropPrefab, position, Quaternion.identity);
            go.transform.SetParent(parent);
            return go;
        }

        return null;
    }
}
