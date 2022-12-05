using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndScale : MonoBehaviour
{
    /*Script that makes the object capable of rotate and scale by touch input
	Put this script inside the object that you want to manipule*/

    Touch touch;
    Vector2 touchPosition;
    Quaternion rotationY;
    float SpeedRotation = 0.5f;

    float initialFingersDistance;
    Vector3 initialScale;
    public GameObject objectTemp;

    // Update is called once per frame
    void Update()
    {

        //If the screen is touched
        if (Input.touchCount == 1)
        {
            //touch is equal to the status of the finger touching the screen
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                //Get the rotation in Y
                rotationY = Quaternion.Euler(0f, -touch.deltaPosition.x * SpeedRotation, 0f);

                //Transforn the rotation of the object to the value of the variable "rotationY"
                transform.rotation = rotationY * transform.rotation;
            }

        }

        //Else, if there are more than one touch, we are scaling
        else if (Input.touches.Length == 2)
        {
            Touch t1 = Input.touches[0];
            Touch t2 = Input.touches[1];

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                initialFingersDistance = Vector2.Distance(t1.position, t2.position);
                initialScale = objectTemp.transform.localScale;
            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {
                var currentFingersDistance = Vector2.Distance(t1.position, t2.position);
                var scaleFactor = currentFingersDistance / initialFingersDistance;
                objectTemp.transform.localScale = initialScale * scaleFactor;
            }
        }

    }

}