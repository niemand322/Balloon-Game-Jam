using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Newspaper : MonoBehaviour
{
    public GameObject newspaper;
    public GameObject NPC_Container;
    public GameObject Player_Container;

    public void News(string news)
    {
        NPC_Container.SetActive(false);
        Player_Container.SetActive(false);
        Instantiate(newspaper, Vector3.zero, Quaternion.identity);
    }
}
