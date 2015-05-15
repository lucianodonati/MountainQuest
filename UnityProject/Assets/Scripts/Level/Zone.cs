using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public List<SkillsManager.SpheresId> spheresEnabled;
    private List<SkillsManager.SpheresId> oldSpheres;
    public bool destroyOnExit, playerMovement, oldMovement, playerCombat, oldCombat;
    private PlayerController playerController;
    private SkillsManager skills;

    private void Start()
    {
        oldSpheres = new List<SkillsManager.SpheresId>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        skills = GameManager.instance.skillsManager;

        oldCombat = playerController.combatEnabled;
        oldMovement = playerController.movementEnabled;

        playerController.combatEnabled = playerCombat;
        playerController.movementEnabled = playerMovement;

        foreach (SkillsManager.SetPrefabs sphere in skills.spheres)
        {
            if (sphere.active)
                oldSpheres.Add(sphere.id);

            skills.ToggleSphere(sphere.id, spheresEnabled.Contains(sphere.id));
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        playerController.combatEnabled = oldCombat;
        playerController.movementEnabled = oldMovement;

        foreach (SkillsManager.SetPrefabs sphere in skills.spheres)
        {
            skills.ToggleSphere(sphere.id, oldSpheres.Contains(sphere.id));
        }

        if (destroyOnExit)
            Destroy(gameObject);
    }
}