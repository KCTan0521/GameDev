using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TouchDetector : MonoBehaviour
{
    public bool jumpButton;
    public bool dashButton;
    public bool slideButton;
    private Touch touch;
    private Vector2 startPos, endPos;
    private float screenWidth;

    private void Start()
    {
        screenWidth = Screen.width;
    }

    private void Update()
    {
        jumpButton = false;
        dashButton = false;
        slideButton = false;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    endPos = touch.position;

                    if (startPos == endPos)
                    {
                        if (startPos.x < screenWidth / 2)
                        {
                            jumpButton = true;
                        }

                        else
                        {
                            dashButton = true;
                            Debug.Log("dash");
                        }
                    }

                    else if (startPos != endPos)
                    {
                        slideButton = true;
                    }

                    break;
            }
        }

       
    }
}