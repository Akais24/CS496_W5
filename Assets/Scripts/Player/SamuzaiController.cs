using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using cakeslice;


public class SamuzaiController : NetworkBehaviour {
    
    public int side = 1;
    public GameObject mouseTarget;

    Vector3 newposition;
    public float speed;
    public float walkRange;
    public float QattackRange = 20.0f;
    public float RattackRange;
    public float AttackDistance;
    public float shootDistance = 10f;
    public float shootRate = .5f;

    private bool moving = false;
    private bool running = false;
    private bool first = true;
    private bool Rdowned = false;

    public Image Qimage;
    public Image Wimage;
    public Image Eimage;
    public Image Rimage;

    float energycount = 0f;
    ChampHealthSystem health;

    public GameObject graphics;
    public Animation anim;


    public float basicAttackRange = 2.0f;
    public GameObject savedTarget;

    public const float q_cool = 5.0f;
    public float q_cool_count = q_cool;
    public const float w_cool = 5.0f;
    public float w_cool_count = w_cool;
    public const float e_cool = 5.0f;
    public float e_cool_count = e_cool;
    public const float r_cool = 10.0f;
    public float r_cool_count = r_cool;

    float nuckbackTime = 0.0f;
    Vector3 nuckbackDest;
    
    float gatherTime = 0.0f;
    Vector3 gatherDest;
    float gatherSpeed = 10.0f;

    // Use this for initialization
    void Awake () {
        newposition = this.transform.position;
        anim = GetComponent<Animation>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if (gatherTime > 0)
        {
            gatherTime -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, gatherDest, gatherSpeed * Time.deltaTime);
            return;
        }
        if (nuckbackTime > 0)
        {
            nuckbackTime -= Time.deltaTime;
            //NuckBack();
            return;
        }

        bool RMB = Input.GetMouseButtonDown(1);
        bool Q = Input.GetKeyDown(KeyCode.Q);
        bool W = Input.GetKeyDown(KeyCode.W);
        bool E = Input.GetKeyDown(KeyCode.E);
        bool R = Input.GetKeyDown(KeyCode.R);

        q_cool_count += Time.deltaTime;
        w_cool_count += Time.deltaTime;
        e_cool_count += Time.deltaTime;
        r_cool_count += Time.deltaTime;

        if (RMB)
        {
            Debug.Log("Right Mouse Click");
            SetTargetPosition();

        }
        if(moving) Move();


        if (Q)
        {
            if (q_cool_count < q_cool) return;
            q_cool_count = 0;
            //Qimage.fillAmount = 0;
            moving = false;
            anim["Jump"].wrapMode = WrapMode.Once;
            anim.Play("Jump");         
            Invoke("QSkill", 0.5f);

        }
        if (W)
        {
            if (w_cool_count < w_cool) return;

            w_cool_count = 0.0f;
            //Wimage.fillAmount = 0;
            
            anim.Play("Run");
            running = true;
            speed += 3;
            Invoke("WSkill", 3);
        }
        if (E)
        {
            //Eimage.fillAmount = 0;
            if (e_cool_count < e_cool) return;
            moving = false;
            Invoke("ESkill", 0.5f);
        }
        //if (R)
        //{
        //    if (r_cool_count < r_cool) return;
        //    if (Rdowned)
        //    {
               
        //        Rdowned = false;
        //        enemyoutline.color = 1;
        //    }
        //    else
        //    {
        //        Rdowned = true;
        //    }

        //    moving = false;
            
        //}

        //if (Rdowned)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Enemy" && (Vector3.Distance(enemy.transform.position, this.transform.position) < RattackRange))
        //    {
        //        enemyoutline.color = 2;
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            r_cool_count = 0.0f;
        //            Rimage.fillAmount = 0;
        //            enemyoutline.color = 1;
        //            this.animation.Play("Jump");
        //            Invoke("RSkill", 0.5f);
        //            newposition = enemy.transform.position;
        //            Quaternion transRot = Quaternion.LookRotation(newposition - this.transform.position, Vector3.up);
        //            graphics.transform.rotation = Quaternion.Slerp(transRot, graphics.transform.rotation, 0.2f);
        //            Rdowned = false;
        //        }
        //    }
        //}

