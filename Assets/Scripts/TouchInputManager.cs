// This script belongs on Main Camera and requires setup by adding Touch Input layer to the touchInputMask
//This code was modified from https://www.youtube.com/watch?v=SrCUO46jcxk
// To use, add the following methods to the GamecObject you wish to have touch functionality
// void OnTOuchDown(){}   // animation for being selected
// void OnTouchUp(){}    // Flip card
// void OnTouchStay(){} // This is active when an object is being held down (maybe we don't want to flip card until finger release)
// void OnTouchExit(){} // This occurs when your finger is on object then slide finger off to cancel action (doesn't work for mouse input)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    public LayerMask touchInputMask;

    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld;
    private RaycastHit2D hit;

    private void Start()
    {
        Input.multiTouchEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.touchCount > 0)
        {
            
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();

            foreach (Touch touch in Input.touches)
            {
                Vector2 ray = GetComponent<Camera>().ScreenToWorldPoint(touch.position);

                if ((hit = Physics2D.Raycast(ray, Vector2.zero, touchInputMask)))
                {
                    GameObject recipent = hit.transform.gameObject;
                    touchList.Add(recipent);

                    if (touch.phase == TouchPhase.Began)
                    {
                        recipent.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        recipent.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        recipent.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Canceled)
                    {
                        recipent.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            
            foreach (GameObject g in touchesOld)
            {
                if (!touchList.Contains(g))
                {
                    g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
            
        }
        
        else if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();

            Vector2 ray = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

            if ((hit = Physics2D.Raycast(ray, Vector2.zero, touchInputMask)))
            {
                GameObject recipent = hit.transform.gameObject;
                touchList.Add(recipent);
                //print(hit.point);

                if (Input.GetMouseButtonDown(0))
                {
                    recipent.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    //print("touch down");
                }
                if (Input.GetMouseButtonUp(0))
                {
                    recipent.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                }
                if (Input.GetMouseButton(0))
                {
                    recipent.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }

            foreach (GameObject g in touchesOld)
            {
                if (!touchList.Contains(g))
                {
                    if (g != null)
                        g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

    }
}
