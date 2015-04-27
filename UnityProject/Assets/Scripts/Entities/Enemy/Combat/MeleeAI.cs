using UnityEngine;
using System.Collections;

public class MeleeAI : AttackAI {

    public float range;

    bool swinging = false;
    bool halfswung = false;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        AttackCheck();
	}

    private void AttackCheck()
    {
        //SWORD CODE
        if (!swinging)
        {
            if (!coordinator.currentMovement.direction && weapon.transform.rotation != Quaternion.Euler(0, 0, 45))
            {
                weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation,
                                                                  Quaternion.Euler(0, 0, 45),
                                                                  (8 * Time.deltaTime));
                weapon.transform.position = Vector3.Lerp(weapon.transform.position,
                                                              weapon.transform.parent.position + new Vector3(-0.5f, 0.5f, -1f),
                                                              (8 * Time.deltaTime));
            }
            else if (coordinator.currentMovement.direction && weapon.transform.rotation != Quaternion.Euler(0, 0, -45))
            {
                weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation,
                                                                  Quaternion.Euler(0, 0, -45),
                                                                  (8 * Time.deltaTime));
                weapon.transform.position = Vector3.Lerp(weapon.transform.position,
                                                              weapon.transform.parent.position + new Vector3(0.5f, 0.5f, -1f),
                                                              (8 * Time.deltaTime));
            }

            if (((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude < range) &&
                Random.Range(1, 3) < 2 &&
                coordinator.currentMovement.GetType() == System.Type.GetType("Seek_Movement") &&
                !swinging)
            {
                swinging = true;
            }
        }
        else
        {
            Quaternion toRot;
            Vector3 toPos;
            if (!halfswung)
            {
                if (coordinator.currentMovement.direction)
                {
                    toRot = Quaternion.Euler(0, 0, -90);
                    toPos = new Vector3(1f, -0.3f, -1f);
                }
                else
                {
                    toRot = Quaternion.Euler(0, 0, 90);
                    toPos = new Vector3(-1f, -0.3f, -1f);
                }
            }
            else
            {
                if (coordinator.currentMovement.direction)
                {
                    toRot = Quaternion.Euler(0, 0, 0);
                    toPos = new Vector3(0.5f, 0.5f, -1f);
                }
                else
                {
                    toRot = Quaternion.Euler(0, 0, 0);
                    toPos = new Vector3(-0.5f, 0.5f, -1f);
                }
            }

            weapon.transform.rotation = Quaternion.Slerp(weapon.transform.rotation,
                                                              toRot,
                                                              (16 * Time.deltaTime));
            weapon.transform.position = Vector3.Lerp(weapon.transform.position,
                                                          weapon.transform.parent.position + toPos,
                                                          (16 * Time.deltaTime));

            if (Quaternion.Angle(weapon.transform.rotation, toRot) < 0.1f && !halfswung)
            {
                halfswung = true;
            }
            else if (Quaternion.Angle(weapon.transform.rotation, toRot) < 0.1f && halfswung)
            {
                halfswung = false;
                swinging = false;
            }
        }
    }
}
