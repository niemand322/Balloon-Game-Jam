using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightswitch : MonoBehaviour
{

    //Scale this to the size of your room
    public GameObject darkness;

    SpriteRenderer darknessSprite;
    // Start is called before the first frame update
    void Start()
    {
        darknessSprite = darkness.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        darknessSprite.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        darkness.transform.position = new Vector3(1, 2, 7);
    }
}
