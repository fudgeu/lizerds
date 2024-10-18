using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LineRenderer lineRenderer;
    [Tooltip("From front to back, the circles which the line renderer should draw points on")][SerializeField] private List<Transform> bodySegment = new List<Transform> ();

    [Header("Configuration")]
    [Tooltip("The amount of points the end cap should recieve")][SerializeField] private int endCapPointCount;

    //--Private Variables--
    private Vector3[] linePositions;
    private Transform[] segmentArray;

    private void Start()
    {
        segmentArray = bodySegment.ToArray();
        linePositions = new Vector3[segmentArray.Length * 2 + 2];

        //Will be used when end caps are ready for implementation.
        //lineRenderer.positionCount = bodySegment.Count*2 + endCapPointCount;

        lineRenderer.positionCount = bodySegment.Count * 2;
        lineRenderer.loop = true;

        lineRenderer.numCapVertices = endCapPointCount;

        //Read through the position array, and add points to the linePositions list
        //  Head point
        linePositions[0] = segmentArray[0].position;
        //  Top Points
        for (int i = 1; i< segmentArray.Length; i++)
        {
            linePositions[i] = segmentArray[i].position + segmentArray[i].up * segmentArray[i].localScale.x/2;
        }
        //  Tail Point
        linePositions[segmentArray.Length] = segmentArray[segmentArray.Length - 1].position + segmentArray[segmentArray.Length - 1].right * segmentArray[segmentArray.Length - 1].localScale.x/2 * -1;
        //  Bottom POints
        for(int i = segmentArray.Length - 1; i >= 0; i--)
        {
            linePositions[2 * segmentArray.Length - i] = segmentArray[i].position + segmentArray[i].up * segmentArray[i].localScale.x / 2 * -1;
        }
    }


    private void Update()
    {
        //  Top Points
        for (int i = 0; i < segmentArray.Length; i++)
        {
            linePositions[i] = segmentArray[i].position + segmentArray[i].up * segmentArray[i].localScale.x/2;

        }
        //  Tail Point
        linePositions[segmentArray.Length] = segmentArray[segmentArray.Length - 1].position + segmentArray[segmentArray.Length - 1].right * segmentArray[segmentArray.Length - 1].localScale.x /2 * -1;
        //  Bottom POints
        for (int i = segmentArray.Length - 1; i >= 0; i--)
        {
            linePositions[2 * segmentArray.Length - i] = segmentArray[i].position + segmentArray[i].up * segmentArray[i].localScale.x/2 * -1;
        }

        lineRenderer.SetPositions(linePositions);
    }

}
