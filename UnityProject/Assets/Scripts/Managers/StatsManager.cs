using System.Collections;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private static StatsManager _instance;

    public static StatsManager instance
    {
        get
        {
            if (_instance != null)
            {
                DontDestroyOnLoad(_instance.gameObject);
            }
            else
            {
                _instance = GameObject.FindObjectOfType<StatsManager>();
            }

            return _instance;
        }
    }

    // Time
    public float timePlayed { get; set; }

    // Attack
    public float damageDealt { get; set; }

    public float damageTaken { get; set; }

    // Enemies
    public int enemiesKilledTotal { get; set; }

    // Arrows

    public int shotsFired { get; set; }

    public int shotsHit { get; set; }

    public int accuracy
    {
        get { return (shotsHit * 100) / (shotsFired > 0 ? shotsFired : 1); }
    }

    public int arrowsRedirected { get; set; }

    public int arrowsBoosted { get; set; }

    public void Awake()
    {
        transform.parent = GameManager.instance.transform;
    }

    // Use this for initialization
    private void Start()
    {
        timePlayed = damageDealt = damageTaken = 0.0f;
        enemiesKilledTotal = shotsFired = arrowsRedirected = arrowsBoosted = shotsHit = 0;
    }
}