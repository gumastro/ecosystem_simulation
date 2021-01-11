using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Rabbit : MonoBehaviour
{
    public GameObject rabbit;
	public GameObject itemPrefab;

    public enum Status {
        Idle, Eat, Drink, Love, Escape
    }

    public Status status;
    public Slider displayHunger;
	public Slider displayThirst;
	public Slider displayLove;

    public float lifeTime;
    public string scrollViewTag;
    public float moveTimeMax;
    public float foodValue = 100;
	public float waterValue = 100;

	Transform scrollView;
	Transform graveyard;
    Transform food;
	Transform water;
	Transform predator;
	Rabbit partner;

    NavMeshAgent agent;
    Vector3 direction;
    SphereCollider col;
    GameObject item;

    float hunger = 0;
	float thirst = 0;
	float love = 0;
    float moveTime;
    float reproductionMult;
    float hungerPercentage;
    float needEscape;

    void Awake()
    {
		agent = GetComponent<NavMeshAgent>();
		col = GetComponentInChildren<SphereCollider>();

        scrollView = GameObject.FindGameObjectWithTag(scrollViewTag).transform;
		graveyard = GameObject.FindGameObjectWithTag("Graveyard").transform;

		direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    // Start is called before the first frame update
    void Start()
    {
        status = Status.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;

        // SET RANDOM DIRECTIONS 
        moveTime += Time.deltaTime;

		// INCREASE HUNGER BY TIME
		hunger += 3 * Time.deltaTime;
		thirst += 3 * Time.deltaTime;
		love += reproductionMult * Time.deltaTime;

        // CHANGE DIRECTION
        if (moveTime >= moveTimeMax)
        {
            direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            moveTime = 0;
        }
        
        SetStatus();
        SetDestination();

		// DISPLAY
		displayHunger.value = hunger;
		displayThirst.value = thirst;
		displayLove.value = love;

        Death(false);
    }

    void SetStatus()
    {
        if (status == Status.Escape) return;

        if (hunger > hungerPercentage && hunger >= thirst) status = Status.Eat;
		if (thirst > hungerPercentage && hunger < thirst) status = Status.Drink;
    }

    void SetDestination()
    {
        // SET DESTINATION
        if (status == Status.Eat && food)
        {
            agent.SetDestination(food.position);
			Eat();
        }
        else if (status == Status.Drink && water)
        {
			agent.SetDestination(water.position);
            Drink();
        }
        else if (status == Status.Love && partner)
        {
			agent.SetDestination(partner.transform.position);
            Love();
        }
        else if (status == Status.Escape && predator)
        {
			direction = transform.position - predator.position;
            agent.SetDestination(transform.position + direction);   
        }
        else
        {
            agent.SetDestination(transform.position + direction * 10);
        }
    }

    // REDUCE HUNGER
    void Eat()
    {
        if (Vector3.Distance(transform.position, food.position) <= 3f) 
        {            
            hunger = Mathf.Max(0, hunger - foodValue);

            if (food.GetComponent<Rabbit>() != null) food.GetComponent<Rabbit>().Death(true);

            Destroy(food.gameObject);
            food = null;

            status = Status.Idle;
        }
    }

    // REDUCE THIRST
	void Drink()
	{
		if (Vector3.Distance(transform.position, water.position) <= 3f)
		{
            thirst = Mathf.Max(0, thirst - waterValue);
            water = null;

			status = Status.Idle;
		}
	}

    void Love()
    {
		if (Vector3.Distance(transform.position, partner.transform.position) <= 1f)
		{
            love = 0;
            partner.SetLove(0);

			GameObject children = Instantiate(rabbit, transform.position, Quaternion.identity, transform.parent);

            // CREATE CHILD WITH 50/50 CHROMOSOMES (DAD/MOM) + MUTATION
            float childSpeed = ((agent.speed + partner.GetSpeed()) / 2f) + Random.Range(-1f, 1f);
			float childVision = ((col.radius + partner.GetVision()) / 2f) + Random.Range(-3f, 3f);
			float reproductionLevel = ((reproductionMult + partner.GetReproduction()) / 2f) + Random.Range(-3f, 3f);
			float hungerLevel = ((hungerPercentage + partner.GetHungerLevel()) / 2f) + Random.Range(-10f, 10f);
			float escapeLevel = ((needEscape + partner.GetEscapeLevel()) / 2f) + Random.Range(-3f, 3f);

			GameObject _item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, scrollView);
            Color color = new Color((GetColor().r + partner.GetColor().r) / 2, (GetColor().g + partner.GetColor().g) / 2f, (GetColor().b + partner.GetColor().b) / 2f, 1);

			children.GetComponent<Rabbit>().SetChromosome(childSpeed, 
                                                        childVision, 
                                                        _item, 
                                                        Mathf.Max(int.Parse(transform.name), 
                                                        int.Parse(partner.transform.name)) + 1, color, reproductionLevel, hungerLevel, escapeLevel);

			partner = null;
			status = Status.Idle;
		}
    }

    public void Death(bool eaten)
    {
        if (hunger >= 100 || thirst >= 100 || eaten) 
        {
            Destroy(item);
            gameObject.SetActive(false);
            transform.SetParent(graveyard);
        }
    }

    public void SetFood(Transform target)
    {
        if (hunger <= hungerPercentage || hunger < thirst) return;

        if (food == null || Vector3.Distance(transform.position, food.position) > Vector3.Distance(transform.position, target.position))
        {
            food = target;
            status = Status.Eat;
        }
    }

	public void SetWater(Transform target)
	{
		if (thirst <= hungerPercentage || thirst <= hunger) return;

        if (water == null || Vector3.Distance(transform.position, water.position) > Vector3.Distance(transform.position, target.position))
        {
		    water = target;
		    status = Status.Drink;
        }
	}

	public void SetMate(Rabbit target)
	{
		if ((status != Status.Idle && status != Status.Love) || (target.GetStatus() != Status.Idle && target.GetStatus() != Status.Love)) 
        {
            partner = null;
            return;
        }
		if (love < 100 || target.GetLove() < 100) 
        {
			partner = null;
			return;
        }

		if (partner == null || Vector3.Distance(transform.position, partner.transform.position) > Vector3.Distance(transform.position, target.transform.position))
		{
			partner = target;
			status = Status.Love;
		}
	}

    public void Escape(Transform predator)
    {
        if (Vector3.Distance(transform.position, predator.position) <= needEscape)
        {
            status = Status.Escape;
            this.predator = predator;
        }
    }

    public void SetChromosome(float speed, float vision, GameObject item, int generation, Color color, float reproductionMult, float hungerPercentage, float escapeLevel)
    {
        transform.name = "" + generation;
        
        agent.speed = speed;
		col.radius = vision;
        this.reproductionMult = reproductionMult;
        this.hungerPercentage = hungerPercentage;
		needEscape = escapeLevel;

		GetComponent<Renderer>().material.color = color;

        item.GetComponentInChildren<Text>().text = "Generation: " + transform.name + "\n";
        item.GetComponentInChildren<Text>().text += "Speed: " + speed + "\n";
		item.GetComponentInChildren<Text>().text += "Vision: " + vision;

        this.item = item;
    }

    public Status GetStatus() { return status; }
	public Color GetColor() { return GetComponent<Renderer>().material.color; }
	public float GetSpeed() { return agent.speed; }
	public float GetVision() { return col.radius; }
	public float GetHunger() { return hunger; }
	public float GetThrist() { return thirst; }
	public float GetLove() { return love; }
	public float GetReproduction() { return reproductionMult; }
	public float GetHungerLevel() { return hungerPercentage; }
	public float GetEscapeLevel() { return needEscape; }
	public void SetLove(float value) { this.love = value; }
}
