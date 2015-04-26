using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    private Health health;

    private GameObject remainingHealthBar;
    private GameObject maxHealthBar;

    public float scaler = 10;

    Bounds hpbounds;
    // Use this for initialization
    private void Start()
    {
        health = gameObject.GetComponent<Health>();

        maxHealthBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        remainingHealthBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
    public struct damagelabel
    {
        public float amount;
        public Rect position;
        public int fontSize;
        public GUIStyle style;
        public float lifetimemax;
        public float lifetime;
        public float offset;

        public void Adjust()
        {
            position = new Rect(position.x, position.y + offset, position.width, position.height);

            style.fontSize = (int)(fontSize * (lifetime/lifetimemax)) + 1;
        }

        public void Display()
        {
            GUI.Label(
                new Rect(Camera.main.WorldToScreenPoint(position.min).x,
                    -Camera.main.WorldToScreenPoint(position.max).y + Screen.height - offset,
                    Camera.main.WorldToScreenPoint(position.max).x,
                    -Camera.main.WorldToScreenPoint(position.min).y + Screen.height),
                    ((int)amount).ToString(),
                    style);
        }
    }

    public damagelabel[] damagelabels;
    public int maxlabels = 100;
    public float labelspeed = 0.1f;
    public float labellife = 1.0f;

	// Use this for initialization
	void Start () {
		health = gameObject.GetComponent<Health> ();

        maxHealthBar.renderer.material.color = Color.black;
        remainingHealthBar.renderer.material.color = Color.red;

        Destroy(maxHealthBar.collider);
        Destroy(remainingHealthBar.collider);
		maxHealthBar.renderer.material.color = Color.black;
		remainingHealthBar.renderer.material.color = Color.red;

        damagelabels = new damagelabel[maxlabels];
        /*for (int i = 0; i < damagelabels.Length; ++i)
        {
            damagelabels[i] = new damagelabel();
            damagelabels[i].lifetime = -0.1f;
        }*/
	}
	
	// Update is called once per frame
	void LateUpdate () {
        PositionBar();

        //Damagelabel pos updating
        for (int i = 0; i < damagelabels.Length; ++i )
        {
            if (damagelabels[i].lifetime > 0.0f)
            {
                damagelabels[i].Adjust();
                damagelabels[i].lifetime -= Time.deltaTime;
            }
        }
	}
        GameObject dummyChild = new GameObject();

        dummyChild.transform.parent = transform;
        dummyChild.name = "Healthbar";
        maxHealthBar.transform.parent = dummyChild.transform;
        remainingHealthBar.transform.parent = dummyChild.transform;
    }
    void OnGUI(){
        for (int i = 0; i < damagelabels.Length; ++i)
        {
            if (damagelabels[i].lifetime > 0.0f)
                damagelabels[i].Display();
        }

    }
    // Update is called once per frame
    private void LateUpdate()
    {
        if (maxHealthBar != null && remainingHealthBar != null)
        {
            maxHealthBar.transform.position = new Vector3(transform.position.x,
                                                          renderer.bounds.max.y + 1f,
                                                          1.0f);

            maxHealthBar.transform.localScale = new Vector3((transform.lossyScale.x / 3),
                                                             0.5f,
                                                             1);
            remainingHealthBar.transform.localScale = new Vector3((transform.lossyScale.x / 3) * (health.currentHP / health.maxHP),
                                                                   0.5f,
                                                                   1);

            remainingHealthBar.transform.position = new Vector3(maxHealthBar.renderer.bounds.min.x
                                                                    - ((maxHealthBar.renderer.bounds.min - maxHealthBar.transform.position)
                                                                        * (health.currentHP / health.maxHP)).x,
                                                                maxHealthBar.transform.position.y,
                                                                0.5f);
        }
    }
}
    void PositionBar(){
        if (maxHealthBar != null && remainingHealthBar != null)
        {

            maxHealthBar.transform.position = new Vector3(transform.position.x,
                                                          renderer.bounds.max.y + 1f,
                                                          1.0f);

            maxHealthBar.transform.localScale = new Vector3((transform.lossyScale.x / 3),
                                                             0.5f,
                                                             1);
            remainingHealthBar.transform.localScale = new Vector3((transform.lossyScale.x / 3) * (health.currentHP / health.maxHP),
                                                                   0.5f,
                                                                   1);

            remainingHealthBar.transform.position = new Vector3(maxHealthBar.renderer.bounds.min.x
                                                                    - ((maxHealthBar.renderer.bounds.min - maxHealthBar.transform.position)
                                                                        * (health.currentHP / health.maxHP)).x,
                                                                maxHealthBar.transform.position.y,
                                                                0.5f);

            if (maxHealthBar != null)
                hpbounds = maxHealthBar.renderer.bounds;

            if (health.currentHP <= 0.0f)
            {
                Destroy(maxHealthBar);
                Destroy(remainingHealthBar);
            }

            if (Input.GetKeyDown(KeyCode.P))
                health.TakeDamage(Random.Range(1.0f, 2.0f), false);
            else if (Input.GetKeyDown(KeyCode.O))
                health.TakeDamage(Random.Range(3.0f, 4.0f), true);
            else if (Input.GetKeyDown(KeyCode.I))
                health.Heal(Random.Range(1.0f, 10.0f));
        }
    }

    public void CreateDamageLabel(float dmg, bool isHealing, bool isCrit){
        damagelabel tmp = new damagelabel();

        tmp.amount = dmg;

        tmp.style = new GUIStyle();

        if (isHealing)
            tmp.style.normal.textColor = Color.green;
        else
            tmp.style.normal.textColor = Color.red;

        if (isCrit)
        {
            tmp.fontSize = 25;
        }
        else
            tmp.fontSize = 12;

        tmp.position = new Rect(hpbounds.min.x, hpbounds.max.y, 0, 0);

        /*tmp.position = new Rect(Camera.main.WorldToScreenPoint(hpbounds.min).x,
                                -Camera.main.WorldToScreenPoint(hpbounds.max).y + Screen.height,
                                Camera.main.WorldToScreenPoint(hpbounds.max).x,
                                -Camera.main.WorldToScreenPoint(hpbounds.min).y + Screen.height);*/

        //rect_Label = Rect (objPos.x - XOffset, -(objPos.y - Screen.height) - YOffset, textSize.x*2, textSize.y*2)

        tmp.lifetimemax = tmp.lifetime = labellife;
        tmp.offset = labelspeed;

        for(int i = 0; i < damagelabels.Length; ++i)
        {
            if (damagelabels[i].lifetime <= 0.0f)
            {
                damagelabels[i] = tmp;
                break;
            }
        }
    }
}
