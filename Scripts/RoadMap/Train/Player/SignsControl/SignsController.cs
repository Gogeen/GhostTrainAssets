using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SignsController : MonoBehaviour {

	public bool ghostMode = false;
    public AudioSource source;
	public ScriptableObject triangleSign;
	public ScriptableObject rectangleSign;
	public ScriptableObject barrierSign;

	public List<SignInfo> signs = new List<SignInfo>();

	public enum SignType
	{
		None,
		Triangle,
		Rectangle,
		Barrier
	}

	[System.Serializable]
	public class SignInfo
	{
		public float cooldown = 0;
		public SignType type;
		public ScriptableObject reference;
	}

	SignInfo GetSignInfo(SignType type)
	{
		foreach (SignInfo sign in signs) {
			if (sign.type == type)
				return sign;
		}
		return null;
	}

	public void RemoveCooldowns(float duration)
	{
		foreach (SignInfo sign in signs) {
			if (sign.cooldown > 0) {
				for(int wagonIndex = 0; wagonIndex < transform.childCount; wagonIndex++)
				{
					if (GetComponent<PlayerTrain>().GetWagon(wagonIndex) == null)
						continue;
					if (GetComponent<PlayerTrain>().GetWagon(wagonIndex).signObject != null)
					{
						if (GetComponent<PlayerTrain> ().GetWagon (wagonIndex).signType == sign.type) {
							//GetComponent<PlayerTrain> ().GetWagon (wagonIndex).signObject.GetComponent<Animator> ().CrossFade ("cooldown", 1);
							GetComponent<PlayerTrain> ().GetWagon (wagonIndex).signObject.GetComponent<Animator> ().Play("idle");
						}
					}
				}
			}
			sign.cooldown = 0;
		}
		StartCoroutine ("useGhostMode", duration);
	}

	IEnumerator useGhostMode(float duration)
	{
		ghostMode = true;
		yield return new WaitForSeconds (duration);
		ghostMode = false;
	}

	public bool CanCast(SignType type)
	{
		if (type == SignType.None)
			return false;
		return GetSignInfo(type).cooldown <= 0;
	}

	public void CastSign(SignType type)
	{
		SignInfo info = GetSignInfo (type);
		Sign castingSign = (Sign)(info.reference);
		if (type == SignType.Triangle)
		{
			StartCoroutine (((TriangleSign)castingSign).Cast(GetComponent<PlayerTrain>()));
		}
		else if (type == SignType.Rectangle)
		{
			StartCoroutine (((RectangleSign)castingSign).Cast(GetComponent<PlayerTrain>()));
		}
		else if (type == SignType.Barrier)
		{
			StartCoroutine (((BarrierSign)castingSign).Cast(GetComponent<PlayerTrain>()));
		}


		source.clip = castingSign.sound;
		source.Play();
		if (ghostMode)
			return;

		info.cooldown = castingSign.cooldown;

		for(int wagonIndex = 0; wagonIndex < transform.childCount; wagonIndex++)
		{
			if (GetComponent<PlayerTrain>().GetWagon(wagonIndex) == null)
				continue;
			if (GetComponent<PlayerTrain>().GetWagon(wagonIndex).signObject != null)
			{
				if (GetComponent<PlayerTrain> ().GetWagon (wagonIndex).signType == type) {
					GetComponent<PlayerTrain> ().GetWagon (wagonIndex).signObject.GetComponent<Animator> ().Play ("cooldown", 0);
					GetComponent<PlayerTrain> ().GetWagon (wagonIndex).signObject.GetComponent<Animator> ().speed = 1 / info.cooldown;
				}
			}
		}

	}

	float magicPowerApplied = 0;

	void Update()
	{
		if (PlayerSaveData.reference.trainData.magicPower != magicPowerApplied) {
			((TriangleSign)triangleSign).duration /= (100 + magicPowerApplied) / 100;
			//((RectangleSign)rectangleSign).;
			((BarrierSign)barrierSign).duration /= (100 + magicPowerApplied) / 100;
			GetComponent<TrainEventManager>().ghostModeCooldown *= (100 + magicPowerApplied) / 100;

			magicPowerApplied = PlayerSaveData.reference.trainData.magicPower;

			((TriangleSign)triangleSign).duration *= (100 + magicPowerApplied) / 100;
			//((RectangleSign)rectangleSign).;
			((BarrierSign)barrierSign).duration *= (100 + magicPowerApplied) / 100;
			GetComponent<TrainEventManager>().ghostModeCooldown /= (100 + magicPowerApplied) / 100;
		}
		foreach (SignInfo sign in signs) {
			if (sign.cooldown > 0)
				sign.cooldown -= Time.deltaTime;
		}
	}
}
