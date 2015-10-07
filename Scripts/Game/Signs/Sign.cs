using UnityEngine;
using System.Collections;

public class Sign : ScriptableObject {

	public float cost;
	public float cooldown;
	public GameObject prefab;
	public RoadFeature roadFeature;
    public AudioClip sound;

	public virtual IEnumerator Cast(PlayerTrain playerTrain)
	{

		yield return null;
	}
}
