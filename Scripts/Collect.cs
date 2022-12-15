using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject star;
    public Collider myCollider;
    void Start()
    {
        myCollider = gameObject.GetComponent<Collider>();
        gameObject.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        OnTriggerEnter(myCollider);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "star")
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            Debug.Log("Nothing happened");
        }
    }
}
