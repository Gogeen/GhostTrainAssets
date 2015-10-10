using UnityEngine;
using System.Collections;
[RequireComponent(typeof(TextQuestSystem))]
public class MapTextEventController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            if (!coll.gameObject.GetComponent<WagonScript>().IsHead())
                return;
            GetComponent<TextQuestSystem>().StartQuest();
            
        }
    }
}
