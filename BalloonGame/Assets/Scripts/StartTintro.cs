using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class StartTintro : MonoBehaviour
{
    public UIManager uimanager;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (!VD.isActive)
            {
                uimanager.Begin(GetComponent<VIDE_Assign>());
            }
        }
    }
}
