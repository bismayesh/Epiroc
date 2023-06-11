using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitPlayerMovement : MonoBehaviour
{
    public Transform playerController;
    public Transform cameraRig;
    public Transform trackingSpace;
    public Transform centerEyeAnchor;

    public Vector3 startPosPlayerController;
    public Vector3 startPosCameraRig;
    public Vector3 startPosTrackingSpace;
    public Vector3 startPosCenterEyeAnchor;

    void Start()
    {
        StartCoroutine(StartPositions());
    }

    IEnumerator StartPositions()
    {
        yield return new WaitForSeconds (1);

        startPosPlayerController = playerController.localPosition;
        startPosCameraRig = cameraRig.localPosition;
        startPosTrackingSpace = trackingSpace.localPosition;
        startPosCenterEyeAnchor = centerEyeAnchor.localPosition;
    }

    void LateUpdate()
    {
        if (playerController.localPosition.x < -0.33f) playerController.localPosition = new Vector3(-0.33f, playerController.localPosition.y, playerController.localPosition.z);

        startPosPlayerController = playerController.localPosition;
        startPosCameraRig = cameraRig.localPosition;
        startPosTrackingSpace = trackingSpace.localPosition;
        startPosCenterEyeAnchor = centerEyeAnchor.localPosition;

    }
}
