using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraFollow : MonoBehaviour
{
    [SerializeField] float threshholdOuter;
    [SerializeField] float threshholdInner;
    [SerializeField] float actualPositionModifier;
    [SerializeField] Transform player;
    Camera cam;

    Vector2 mousePos;
    Vector2 targetPos;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        targetPos = mousePos - (Vector2)player.position;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshholdOuter, threshholdOuter);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshholdOuter, threshholdOuter);

        

        targetPos *= actualPositionModifier;

        transform.position = (Vector2)player.position + targetPos;
    }
}
