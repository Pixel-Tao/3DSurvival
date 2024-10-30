using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScene : MonoBehaviour
{
    public Transform itemParent;

    void Start()
    {
        ItemManager.Instance.InitData();

        UIManager.Instance.Open<UIGeneral>();

        UIManager.Instance.Open<UIInventory>();
        UIManager.Instance.Close<UIInventory>();

        ItemManager.Instance.LoadItem(101, itemParent.position, itemParent);
        ItemManager.Instance.LoadItem(102, itemParent.position, itemParent);
        ItemManager.Instance.LoadItem(103, itemParent.position, itemParent);
        ItemManager.Instance.LoadItem(104, itemParent.position, itemParent);
        ItemManager.Instance.LoadItem(105, itemParent.position, itemParent);
        ItemManager.Instance.LoadItem(106, itemParent.position, itemParent);
    }

}
