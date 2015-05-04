using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour {
    public Player player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	public void BuyUpgrade()
    {
        //if(player.skil)
    }
}
