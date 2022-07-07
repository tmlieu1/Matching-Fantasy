using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // init length and starting position variables, attach camera, parallaxEffect rate
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    // init startposition and length 
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // init speed of parallax effect on object based on prev parallaxEffect float
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        // shifts the position of the images to the right based on temp, dist
        transform.position = new Vector2(startpos + dist, transform.position.y + transform.position.z);

        if (temp > startpos + length)
            startpos += length;

        else if (temp < startpos - length)
            startpos -= length;

    }
}
