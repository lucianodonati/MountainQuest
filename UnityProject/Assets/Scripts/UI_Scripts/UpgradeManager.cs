using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {
    private Player player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        transform.FindChild("InfoPanel").FindChild("Text").GetComponent<Text>().text = "Level " + player.level + "\nSkill Points " + player.skillPoints;
	}

    void Update()
    {
        foreach (BuyButton buyButton in GetComponentsInChildren<BuyButton>())
        {
            if (!buyButton.bought && buyButton.skillCost <= player.skillPoints)
                buyButton.GetComponent<Button>().interactable = true;
            else
                buyButton.GetComponent<Button>().interactable = false;
        }
    }
	
	public void BuyUpgrade(BuyButton buyButton)
    {
        if(player.skillPoints >= buyButton.skillCost)
        {
            player.skillPoints -= buyButton.skillCost;
            buyButton.bought = true;
            buyButton.GetComponent<Button>().interactable = false;

            if (buyButton.name.Contains("Shield"))
                GameManager.instance.skillsManager.ToggleSphere(SkillsManager.SpheresId.Shield, true);
            if(buyButton.name.Contains("Wave"))
                GameManager.instance.skillsManager.ToggleSphere(SkillsManager.SpheresId.Wave, true);

            if (buyButton.name.Contains("Exploding"))
                /*GameManager.instance.skillsManager.ToggleSphere(GameManager.instance.skillsManager.ArrowsID.Exploding, true)*/;
            if(buyButton.name.Contains("Shattering"))
                /*GameManager.instance.skillsManager.ToggleSphere(GameManager.instance.skillsManager.ArrowsID.Shattering, true)*/;
            if (buyButton.name.Contains("Lightning"))
                /*GameManager.instance.skillsManager.ToggleSphere(GameManager.instance.skillsManager.ArrowsID.Lightning, true)*/;
            if(buyButton.name.Contains("Earthquake"))
                /*GameManager.instance.skillsManager.ToggleSphere(GameManager.instance.skillsManager.ArrowsID.Earthquake, true)*/;
            if (buyButton.name.Contains("Plague"))
                /*GameManager.instance.skillsManager.ToggleSphere(GameManager.instance.skillsManager.ArrowsID.Plague, true)*/;

            transform.FindChild("InfoPanel").FindChild("Text").GetComponent<Text>().text = "Level " + player.level + "\nSkill Points " + player.skillPoints;
        }
    }
}
