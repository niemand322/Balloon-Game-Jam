using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{

    public GameObject picture;
    public void DeleteGameObject(string tag)
    {
        Destroy(GameObject.FindGameObjectWithTag(tag));
    }

    public void ShowPicture(bool active)
    {
        picture.SetActive(active);
    }
}
