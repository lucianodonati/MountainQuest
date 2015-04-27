using UnityEngine;
using System.Collections;

public class WindArrow : Arrow {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	  private void OnCollisionEnter2D(Collision2D coll)
    {
        numCollisions += 1;
        if (coll.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            GameManager.instance.stats.shotsHit++;
            Entity isEntity = coll.gameObject.GetComponent<Entity>();
            if (isEntity != null)
                damageType.attachToEnemy(isEntity);
        }
        if (numCollisions <= 2)
        {
            GetStuck(coll.collider);            
        }
    }
}
