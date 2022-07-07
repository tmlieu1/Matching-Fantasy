using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class projectile : MonoBehaviour
{

    private float lifeLimit = 3f;

    private Transform projectileTransform;
    // Start is called before the first frame update
    void Start()
    {
        projectileTransform = GetComponent<Transform>();
        Destroy(gameObject, lifeLimit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        projectileTransform.position = Vector3.MoveTowards(projectileTransform.position, new Vector3(projectileTransform.position.x + 3 *projectileTransform.position.normalized.x, projectileTransform.position.y), 0.5f);
    }
}
