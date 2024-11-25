using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawResetter : MonoBehaviour
{
    [SerializeField] private HingeJoint2D _hingeJoint;
    [SerializeField] private float wiggleRoom = 8f;



    private void Update()
    {
        if (_hingeJoint.attachedRigidbody.rotation > _hingeJoint.limits.max + wiggleRoom)
        {

            Debug.Log(gameObject.name + " reset to Max Rotation\nInitial Angle: " + _hingeJoint.attachedRigidbody.rotation);
            //_hingeJoint.attachedRigidbody.rotation = _hingeJoint.limits.max;
        }

        else if (_hingeJoint.attachedRigidbody.rotation < _hingeJoint.limits.min - wiggleRoom)
        {
            //_hingeJoint.attachedRigidbody.rotation = _hingeJoint.limits.min;
            Debug.Log(gameObject.name + " reset to Min Rotation\nInitial Angle: " + _hingeJoint.attachedRigidbody.rotation);
        }
    }
}
