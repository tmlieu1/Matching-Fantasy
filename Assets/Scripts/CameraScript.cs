using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Update is called once per frame
    public void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime); // Shift the position of the camera to the right over time
    }
}
