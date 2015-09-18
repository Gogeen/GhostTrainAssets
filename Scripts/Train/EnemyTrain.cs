using UnityEngine;
using System.Collections;

public class EnemyTrain : TrainController {

	public float waystationTime;

	public override void Start()
	{
		base.Start ();
		AccelerateTo (maxSpeed);
	}

	public void StopFor(float time)
	{
		StopCoroutine ("stopFor");
		StartCoroutine ("stopFor", time);
	}

	IEnumerator stopFor(float time)
	{
		AccelerateTo (0);
		while (speed != 0)
		{
			yield return null;
		}
		yield return new WaitForSeconds (time);
		AccelerateTo (maxSpeed);
	}
}
