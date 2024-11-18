using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BodyAnimation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementTargetController movementTargetController;     //As I mentioned in NewPlayerContorller, this here is bad software architecture, but i think 'spaghetti code' is a LIBERAL LIE.
    [SerializeField] private DistanceJoint2D jawJoint;
    [SerializeField] private Transform rootBone;
    [SerializeField] private Transform movementTarget;
    [SerializeField] private List<FootPositioner> limbTargets;
    [SerializeField] private List<Transform> transformsToBeRotated;
    [SerializeField] private Transform targetToBeRotatedAround;

    [Header("Configuration")]
    [SerializeField] private float openJawDistance = 1;
    [SerializeField] private float rotateSpeed = 1;
    [SerializeField] private float distanceTillTurn = 0;
    [SerializeField] private float waitAfterLeapToSearchForGround = 1f;

    public bool flippedX = false;
    public bool currentlyTurning = false;

    private bool legsReleased = false;
    private float closeJawDistance;

    private void Start()
    {
        closeJawDistance = jawJoint.distance;
    }

    private void Update()
    {
        if (!currentlyTurning && movementTarget.position.x > rootBone.position.x + distanceTillTurn)
        {
            if (!flippedX)
                FlipX();
        }
        else if (!currentlyTurning && flippedX && movementTarget.position.x < rootBone.position.x - distanceTillTurn)
            FlipX();
    }

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
        while (!movementTargetController.isGrounded)
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

    //--------Flips the Lizerd on the X---------
    public void FlipX()
    {
        flippedX = !flippedX;
        StartCoroutine(RotateOverTime(rotateSpeed));
    }

    //--------------All of the following functions are used for the FlipX() function--------------------
    private void flipLimbs()
    {
        foreach (FootPositioner limb in limbTargets)
        {
            if (limb.gameObject.activeSelf)
                limb.isOnLeft = !limb.isOnLeft;
        }
    }

    private void SetKinematic(GameObject bone)
    {
        if (bone.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            if (!rb.isKinematic)
            {
                rb.isKinematic = true;
            }

        for (int i = 0; i < bone.transform.childCount; i++)
        {
            SetKinematic(bone.transform.GetChild(i).gameObject);
        }
    }

    private void SetDynamic(GameObject bone)
    {
        if (bone.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
            }

        for (int i = 0; i < bone.transform.childCount; i++)
        {
            SetDynamic(bone.transform.GetChild(i).gameObject);
        }
    }

    private void FlipHingeLimits(GameObject bone)
    {
        if (bone.TryGetComponent<HingeJoint2D>(out HingeJoint2D hinge))
        {
            if (hinge.useLimits)
            {
                JointAngleLimits2D limits = hinge.limits;
                float temp = limits.max;
                limits.max = limits.min;
                limits.min = temp;

                hinge.limits = limits;
            }
            //Vector2 connectedAnchor = hinge.connectedAnchor;
            //connectedAnchor.x = connectedAnchor.x * -1;
            //hinge.connectedAnchor = connectedAnchor;
        }

        for (int i = 0; i < bone.transform.childCount; i++)
        {
            FlipHingeLimits(bone.transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator RotateOverTime(float duration)
    {
        currentlyTurning = true;
        SetKinematic(rootBone.gameObject);
        FlipHingeLimits(rootBone.gameObject);

        foreach (FootPositioner limb in limbTargets)
        {
            limb.enabled = false;
        }

        Vector3 targetRotatorPosition = targetToBeRotatedAround.position;
        flipLimbs();

        //Quaternion[] startRotations = new Quaternion[transformsToBeRotated.Count];
        //for(int i = 0; i < transformsToBeRotated.Count;i++)
        //    startRotations[i] = transformsToBeRotated[i].rotation;


        //Quaternion[] targetRotations = new Quaternion[transformsToBeRotated.Count];
        //for (int i = 0; i < transformsToBeRotated.Count; i++)
        //{
        //    Vector3 directionToTarget = transformsToBeRotated[i].position - targetToBeRotatedAround.position;
        //    if (startRotations[i].y > 170)
        //        targetRotations[i] = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(0f, -180f, 0f);
        //    else
        //        targetRotations[i] = Quaternion.LookRotation(directionToTarget) * Quaternion.Euler(0f, 180f, 0f);
        //}

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            foreach(Transform t in transformsToBeRotated)
                t.RotateAround(targetRotatorPosition, Vector3.up, 180/duration * Time.deltaTime);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        SetDynamic(rootBone.gameObject);
        foreach (FootPositioner limb in limbTargets)
        {
            limb.enabled = true;
        }

        currentlyTurning = false;
    }

}