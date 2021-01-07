using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform subjects;
    public Vector3 offset;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (subjects.childCount > 0)
        {
            Vector3 target = subjects.GetChild(0).position + offset;

            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            subjects.GetChild(0).GetChild(1).LookAt(transform);
        }
    }
}
