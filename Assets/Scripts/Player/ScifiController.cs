using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using cakeslice;

public class ScifiController : NetworkBehaviour
{
    public int side = 1;
    public GameObject mouseTarget;

    public Vector3 newposition;

    public GameObject gun;

    public float speed;
    public float walkRange;
    public float QattackRange;
    public float AttackDistance;
    public float shootDistance = 10f;
    public float shootRate = .5f;
    public float shootRange = 300f;
    private Transform targetedEnemy;

    private bool moving = false;

    float energycount = 0f;

    Actions actions;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    Light gunLight;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;

    public float basicAttackRange = 7.0f;
    public GameObject savedTarget;
    
    public const float q_cool = 5.0f;
    public float q_cool_count = q_cool;
    private bool Wdowned = false;
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
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        newposition = this.transform.position;
        actions = GetComponent<Actions>();

        gunParticles = gun.GetComponent<ParticleSystem>();
        gunLine = gun.GetComponent<LineRenderer>();
        gunLight = gun.GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {
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
            NuckBack();
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
        if (moving)
        {
            Move();
        }

        if (Q)
        {
            if (q_cool_count < q_cool || mouseTarget == null) return;

            float distance = Vector3.Distance(mouseTarget.transform.position, transform.position);
            float range = 16.0f;
            if (distance > range) return;

            actions.Aiming();
            transform.LookAt(mouseTarget.transform);
            //QSkill();
            moving = false;
            Invoke("QSkill", 0.5f);
        }


        if (W)
        {
            if (Wdowned)
            {
                Wdowned = false;
                DisableEffects();
                actions.Stay();
            }
            else
            {
                if (w_cool_count < w_cool) return;
                Wdowned = true;
            }
            moving = false;
        }

        if (Wdowned)
        {
            actions.Aiming();

            gunLine.enabled = true;
            gunLine.SetPosition(0, gun.transform.position);
            shootRay.origin = gun.transform.position;
            shootRay.direction = - gun.transform.up;
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * shootRange);

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //Physics.Raycast(ray, out hit);
            //newposition = hit.point;
            //Debug.Log(newposition.ToString());
            //Quaternion transRot = Quaternion.LookRotation(newposition - this.transform.position, Vector3.forward);
            //transform.rotation = Quaternion.Slerp(transRot, gameObject.transform.rotation, 0.2f);

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("USE W");
                WSkill();
                DisableEffects();
            }

        }

        if (E)
        {
            if (e_cool_count < e_cool) return;
            moving = false;
            Invoke("ESkill", 0.5f);
        }
        if (R)
        {
            if (r_cool_count < r_cool) return;
            moving = false;
            InvokeRepeating("Rotate", 0, 0.005f);
            Invoke("CancleR", 2);

        }

    }
    public void Gather(Vector3 dest)
    {
        gatherDest = dest;
        gatherTime = 0.5f;
    }


    void SetTargetPosition()
    {
        savedTarget = null;
        moving = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit);
        newposition = hit.point;
        Quaternion transRot = Quaternion.LookRotation(newposition - this.transform.position, Vector3.up);
        gameObject.transform.rotation = Quaternion.Slerp(transRot, gameObject.transform.rotation, 0.2f);
        
        if(mouseTarget != null)
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

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Ground")
        //{
        //    enemyClicked = false;
        //    newposition = hit.point;
        //    Quaternion transRot = Quaternion.LookRotation(newposition - this.transform.position, Vector3.up);
        //    graphics.transform.rotation = Quaternion.Slerp(transRot, graphics.transform.rotation, 0.2f);
        //}

        //if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Player")
        //{

        //    Debug.Log("Enemy");
        //    targetedEnemy = hit.transform;
        //    newposition = targetedEnemy.position;
        //    Quaternion transRot = Quaternion.LookRotation(newposition - this.transform.position, Vector3.up);
        //    graphics.transform.rotation = Quaternion.Slerp(transRot, graphics.transform.rotation, 0.2f);
        //    enemyClicked = true;
        //}
    }

    void Move()
    {
        if (Wdowned) newposition = Vector3.zero;
        if (newposition == Vector3.zero) return;

        actions.Walk();
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
                actions.Stay();
            }
        }
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

        //health = enemy.GetComponent<SamuHealth>();
        //health.TakeDamage(1, 0);
        //actions.Attack();
        //if (targetedEnemy == null)
        //    return;
        //enemyClicked = false;

        if (savedTarget.GetComponentInParent<HealthSystem>() != null)
        {
            savedTarget.GetComponentInParent<HealthSystem>().TakeDamage(10);
            actions.Attack();
            Invoke("setStay", 0.5f);
            savedTarget = null;
        }

    }

    void setStay()
    {
        actions.Stay();
    }

    void QSkill()
    {
        // enemyoutline.color = 1;
        //enemy = GameObject.FindGameObjectWithTag("Player");
        //health = enemy.GetComponent<SamuHealth>();
        //actions.Attack();
        //health.TakeDamage(1, 1);
        //this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position-enemy.transform.position, 5);

        if (mouseTarget.GetComponentInParent<HealthSystem>() != null)
        {
            q_cool_count = 0.0f;
            Debug.Log("Q fire");
            mouseTarget.GetComponentInParent<HealthSystem>().TakeDamage(20);
            actions.Attack();
            nuckbackTime = 0.5f;
            nuckbackDest = transform.position + Vector3.Normalize(transform.position - mouseTarget.transform.position) * 5;
        }
        actions.Stay();

    }

    void WSkill()
    {
        w_cool_count = 0.0f;

        shootRay.origin = gun.transform.position - new Vector3(0, gun.transform.position.y - 0.5f, 0);
        shootRay.direction = -gun.transform.up - new Vector3(0, -gun.transform.up.y, 0);
        if (Physics.Raycast(shootRay, out shootHit, shootRange, shootableMask))
        {
            //GameObject touchObject = (GameObject) shootHit.collider;
            Debug.Log("SHOOT!");
            if(shootHit.collider.GetComponentInParent<HealthSystem>() != null)
            {
                Debug.Log("HIT!");
                shootHit.collider.GetComponentInParent<HealthSystem>().TakeDamage(50);
            }
            if (shootHit.collider.GetComponent<HealthSystem>() != null)
            {
                Debug.Log("HIT!");
                shootHit.collider.GetComponent<HealthSystem>().TakeDamage(50);
            }
            //SamuHealth health = shootHit.collider.GetComponent<SamuHealth>();
            //if (health != null)
            //{
            //    health.TakeDamage(1, 3);
            //}
            //gunLine.SetPosition(1, shootHit.point);
        }
        //else
        //{
        //    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * shootRange);
        //}
        ////nontargeting attack
        Wdowned = false;
        DisableEffects();
        actions.Stay();
    }

    void ESkill()
    {
        e_cool_count = 0.0f;
        GetComponent<ChampHealthSystem>().Healthier();
    }

    void Rotate()
    {
        //newposition = enemy.transform.position;
        //r_cool_count = 0.0f;
        //gameObject.transform.Rotate(Vector3.up, 5f);
        ////gunAudio.Play();
        //gunLight.enabled = true;

        //gunParticles.Stop();
        //gunParticles.Play();

        //gunLine.enabled = true;
        //gunLine.SetPosition(0, gun.transform.position);
        //shootRay.origin = gun.transform.position;
        //shootRay.direction = -gun.transform.up;

        //actions.Aiming();
        //if (Physics.Raycast(shootRay, out shootHit, shootRange, shootableMask))
        //{
        //    SamuHealth health = shootHit.collider.GetComponent<SamuHealth>();
        //    if (health != null)
        //    {
        //        health.TakeDamage(1, 4);
        //    }
        //    gunLine.SetPosition(1, shootHit.point);
        //}
        //else
        //{
        //    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * shootRange);
        //}



        //얘가 쳐다보는 방향에 적있으면 helath.TakeDamage(1,4);
    }

    void CancleR()
    {
        CancelInvoke("Rotate");
        DisableEffects();
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }
}
