using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public int lives = 3;

    [HideInInspector]
    public int level = 1;

    [HideInInspector]
    public int skillPoints = 0;

    private bool isAiming = false;
    public GameObject RedirectClickObj;

    public SkillsManager.SetPrefabs ClickObj;

    private GameObject CreateRedirectSphere;
    private GameObject CreateOtherSphere;
    private GameObject CreateBasicSphere;
    private bool RedirectMade = false;
    public float SphereDistance = 7;
    public float PlayerAliveTimer = 8.0f;

    [HideInInspector]
    public int RSphereTotal = 0;

    public int RSphereCap = 3;

    [HideInInspector]
    public int OSphereTotal = 0;

    public int OSphereCap = 3;
    private float deathTimer = 0.0f;
    private bool reallyDead = false;

    // Use this for initialization
    protected override void Start()
    {
        ClickObj = GameManager.instance.skillsManager.spheres[0];
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (deathTimer > 0)
        {
            deathTimer -= Time.deltaTime;

            if (GetComponent<HealthBar>() != null)
            {
                if (GetComponent<HealthBar>().maxHealthBar != null)
                {
                    GetComponent<HealthBar>().maxHealthBar.renderer.enabled = false;
                    GetComponent<HealthBar>().remainingHealthBar.renderer.enabled = false;
                }
            }
        }
        else
        {
            if (dead && !reallyDead)
            {
                gameObject.layer = LayerMask.NameToLayer("Player");

                dead = false;
                GetComponent<PlayerController>().activeControls = true;
                GetComponent<HealthBar>().maxHealthBar.renderer.enabled = true;
                GetComponent<HealthBar>().remainingHealthBar.renderer.enabled = true;
                renderer.enabled = true;
                foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
                    child.enabled = true;
            }
        }

        if (!dead)
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = 10;
            Vector3 mPos = Camera.main.ScreenToWorldPoint(mouse);

            if (Input.GetMouseButtonDown(1))
            {
                CreateBasicSphere = new GameObject();
                CreateBasicSphere.transform.position = mPos;
            }

            if (Input.GetMouseButtonUp(1) && OSphereTotal <= OSphereCap && (Vector3.Distance(mPos, CreateBasicSphere.transform.position) <= 0.7f))
            {
                bool goCreate = true;
                Debug.Log(ClickObj.id.ToString());
                if (ClickObj.active)
                {
                    if (ClickObj.prefab.GetComponent<BoostSphere>() != null)
                    {
                        foreach (BaseSphere ball in GameObject.FindObjectsOfType<BaseSphere>())
                        {
                            if (ball.GetType() == System.Type.GetType("BoostSphere") && Vector3.Distance(CreateBasicSphere.transform.position, ball.transform.position) < SphereDistance)
                            {
                                goCreate = true;
                                Destroy(ball.gameObject);
                                OSphereTotal--;
                            }
                            if (OSphereCap == OSphereTotal && Vector3.Distance(CreateBasicSphere.transform.position, ball.transform.position) > SphereDistance)
                                goCreate = false;
                        }
                    }
                    else
                    {
                        if ((ClickObj.prefab.name.Contains("ShieldSphere") && gameObject.GetComponentInChildren<ShieldSphere>()) ||
                            (ClickObj.prefab.name.Contains("WaveSphere") && FindObjectOfType<WaveSphere>() != null))
                            goCreate = false;
                    }

                    if (goCreate)
                    {
                        CreateOtherSphere = (GameObject)Instantiate(ClickObj.prefab);
                        CreateOtherSphere.GetComponent<BaseSphere>().Owner = this;

                        if (CreateOtherSphere.GetComponent<ShieldSphere>() != null)
                        {
                            CreateOtherSphere.transform.position = transform.position;
                            CreateOtherSphere.transform.parent = transform;
                        }
                        else
                        {
                            CreateOtherSphere.transform.position = mPos;
                        }
                        OSphereTotal += 1;
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1) && RSphereTotal <= RSphereCap)
                isAiming = true;

            if (GameManager.instance.skillsManager.spheres[3].active)
            {
                if (isAiming && Vector3.Distance(mPos, CreateBasicSphere.transform.position) > 0.7f)
                {
                    if (RedirectMade == false)
                    {
                        bool goCreate = true;
                        foreach (RedirectSphere ball in GameObject.FindObjectsOfType<RedirectSphere>())
                        {
                            if (Vector3.Distance(CreateBasicSphere.transform.position, ball.transform.position) < SphereDistance)
                            {
                                Destroy(ball.gameObject);
                                RSphereTotal--;
                                goCreate = true;
                            }
                            if (RSphereCap == RSphereTotal && Vector3.Distance(CreateBasicSphere.transform.position, ball.transform.position) > SphereDistance)
                            {
                                goCreate = false;
                            }
                        }
                        if (goCreate)
                        {
                            CreateRedirectSphere = (GameObject)Instantiate(RedirectClickObj, CreateBasicSphere.transform.position, Quaternion.identity);
                            RSphereTotal += 1;
                            CreateRedirectSphere.GetComponent<RedirectSphere>().SetOwner(this);
                            RedirectMade = true;
                        }
                    }
                    if (CreateRedirectSphere != null)
                    {
                        Vector3 aimDirection = mPos - CreateRedirectSphere.transform.position;
                        aimDirection.Normalize();
                        float angle = Vector3.Angle(aimDirection, new Vector3(0, 1, 0));
                        Vector3 cross = Vector3.Cross(aimDirection, new Vector3(0, 1, 0));
                        if (cross.z > 0)
                            angle = 360 - angle;

                        CreateRedirectSphere.GetComponent<RedirectSphere>().ChangeDirection(angle, aimDirection, PlayerAliveTimer);
                    }
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                isAiming = false;
                CreateRedirectSphere = null;
                CreateOtherSphere = null;
                RedirectMade = false;
                Destroy(CreateBasicSphere);
            }
        }
    }

    public void nextSphere()
    {
        List<SkillsManager.SetPrefabs> tempSpheres = GameManager.instance.skillsManager.spheres;

        int currentOne = tempSpheres.IndexOf(ClickObj);

        for (int i = currentOne + 1; i != currentOne; i++)
        {
            if (i == (tempSpheres.Count - 1))
                i = 0;

            if (tempSpheres[i].active)
            {
                ClickObj = tempSpheres[i];
                break;
            }
        }
    }

    public void RemoveRSphere()
    {
        RSphereTotal -= 1;
    }

    public void RemoveOSphere()
    {
        OSphereTotal -= 1;
    }

    public int numRedirectSpheresLeft()
    {
        return (RSphereCap - RSphereTotal);
    }

    public int numOtherSpheresLeft()
    {
        return (OSphereCap - OSphereTotal);
    }

    public override void die()
    {
        base.die();

        gameObject.layer = LayerMask.NameToLayer("Default");

        GetComponent<PlayerController>().activeControls = false;

        renderer.enabled = false;
        foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
            child.enabled = false;

        if (lives > 1)
        {
            health.currentHP = health.maxHP;
            Affliction[] possibleAffliction = GetComponents<Affliction>();
            for (int i = possibleAffliction.Length - 1; i >= 0; i--)
                Destroy(possibleAffliction[i]);

            deathTimer = 2.0f;
            lives--;
        }
        else
        {
            reallyDead = true;
            deathTimer = 2.0f;
            //Recode when proper game over has been made
            GameManager.instance.switchToMenu(GameManager.Menus.GameOver);
            gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }

    public void CheckForUpgrade()
    {
        level = experience / 100 + 1;
        skillPoints = level - 1;
        //if (experience / 100 > level - 1)
        //{
        //    level++;
        //    skillPoints++;
        //}
    }
}