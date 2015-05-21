using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Animator animator;
    private bool createIt = true;
    private PlayerController player;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (createIt)
        {
            GetComponentsInChildren<Animator>()[0].SetBool("Create", true);
            GetComponentsInChildren<Animator>()[1].SetBool("Create", true);
            createIt = false;
            Destroy(GetComponent<CircleCollider2D>());
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            if (coll.name == "KO")
            {
                GetComponentsInChildren<Animator>()[0].SetBool("Destroy", true);
                GetComponentsInChildren<Animator>()[1].SetBool("Destroy", true);
                coll.GetComponent<Animator>().SetBool("Converted", true);
            }
        }
    }

    private void OnDestroy()
    {
        player.movementEnabled = true;
        player.combatEnabled = true;
    }
}