        //Qimage.fillAmount = q_cool_count / q_cool;
        //Wimage.fillAmount = w_cool_count / w_cool;
        //Eimage.fillAmount = e_cool_count / e_cool;
        //Rimage.fillAmount = r_cool_count / r_cool;



    }

    void SetTargetPosition()
    {
        moving = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit);
        newposition = hit.point;
        Quaternion transRot = Quaternion.LookRotation(newposition - this.transform.position, Vector3.up);
        gameObject.transform.rotation = Quaternion.Slerp(transRot, gameObject.transform.rotation, 0.2f);

        if (mouseTarget != null)
        {
            float distance = Vector3.Distance(newposition, transform.position);
            savedTarget = mouseTarget;
            if (distance < basicAttackRange)
            {
                Debug.Log("Direct Arrack");
                moving = false;
                Attack();
            }
            else
            {
                Debug.Log("Move for Arrack");
                newposition += basicAttackRange * Vector3.Normalize(transform.position - newposition);
            }
        }
    }

    void Move()
    {
        //    if (running)
        //    {
        //        anim.Play("Run");

        //    }
        //    else
        //    {
        //        anim.Play("Walk");
        //    }
        //    if ( Vector3.Distance(newposition, this.transform.position) > walkRange)
        //{
        //        this.transform.position = Vector3.MoveTowards(this.transform.position, newposition, speed * Time.deltaTime);

        //    }

        if (running) anim.Play("Run");
        else anim.Play("Walk");

        if (savedTarget != null)
        {
            if (Vector3.Distance(transform.position, savedTarget.transform.position) <= basicAttackRange)
            {
                moving = false;
                Attack();
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, savedTarget.transform.position, speed * Time.deltaTime);
            }
        }
        else
        {
            if (Vector3.Distance(newposition, this.transform.position) > walkRange)
                this.transform.position = Vector3.MoveTowards(this.transform.position, newposition, speed * Time.deltaTime);
            else
            {
                moving = false;
                anim.Play("idle");
            }
        }

    }

    public void Gather(Vector3 dest)
    {
        gatherDest = dest;
        gatherTime = 0.5f;
    }

    void NuckBack()
    {
        float speed = 10.0f;
        if (nuckbackDest == null)
        {
            nuckbackTime = 0.0f;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, nuckbackDest, speed * Time.deltaTime);
    }

    void Attack()
    {
        Debug.Log("BASIC ATTACK");
        moving = false;

        //Debug.Log("Enemy");
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        //health = enemy.GetComponent<ChampHealthSystem>();
        //health.TakeDamage(2);
        //anim["Attack"].wrapMode = WrapMode.Once;
        //anim.Play("Attack");
        //if (targetedEnemy == null)
        //    return;
        //enemyClicked = false;

        if (savedTarget.GetComponentInParent<HealthSystem>() != null)
        {
            savedTarget.GetComponentInParent<HealthSystem>().TakeDamage(10);
            anim.Play("Attack");
            Invoke("setStay", 1.0f);
            savedTarget = null;
        }

    }

    void setStay()
    {
        anim.Play("idle");
    }

    void QSkill()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        //if (Vector3.Distance(enemy.transform.position, this.transform.position) < QattackRange)
        //{

        //    health = enemy.GetComponent<ChampHealthSystem>();
        //    health.Jump();
        //    health.TakeDamage(4);
        //    health. MoveTo(this.transform.position);

        //}

        GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
        GameObject[] champions = GameObject.FindGameObjectsWithTag("Champion");

        GameObject target;
        for(int i=0; i<minions.Length; i++)
        {
            target = minions[i];
            if(Vector3.Distance(transform.position, target.transform.position) <= QattackRange)
            {
                Vector3 dest = transform.position + (target.transform.position - transform.position) / 2;
                target.GetComponent<MinionState>().Gather(dest);
            }
        }
        for (int i = 0; i < champions.Length; i++)
        {
            target = champions[i];
            if (Vector3.Distance(transform.position, target.transform.position) <= QattackRange)
            {
                Vector3 dest = transform.position + (target.transform.position - transform.position) / 2;
                if(target.GetComponent<ScifiController>() != null) target.GetComponent<ScifiController>().Gather(dest);
                if (target.GetComponent<SamuzaiController>() != null) target.GetComponent<SamuzaiController>().Gather(dest);
            }
        }

    }

    void WSkill()
    {
        
        speed -= 3;
        running = false;
    }

    void ESkill()
    {
        e_cool_count = 0.0f;
        GetComponent<ChampHealthSystem>().Healthier();
    }

    void RSkill()
    {
        //this.transform.position = enemy.transform.position;
        //anim.Play("Attack");
        //health.TakeDamage(4);

    }

}
