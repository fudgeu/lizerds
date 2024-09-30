using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TentacleAnimation : MonoBehaviour
{
    [SerializeField] private int length;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Vector3[] segmentPoses;
    [SerializeField] private Vector3[] segmentV;

    [SerializeField] private float targetDistance;
    [SerializeField] private Transform targetDirection;
    [SerializeField] private float smoothSpeed;

    private void Start()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }

    private void Update()
    {
        segmentPoses[0] = targetDirection.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDirection.right * targetDistance, ref segmentV[i], smoothSpeed);
        }
        lineRenderer.SetPositions(segmentPoses);
    }
}
