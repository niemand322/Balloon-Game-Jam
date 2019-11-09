using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;

public class Inventory : MonoBehaviour
{
    public Image bigPicture;
    public Sprite sprite;

    private void Start()
    {
        bigPicture.gameObject.SetActive(false);
    }

    public void ShowItem(string s) // The function must accept a variable so that it can be called by the action node. This variable can be used to find the right object in the inventory.
    {
        // I wrote this function only to test the display of objects. But it should search for the sprite from the inventory.

        VD.OnNodeChange += HideItem; 
        bigPicture.gameObject.SetActive(true); // I register HideItem at OnNodeChange, so that the picture will be hidden again at the next "Enter".
        bigPicture.sprite = sprite;
    }

    void HideItem(VD.NodeData data)
    {
        bigPicture.gameObject.SetActive(false);
        VD.OnNodeChange -= HideItem; // HideItem must be unsubscribed again so that it is no longer called at each following "Enter
    }
}
