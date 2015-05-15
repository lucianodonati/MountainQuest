using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        StatsManager stats = GameManager.instance.statsManager;

        string text = "Time Played: ";
        {
            int minutes = (int)stats.timePlayed / 60;
            if (minutes > 0)
                text += minutes + " minutes and ";
            text += (int)stats.timePlayed + " seconds.\n\n";
        }

        text += "Damage Dealt: " + stats.damageDealt + "\nDamage Taken: " + stats.damageTaken + "\n\n";

        text += "Enemies Killed: " + stats.enemiesKilledTotal + "\n\n";

        text += "Shots Fired: " + stats.shotsFired + "\n Accuracy: " + stats.accuracy + "%\n\n";

        text += "Arrows Redirected: " + stats.arrowsRedirected + "\nArrows Boosted: " + stats.arrowsBoosted;
        GetComponent<Text>().text = text;
    }
}