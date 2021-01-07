using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell : MonoBehaviour
{
    public LayerMask food;
	public LayerMask predator;

    void OnTriggerStay(Collider other)
    {
        // CHECK PREDATORS
		if (((1 << other.gameObject.layer) & predator) != 0)
		{
			GetComponentInParent<Rabbit>().Escape(other.transform);
		}
        else 
        {
            // FOOD
            if (((1 << other.gameObject.layer) & food) != 0)
            {
                GetComponentInParent<Rabbit>().SetFood(other.transform);
            }

            // WATER
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                GetComponentInParent<Rabbit>().SetWater(other.transform);
            }

            // PARTNER
            if (other.gameObject.layer == transform.parent.gameObject.layer)
            {
                GetComponentInParent<Rabbit>().SetMate(other.GetComponent<Rabbit>());
            }
        }
    }
}
