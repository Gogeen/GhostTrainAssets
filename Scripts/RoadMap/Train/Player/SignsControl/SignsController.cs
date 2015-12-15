using UnityEngine;
using System.Collections;

public class SignsController : MonoBehaviour {

	public float globalCooldown = 0;
    public AudioSource source;
	public ScriptableObject triangleSign;
	public ScriptableObject rectangleSign;
	public enum SignType
	{
		None,
		Triangle,
		Rectangle
	}

	public bool CanCast()
	{
		return globalCooldown <= 0;
	}

	public void CastSign(SignType type)
	{
		if (type == SignType.Triangle)
		{
			StartCoroutine (((TriangleSign)triangleSign).Cast(GetComponent<PlayerTrain>()));
			globalCooldown = ((Sign)triangleSign).cooldown;
            source.clip = ((Sign)triangleSign).sound;
        }
		else if (type == SignType.Rectangle)
		{
			StartCoroutine (((RectangleSign)rectangleSign).Cast(GetComponent<PlayerTrain>()));
			globalCooldown = ((Sign)rectangleSign).cooldown;
            source.clip = ((Sign)rectangleSign).sound;
        }
		globalCooldown *= (100 - PlayerSaveData.reference.trainData.magicPower) / 100;
		source.Play();
		for(int wagonIndex = 0; wagonIndex < transform.childCount; wagonIndex++)
		{
			if (GetComponent<PlayerTrain>().GetWagon(wagonIndex) == null)
				continue;
			if (GetComponent<PlayerTrain>().GetWagon(wagonIndex).signObject != null)
			{
				GetComponent<PlayerTrain>().GetWagon(wagonIndex).signObject.GetComponent<Animator>().Play("cooldown",0);
				GetComponent<PlayerTrain>().GetWagon(wagonIndex).signObject.GetComponent<Animator>().speed = 1/globalCooldown;
			}
		}

	}

	void Update()
	{
		if (globalCooldown > 0)
			globalCooldown -= Time.deltaTime;
	}
}
