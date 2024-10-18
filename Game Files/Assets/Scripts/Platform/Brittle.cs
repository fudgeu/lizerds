using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brittle : MonoBehaviour
{
    //Component Field to attach to object
    [SerializeField] private new Rigidbody2D rigidbody;
    //[SerializeField] private new Collider Collider;

    private void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("Do something here");
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "Player")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Do something here");
            rigidbody.constraints = RigidbodyConstraints2D.None;
        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Player")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Do something else here");
        }
    }
}
