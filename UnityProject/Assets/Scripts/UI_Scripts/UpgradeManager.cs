using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private Player player;

    // Use this for initialization
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        transform.FindChild("InfoPanel").FindChild("Text").GetComponent<Text>().text = "Level " + player.level + "\nSkill Points " + player.skillPoints;
    }

    private void Update()
    {
        foreach (BuyButton buyButton in GetComponentsInChildren<BuyButton>())
        {
            if ((buyButton.name.Contains("Shield") && GameManager.instance.skillsManager.spheres[(int)SkillsManager.SpheresId.Shield].active) ||
                (buyButton.name.Contains("Wave") && GameManager.instance.skillsManager.spheres[(int)SkillsManager.SpheresId.Wave].active) ||
                (buyButton.name.Contains("Exploding") && GameManager.instance.skillsManager.arrows[(int)SkillsManager.ArrowsId.Exploding].active) ||
                (buyButton.name.Contains("Shattering") && GameManager.instance.skillsManager.arrows[(int)SkillsManager.ArrowsId.Shattering].active) ||
                (buyButton.name.Contains("Lightning") && GameManager.instance.skillsManager.arrows[(int)SkillsManager.ArrowsId.Lightning].active) ||
                (buyButton.name.Contains("Earthquake") && GameManager.instance.skillsManager.arrows[(int)SkillsManager.ArrowsId.EarthQuake].active) ||
                (buyButton.name.Contains("Plague") && GameManager.instance.skillsManager.arrows[(int)SkillsManager.ArrowsId.Plague].active))
                buyButton.bought = true;

            if (!buyButton.bought && buyButton.skillCost <= player.skillPoints)
                buyButton.GetComponent<Button>().interactable = true;
            else
                buyButton.GetComponent<Button>().interactable = false;
        }
    }

    public void BuyUpgrade(BuyButton buyButton)
    {
        if (player.skillPoints >= buyButton.skillCost)
        {
            player.skillPoints -= buyButton.skillCost;
            buyButton.bought = true;
            buyButton.GetComponent<Button>().interactable = false;

            if (buyButton.name.Contains("Shield"))
                GameManager.instance.skillsManager.SetSphere(SkillsManager.SpheresId.Shield, true);
            if (buyButton.name.Contains("Wave"))
                GameManager.instance.skillsManager.SetSphere(SkillsManager.SpheresId.Wave, true);

            if (buyButton.name.Contains("Exploding"))
            {
                GameManager.instance.skillsManager.SetArrow(SkillsManager.ArrowsId.Fire, false);
                GameManager.instance.skillsManager.SetArrow(SkillsManager.ArrowsId.Exploding, true);
            }
            if (buyButton.name.Contains("Shattering"))
            {
                GameManager.instance.skillsManager.SetArrow(SkillsManager.ArrowsId.Ice, false);
                GameManager.instance.skillsManager.SetArrow(SkillsManager.ArrowsId.Shattering, true);
            }
            if (buyButton.name.Contains("Lightning"))
            {
                GameManager.instance.skillsManager.SetArrow(SkillsManager.ArrowsId.Wind, false);
                GameManager.instance.skillsManager.SetArrow(SkillsManager.ArrowsId.Lightning, true);
            }
            if (buyButton.name.Contains("Earthquake"))
            {
                GameManager.instance.skillsManager.SetArrow(SkillsManager.ArrowsId.Earth, false);
                GameManager.instance.skillsManager.SetArrow(SkillsManager.ArrowsId.EarthQuake, true);
            }
            if (buyButton.name.Contains("Plague"))
                GameManager.instance.skillsManager.SetArrow(SkillsManager.ArrowsId.Plague, true);

            transform.FindChild("InfoPanel").FindChild("Text").GetComponent<Text>().text = "Level " + player.level + "\nSkill Points " + player.skillPoints;
        }
    }
}