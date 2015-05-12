using UnityEngine;
using System.Collections;

public class BaseSphere : MonoBehaviour {
    public float AliveTimer = 7;
    public Player Owner;
    private float startingTransparency;

	// Use this for initialization
	public virtual void Start () {
        startingTransparency = GetComponent<SpriteRenderer>().color.a;
	}
	
	// Update is called once per frame
	public virtual void Update () {
        if (AliveTimer != -1)
        {
            AliveTimer -= Time.deltaTime;
            if (AliveTimer <= 0)
            {
                if (Owner != null && Owner.GetComponent<Player>() != null)
                    Owner.GetComponent<Player>().RemoveOSphere();
                Destroy(this.gameObject);
            }

            if (AliveTimer <= 2)
            {
                SpriteRenderer mySR = gameObject.GetComponent<SpriteRenderer>();
                mySR.color = new Color(1, 1, 1, startingTransparency * AliveTimer / 2);
            }
        }
    }
}
