using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MouseCameraFollow : MonoBehaviour
{
    [SerializeField] float threshHoldX;
    [SerializeField] float threshHoldY;
    [SerializeField] float actualPositionModifier;
    [SerializeField] Transform player;
    [SerializeField] CinemachineVirtualCamera cmVcam;
    Camera cam;

    Vector2 mousePosWorld;
    Vector2 mousePosScreen;
    Vector2 targetPos;

    void Start()
    {
        cmVcam.Follow = player;

        cam = Camera.main;

        SafeZone.OnSafeZoneExit.AddListener(StartCameraFollow);
    }

    void StartCameraFollow()
    {
        cmVcam.Follow = transform;
        SafeZone.OnSafeZoneExit.RemoveListener(StartCameraFollow);
    }

    void Update()
    {
        mousePosScreen = Input.mousePosition;

        mousePosScreen.x = Mathf.Clamp(mousePosScreen.x, 0, Screen.width);
        mousePosScreen.y = Mathf.Clamp(mousePosScreen.y, 0, Screen.height);

        mousePosWorld = cam.ScreenToWorldPoint(mousePosScreen);

        targetPos = mousePosWorld - (Vector2)player.position;

        targetPos *= actualPositionModifier;

        transform.position = (Vector2)player.position + targetPos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(player.position, new Vector3(threshHoldX, threshHoldX, 0));
    }
}
