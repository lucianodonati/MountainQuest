using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    private Player player;
    private int numRSpheresLeft = 3, numOSpheresLeft = 3, lives = 3;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        if(player.lives != lives)
        {
            lives = player.lives;
            transform.FindChild("LivesText").GetComponent<Text>().text = "Lives " + lives.ToString();
        }

        if(player.RSphereCap - player.RSphereTotal != numRSpheresLeft)
        {
            numRSpheresLeft = player.RSphereCap - player.RSphereTotal;
            for (int sphereImage = 1; sphereImage <= player.RSphereCap; sphereImage++)
            {
                transform.FindChild("RSphereDot" + sphereImage.ToString()).gameObject.SetActive(sphereImage <= numRSpheresLeft);
            }
        }

        if (player.OSphereCap - player.OSphereTotal != numOSpheresLeft)
        {
            numOSpheresLeft = player.OSphereCap - player.OSphereTotal;
            for (int sphereImage = 1; sphereImage <= player.OSphereCap; sphereImage++)
            {
                transform.FindChild("OSphereDot" + sphereImage.ToString()).gameObject.SetActive(sphereImage <= numOSpheresLeft);
            }
        }

        Sprite otherSphereSprite = player.ClickObj.prefab.GetComponent<SpriteRenderer>().sprite;
        if (otherSphereSprite != transform.FindChild("OtherSphereImage").GetComponent<Image>().sprite)
            transform.FindChild("OtherSphereImage").GetComponent<Image>().sprite = otherSphereSprite;
	}
}
