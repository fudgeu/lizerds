using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSaw : MonoBehaviour
{
    //Component Field to attach to object
    [SerializeField] private new Rigidbody2D rigidbody;

    void Update()
    {
        //limits the rotation of the object
        if (rigidbody.rotation > 60)
        {
            rigidbody.MoveRotation(60);
        }
        else if (rigidbody.rotation < -60)
        {
            rigidbody.MoveRotation(-60);
        }
    }
}
