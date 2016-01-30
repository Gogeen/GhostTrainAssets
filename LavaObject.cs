using UnityEngine;
using System.Collections;

public class LavaObject : MonoBehaviour {

	public GameObject lavaBallPrefab;
	public Transform spawn;

	public float spawnTime;
	public float spawnCooldown;
	public float spawnCooldownFrequency;
	public float damage;
	public float damageFrequency;
	public float moveSpeed;

	float spawnTimer = 0;
	float spawnCooldownTimer = 0;
	float damageTimer = 0;
	void Start()
	{
		spawnCooldownTimer = spawnCooldownFrequency;
	}

	public void OnTrigger(Collider2D coll)
	{
		if (damageTimer <= 0)
		{
			InventoryItemsBreakSystem.reference.BreakWagon(coll.GetComponent<WagonScript> ().GetIndex (), damage);
			Debug.Log("hit!");
			damageTimer = damageFrequency;
		}
	}

	void Spawn ()
	{
		GameObject lavaBall = Instantiate (lavaBallPrefab) as GameObject;
		lavaBall.transform.parent = transform;
		lavaBall.transform.position = spawn.position;

		lavaBall.GetComponent<LavaBall> ().velocity = (transform.position - spawn.position).normalized*moveSpeed;
		lavaBall.GetComponent<LavaBall> ().liveRange = (transform.position - spawn.position).magnitude;
	}

	void Update()
	{
		if (spawnTimer > 0)
			spawnTimer -= Time.deltaTime;
		if (spawnCooldownTimer > 0)
			spawnCooldownTimer -= Time.deltaTime;
		if (damageTimer > 0)
			damageTimer -= Time.deltaTime;
		
		if (spawnTimer <= 0) {
			Spawn ();
			if (spawnCooldownTimer <= 0) {
				spawnTimer = spawnCooldown;
				spawnCooldownTimer = spawnCooldownFrequency;
			} else {
				spawnTimer = spawnTime;
			}
		}
	}
}
