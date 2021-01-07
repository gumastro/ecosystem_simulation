using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ecosystem : MonoBehaviour
{
    public GameObject rabbit;
	public GameObject fox;
    public GameObject food;
	public GameObject item;

    // INFO
    public GameObject infoScreen;
    GameObject originalGameObject;
    GameObject[] childs;
    float avg_speed;
    float avg_vision;

    public Transform scrollViewRabbit;
	public Transform scrollViewFox;

    public float spawnDelay;
    public float infoDelay;
    public float initRabbit;
	public float initFox;
    public float initFood;

    // Start is called before the first frame update
    void Start()
    {
        SpawnInit();
        StartCoroutine(SpawnFood());
        StartCoroutine(Info());
    }

    void SpawnInit()
    {
        // INITIAL RANDOM RABBIT SPECIMENS
        for (int i = 0; i < initRabbit; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(0f, 100f), 0.6f, Random.Range(0f, 100f));
            GameObject subject = Instantiate(rabbit, randPos, Quaternion.identity, transform.Find("Subjects"));
            GameObject _item = Instantiate(item, Vector3.zero, Quaternion.identity, scrollViewRabbit);
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

            subject.GetComponent<Rabbit>().SetChromosome(Random.Range(1f, 6f), Random.Range(5f, 30f), _item, 1, color);
        }

		// INITIAL RANDOM FOX SPECIMENS
		for (int i = 0; i < initFox; i++)
		{
			Vector3 randPos = new Vector3(Random.Range(0f, 100f), 0.6f, Random.Range(0f, 100f));
			GameObject subject = Instantiate(fox, randPos, Quaternion.identity, transform.Find("Subjects"));
			GameObject _item = Instantiate(item, Vector3.zero, Quaternion.identity, scrollViewFox);
			Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

			subject.GetComponent<Rabbit>().SetChromosome(Random.Range(1f, 6f), Random.Range(5f, 30f), _item, 1, color);
		}

        // INITIAL FOOD
        for (int i = 0; i < initFood; i++)
        {   
            Vector3 randPos = new Vector3(Random.Range(0, 100), 0.6f, Random.Range(0, 100));
            Instantiate(food, randPos, Quaternion.identity, transform.Find("Foods"));
        }


        originalGameObject = GameObject.Find("Subjects");
    }

    // RANDOM FOOD OVER TIME
    IEnumerator SpawnFood()
    {
        while (true)
        {
            Vector3 randPos = new Vector3(Random.Range(0, 100), 0.6f, Random.Range(0, 100));
            Instantiate (food, randPos, Quaternion.identity, transform.Find("Foods"));
            
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator Info()
    {
        while (true)
        {
            // ARRAY TO STORE CHILD
            childs = new GameObject[originalGameObject.transform.childCount];

            infoScreen.GetComponentInChildren<Text>().text = "";
            avg_speed = 0;
            avg_vision = 0;
            for (int i = 0; i < childs.Length; i++)
            {
                // GET THE GAME OBJECT FROM THE SELECTED CHILD
                childs[i] = originalGameObject.transform.GetChild(i).gameObject;
                avg_speed += childs[i].GetComponent<Rabbit>().GetSpeed();
                avg_vision += childs[i].GetComponent<Rabbit>().GetVision();
            }
            avg_speed /= originalGameObject.transform.childCount;
            avg_vision /= originalGameObject.transform.childCount;

            infoScreen.GetComponentInChildren<Text>().text += "Nº of rabbits: " + originalGameObject.transform.childCount + "\n";
            infoScreen.GetComponentInChildren<Text>().text += "Avg speed: " + avg_speed + "\n";
            infoScreen.GetComponentInChildren<Text>().text += "Avg vision: " + avg_vision + "\n";

            yield return new WaitForSeconds(infoDelay);
        }
    }
}
