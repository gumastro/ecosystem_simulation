using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject food;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Food"))
        {
            Vector3 randPos = new Vector3(Random.Range(0, 100), 0.6f, Random.Range(0, 100));
            Instantiate(food, randPos, Quaternion.identity, transform.parent.Find("Foods"));

            Destroy(other.gameObject);
        }
    }
}
