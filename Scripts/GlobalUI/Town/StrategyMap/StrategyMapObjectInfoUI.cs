using UnityEngine;
using System.Collections;

public class StrategyMapObjectInfoUI : MonoBehaviour {

    public void ShowInfo(GameObject obj)
    {
        gameObject.SetActive(true);
        transform.position = obj.transform.position;
        Vector3 localPos = transform.localPosition;
        localPos += new Vector3(0, GetComponent<UISprite>().localSize.y / 2 + 100, 0);
        transform.localPosition = localPos;
        
    }

    public void HideInfo()
    {
        gameObject.SetActive(false);
    }
}
