using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public string abilityType = "";
    public float minDamageRequirement = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Arrow" || other.gameObject.tag == "Sword")
        {
            if ((abilityType == "" || other.name.Contains(abilityType)) &&
                ((other.GetComponent<OneTimeHit>() != null && other.GetComponent<OneTimeHit>().damage >= minDamageRequirement) ||
                (other.GetComponent<Affliction>() != null && other.GetComponent<Affliction>().damage >= minDamageRequirement) ||
                (other.GetComponent<Parasite>() != null && other.GetComponent<Parasite>().damage >= minDamageRequirement)))
                Destroy(this.gameObject);
        }
    }
}