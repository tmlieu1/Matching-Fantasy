using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.gamasutra.com/blogs/VivekTank/20180710/321793/Multiple_ways_of_doing_sprite_sheet_animation_in_Unity3D.php

public class PlayerAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] walk;
    public Sprite[] idle;
    public Sprite[] jump;
    void Start()
    {
        StartCoroutine(Idle());
    }
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.I))
        {
            StopAllCoroutines();
            StartCoroutine(Idle());
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StopAllCoroutines();
            StartCoroutine(Jump());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StopAllCoroutines();
            StartCoroutine(Walk());
        }
        */
        //StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {
        int i;
        i = 0;
        while (i < idle.Length)
        {
            spriteRenderer.sprite = idle[i];
            i++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;

        }
        StartCoroutine(Idle());
    }
    IEnumerator Walk()
    {
        int i;
        i = 0;
        while (i < walk.Length)
        {
            spriteRenderer.sprite = walk[i];
            i++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;
        }
        StartCoroutine(Walk());
    }

    IEnumerator Jump()
    {
        int i;
        i = 0;
        while (i < jump.Length)
        {
            spriteRenderer.sprite = jump[i];
            i++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;

        }
        StartCoroutine(Jump());
    }


}
