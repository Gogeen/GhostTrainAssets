using UnityEngine;
using System.Collections;

public class SignsController : MonoBehaviour {

	public float globalCooldown = 0;
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
		}
		else if (type == SignType.Rectangle)
		{
			StartCoroutine (((RectangleSign)rectangleSign).Cast(GetComponent<PlayerTrain>()));
			globalCooldown = ((Sign)rectangleSign).cooldown;
		}
	}

	void Update()
	{
		if (globalCooldown > 0)
			globalCooldown -= Time.deltaTime;
	}
}
