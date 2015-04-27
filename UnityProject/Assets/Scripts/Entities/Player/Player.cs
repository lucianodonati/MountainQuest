using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Arrow arrow;
    public bool isAiming = false;
    public GameObject instructionsUI;
    public GameObject ClickObj;
    public GameObject ClickObjBoost;
    public GameObject basicClickObj;
    private GameObject CreateRedirectSphere;
    private GameObject CreateBoostSphere;
    private GameObject CreateBasicSphere;
    private bool RedirectMade = false;
    public float SphereDistance = 7;
    public float PlayerAliveTimer = 8.0f;
    public int RSphereTotal = 0;
    public int RSphereCap = 3;
    public int BSphereTotal = 0;
    public int BSphereCap = 3;
    public int lives = 3;
    private float deathTimer = 0.0f;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (deathTimer >= 0)
        {
            deathTimer -= Time.deltaTime;
            renderer.enabled = false;
            GetComponent<HealthBar>().maxHealthBar.renderer.enabled = false;
            GetComponent<HealthBar>().remainingHealthBar.renderer.enabled = false;
        }
        else
        {
            GetComponent<HealthBar>().maxHealthBar.renderer.enabled = true;
            GetComponent<HealthBar>().remainingHealthBar.renderer.enabled = true;
            renderer.enabled = true;
        }

        Vector3 mouse = Input.mousePosition;
        mouse.z = 10;
        Vector3 mPos = Camera.main.ScreenToWorldPoint(mouse);

        if (Input.GetMouseButtonDown(1))
        {
            CreateBasicSphere = (GameObject)GameObject.Instantiate(basicClickObj, mPos, Quaternion.identity);
        }

        if (Input.GetMouseButtonUp(1) && BSphereTotal <= BSphereCap && (Vector3.Distance(mPos, CreateBasicSphere.transform.position) <= 0.7))
        {
            bool goCreate = true;
            foreach (BoostSphere ball in GameObject.FindObjectsOfType<BoostSphere>())
            {
                if (Vector3.Distance(CreateBasicSphere.transform.position, ball.transform.position) < SphereDistance)
                {
                    goCreate = true;
                    Destroy(ball.gameObject);
                    BSphereTotal--;
                }
                if (BSphereCap == BSphereTotal && Vector3.Distance(CreateBasicSphere.transform.position, ball.transform.position) > SphereDistance)
                {
                    goCreate = false;
                }
            }

            if (goCreate)
            {
                CreateBoostSphere = (GameObject)Instantiate(ClickObjBoost, mPos, Quaternion.identity);
                BSphereTotal += 1;
                CreateBoostSphere.GetComponent<BoostSphere>().SetOwner(this);
            }
        }
        else if (Input.GetMouseButtonDown(1) && RSphereTotal <= RSphereCap)
            isAiming = true;

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
                    CreateRedirectSphere = (GameObject)Instantiate(ClickObj, CreateBasicSphere.transform.position, Quaternion.identity);
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

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            CreateRedirectSphere = null;
            CreateBoostSphere = null;
            RedirectMade = false;
            Destroy(CreateBasicSphere);
        }
    }

    public void RemoveRSphere()
    {
        RSphereTotal -= 1;
    }

    public void RemoveBSphere()
    {
        BSphereTotal -= 1;
    }

    public int numRedirectSpheresLeft()
    {
        return (RSphereCap - RSphereTotal);
    }

    public int numBoostSpheresLeft()
    {
        return (BSphereCap - BSphereTotal);
    }

    public override void die()
    {
        if (lives > 1)
        {
            health.currentHP = health.maxHP;
            transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
            Affliction[] possibleAffliction = GetComponents<Affliction>();
            for (int i = possibleAffliction.Length - 1; i >= 0; i--)
                Destroy(possibleAffliction[i]);

            deathTimer = 2.0f;
            lives--;
        }
        else
        {
            if (health.currentHP <= 0)
            {
                //Recode when proper game over has been made
                GameManager.instance.switchToMenu(GameManager.Menus.GameOver);
                gameObject.GetComponent<PlayerController>().enabled = false;
            }
        }
        SoundFX sfx = GetComponent<SoundFX>();
        if (sfx != null)
            sfx.Play("Die");
    }
}