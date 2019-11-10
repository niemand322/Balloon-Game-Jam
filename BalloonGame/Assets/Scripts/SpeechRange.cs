using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class SpeechRange : MonoBehaviour
{

    public UIManager uimanager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!VD.isActive)
                {
                    uimanager.Begin(GetComponent<VIDE_Assign>());
                }
            }
        }
    }
}
