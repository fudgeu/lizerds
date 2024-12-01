using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointResetter : MonoBehaviour
{
    [SerializeField] private BodyAnimation bodyAnimation;
    private Vector3 localStartPosition;
    private float startRotation;
    [SerializeField] private bool isHand = false;
    private int troubleCounter = 0;

    private void Start()
    {
        localStartPosition = transform.localPosition;
        startRotation = transform.rotation.z;
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        Debug.Log("A joint has just been broken: " + gameObject.name);
        Debug.Log("The broken joint exerted a reaction force of " + joint.reactionForce);
        Debug.Log("The broken joint exerted a reaction torque of " + joint.reactionTorque);
        troubleCounter++;
        if (troubleCounter == 10)
        {
            {
                troubleCounter = 0;

                if (isHand)
                {
                    bodyAnimation.ReleaseLegs();
                    return;
                }

                if (bodyAnimation.flippedX)
                {
                    joint.attachedRigidbody.position = transform.TransformPoint(new Vector3(-localStartPosition.x, localStartPosition.y, localStartPosition.z));
                }
                else
                    joint.attachedRigidbody.position = transform.TransformPoint(localStartPosition);

                joint.attachedRigidbody.rotation = startRotation;
            }
        }
    }
}
