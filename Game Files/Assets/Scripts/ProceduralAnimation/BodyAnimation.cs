using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BodyAnimation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementTargetController movementTargetController;     //As I mentioned in NewPlayerContorller, this here is bad software architecture, but i think 'spaghetti code' is a LIBERAL LIE.
    [SerializeField] private DistanceJoint2D jawJoint;
    [SerializeField] private Transform root;
    //[SerializeField] private Transform head;
    [SerializeField] private Transform movementTarget;
    [SerializeField] private List<FootPositioner> limbTargets;
    [SerializeField] private List<Transform> transformsToBeRotated;
    [SerializeField] private Transform targetToBeRotatedAround;

    [Header("Configuration")]
    [SerializeField] private float openJawDistance = 1;
    [SerializeField] private float waitAfterLeapToSearchForGround = 1f;

    public bool flippedX = false;
    private bool legsReleased = false;
    private float closeJawDistance;

    [Header("Hard-Coded Head Rigidbodies")]
    [SerializeField] private Transform headJoint;
    [SerializeField] private Transform topJawJoint;
    [SerializeField] private Transform bottomJawJoint;
    private float savedHeadRotation;
    private float savedTopJawRotation;
    private float savedBottomJawRotation;


    private void Start()
    {
        closeJawDistance = jawJoint.distance;
        
        //Hard coding the head transforms so they don't break on flip
        savedHeadRotation = headJoint.rotation.z;
        savedBottomJawRotation = bottomJawJoint.rotation.z;
        savedTopJawRotation = topJawJoint.rotation.z;
    }

    #region Leg Control Systems
    //-----------Releases foot placement system on the legs--------------
    public void ReleaseLegs()
    {

        if (legsReleased) return;

        Debug.Log("Releasing Legs.");
        legsReleased = true;
        foreach (FootPositioner footPositioner in limbTargets)
        {
            footPositioner.LegPositioning = false;
            footPositioner.ReleaseFoot();
        }
        StartCoroutine(waitForLanding());
    }

    private IEnumerator waitForLanding()
    {
        yield return new WaitForSeconds(waitAfterLeapToSearchForGround);
        Debug.Log("Checking for ground...");
        while (!movementTargetController.isGrounded || movementTargetController.backCheck.position.y < movementTargetController.groundCheck.position.y)
        {
            yield return new WaitForEndOfFrame();
        }
        EnableLegs();
    }

    //------------Reenabled foot placement system------------------------
    public void EnableLegs()
    {
        Debug.Log("Enabling Legs.");
        foreach (FootPositioner footPositioner in limbTargets)
        {
            footPositioner.LegPositioning = true;
            footPositioner.EnableFoot();
        }
        legsReleased = false;
    }
    #endregion

    #region Mouth Control Systems

    //------------Opens Mouth---------------
    public void OpenMouth()
    {
        jawJoint.distance = openJawDistance;
    }

    //-----------Closes Mouth------------
    public void CloseMouth()
    {
        jawJoint.distance = closeJawDistance;
    }
    #endregion

    #region Turning System
    //--------Flips the Lizerd on the X---------
    public void FlipX()
    {
        StartCoroutine(FlipCoroutine());
    }

    private IEnumerator FlipCoroutine()
    {
        flippedX = !flippedX;


        foreach (FootPositioner footPositioner in limbTargets)
        {
            footPositioner.ReleaseFoot();
        }

        FlipLimbs();

        foreach (Transform t in transformsToBeRotated)                                                 //<--Can take multiple transforms, if we need to experiment.
        {
            t.position += new Vector3(0, 1f, 0);
            t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
        }

        //FlipHingeLimits(root.gameObject);
        //FlipHingeAnchors(root.gameObject);                                                            //<-- Legacy system, kinda overcomplicates the turning. Feel free to play with enabling these, Physics is finniky.
        //FlipRigidbodyVelocities(root.gameObject);

        yield return new WaitForEndOfFrame();

        ReleaseLegs();
        //Reenable the legs
        //foreach (FootPositioner footPositioner in limbTargets)
        //{
        //    footPositioner.EnableFoot();
        //}

        //JointAngleLimits2D limits = topJawJoint.gameObject.GetComponent<HingeJoint2D>().limits;
        //limits.max = -limits.max;
        //limits.min = -limits.min;
        //topJawJoint.gameObject.GetComponent<HingeJoint2D>().limits = limits;                                                                          //<---------The solution to head breaking is somewhere in here...

        //limits = bottomJawJoint.gameObject.GetComponent<HingeJoint2D>().limits;
        //limits.max = -limits.max;
        //limits.min = -limits.min;
        //bottomJawJoint.gameObject.GetComponent<HingeJoint2D>().limits = limits;

        //headJoint.rotation = new Quaternion(headJoint.rotation.x, headJoint.rotation.y, savedHeadRotation, headJoint.rotation.w);
        topJawJoint.rotation = new Quaternion(topJawJoint.rotation.x, topJawJoint.rotation.y, savedTopJawRotation, topJawJoint.rotation.w);
        bottomJawJoint.rotation = new Quaternion(bottomJawJoint.rotation.x, bottomJawJoint.rotation.y, savedBottomJawRotation, bottomJawJoint.rotation.w);
    }

    //--------------All of the following functions are used for the FlipX() function--------------------
    /// <summary>
    /// Flips which limbs are set to left and which are set to right. Used in foot positioning offset.
    /// </summary>
    private void FlipLimbs()
    {
        foreach (FootPositioner limb in limbTargets)
        {
            if (limb.gameObject.activeSelf)
                limb.isOnLeft = !limb.isOnLeft;
        }
    }

    /// <summary>
    /// Reverses the limits of all hinge joints, effectively mirroring them.
    /// </summary>
    /// <param name="bone"></param>
    private void FlipHingeLimits(GameObject bone)
    {
        if (bone.TryGetComponent<HingeJoint2D>(out HingeJoint2D hinge))
        {
            if (hinge.useLimits)
            {
                JointAngleLimits2D limits = hinge.limits;
                limits.max = -limits.max;
                limits.min = -limits.min;
                //if (flippedX)
                //{
                //    limits.max = limits.max - 180;
                //    limits.min = limits.min - 180;
                //}
                //else
                //{
                //    limits.max = limits.max + 180;
                //    limits.min = limits.min + 180;
                //}


                hinge.limits = limits;
            }
        }

        for (int i = 0; i < bone.transform.childCount; i++)
        {
            FlipHingeLimits(bone.transform.GetChild(i).gameObject);
        }
    }

    //Full disclosure, Chat GPT wrote the following two functions. Sue me.
    /// <summary>
    /// Flips all velocities effecting the system on the X axis
    /// </summary>
    private void FlipRigidbodyVelocities(GameObject bone)
    {
        // Get the Rigidbody2D component of the current bone and flip its velocity if it exists
        Rigidbody2D rb = bone.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 velocity = rb.velocity;
            rb.velocity = new Vector2(-velocity.x, velocity.y);  // Flip the X velocity to maintain the correct movement direction
        }

        // Recursively call FlipRigidbodyVelocities on all child objects of the current bone
        for (int i = 0; i < bone.transform.childCount; i++)
        {
            FlipRigidbodyVelocities(bone.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Flips the offset of HingeAnchors, preventing squeezing
    /// </summary>
    private void FlipHingeAnchors(GameObject bone)
    {
        if (bone.TryGetComponent<HingeJoint2D>(out HingeJoint2D hinge))
        {
            // Flip the anchor point's X value
            Vector2 anchor = hinge.anchor;
            hinge.anchor = new Vector2(-anchor.x, anchor.y);
        }

        // Recursively call FlipHingeAnchors on all child objects of the current bone
        for (int i = 0; i < bone.transform.childCount; i++)
        {
            FlipHingeAnchors(bone.transform.GetChild(i).gameObject);
        }
    }

    //----------------Legacy Code, used in an old flip version------------------

    //private void SetKinematic(GameObject bone)
    //{
    //    for (int i = 0; i < bone.transform.childCount; i++)
    //    {
    //        SetKinematic(bone.transform.GetChild(i).gameObject);
    //    }

    //    if (bone.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
    //        if (!rb.isKinematic)
    //        {
    //            rb.isKinematic = true;
    //        }
    //}

    //private void SetDynamic(GameObject bone)
    //{
    //    for (int i = 0; i < bone.transform.childCount; i++)
    //    {
    //        SetDynamic(bone.transform.GetChild(i).gameObject);
    //    }

    //    if (bone.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
    //        if (rb.isKinematic)
    //        {
    //            rb.isKinematic = false;
    //        }
    //}

    //private IEnumerator RotateOverTime(float duration)
    //{
    //    currentlyTurning = true;
    //    SetKinematic(rootBone.gameObject);
    //    FlipHingeLimits(rootBone.gameObject);

    //    foreach (FootPositioner limb in limbTargets)
    //    {
    //        limb.enabled = false;
    //    }

    //    Vector3 targetRotatorPosition = targetToBeRotatedAround.position;
    //    flipLimbs();

    //    //Quaternion[] startRotations = new Quaternion[transformsToBeRotated.Count];
    //    //for(int i = 0; i < transformsToBeRotated.Count;i++)
    //    //    startRotations[i] = transformsToBeRotated[i].rotation;


    //    //Quaternion[] targetRotations = new Quaternion[transformsToBeRotated.Count];
    //    //for (int i = 0; i < transformsToBeRotated.Count; i++)
    //    //{
    //    //    Vector3 directionToTarget = transformsToBeRotated[i].position - targetToBeRotatedAround.position;
    //    //    if (startRotations[i].y > 170)
    //    //        targetRotations[i] = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(0f, -180f, 0f);
    //    //    else
    //    //        targetRotations[i] = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(0f, 180f, 0f);
    //    //}

    //    float timeElapsed = 0f;

    //    while (timeElapsed < duration)
    //    {
    //        foreach(Transform t in transformsToBeRotated)
    //            t.RotateAround(targetRotatorPosition, Vector3.up, 180/duration * Time.deltaTime);
    //        timeElapsed += Time.deltaTime;

    //        yield return null;
    //    }

    //    SetDynamic(rootBone.gameObject);
    //    foreach (FootPositioner limb in limbTargets)
    //    {
    //        limb.enabled = true;
    //    }

    //    currentlyTurning = false;
    //}
    #endregion
}
