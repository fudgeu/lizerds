using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSaw : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;

    void Update()
    {
        

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
