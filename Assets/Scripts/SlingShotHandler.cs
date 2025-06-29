using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SlingShotHandler : MonoBehaviour
{
    private LineRenderer _leftLineRenderer, _rightLineRenderer;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Debug.Log("mouse was clicked");
            DrawSlingShot();
        }

        Debug.Log(Mouse.current.position.ReadValue());
    }

    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
