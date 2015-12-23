using UnityEngine;
using System.Collections;

public class TownAction : MonoBehaviour {

	public int actionTimeInMinutes;

	public virtual int GetTime()
	{
		return actionTimeInMinutes;
	}
}
