		﻿

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public List<Sphere> Spheres;
    public List<Arrow> Arrows;
    public List<Sword> Swords;
    private Sword ActiveSword;
    private Arrow ActiveArrow;
    private Sphere ActiveSphere;
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
    private int RSphereTotal = 0;
    public int RSphereCap = 3;
    private int BSphereTotal = 0;
    public int BSphereCap = 3;
    private Vector3 spawnpos;
    public int lives = 3;

    // Use this for initialization
    private void Start()
    {
        spawnpos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
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
                if (Vector3.Distance(mPos, ball.transform.position) < SphereDistance && goCreate)
                {
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
                CreateBoostSphere = (GameObject)Instantiate(ClickObjBoost, CreateBasicSphere.transform.position, Quaternion.identity);
                BSphereTotal += 1;
                CreateBoostSphere.GetComponent<BoostSphere>().SetOwner(this);
                //CreateBoostSphere.GetComponent<Animator> ().Play();
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
                    if (Vector3.Distance(CreateBasicSphere.transform.position, ball.transform.position) < SphereDistance && goCreate)
                    {
                        Destroy(ball.gameObject);
                        RSphereTotal--;
                        goCreate = false;
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
        if (lives <= 0)
        {
            health.currentHP = health.maxHP;
            transform.position = spawnpos;
            lives--;
        }
        else
        {
            //Recode when proper game over has been made
            Application.LoadLevel("MainMenu");
        }
    }
}