using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float slashpowermax = 3;
    public float slashpowermin = 1.5f;
    private Vector3 acceleration;
    private Vector3 accelerationbef;
    private Animator animator;
    private float time = 0;
    private Transform gravity;
    private Transform gravitybef;
    private Vector3 power;
    private float gyrozbef;
    private float kakudo=0;
    private bool left = false;
    private bool slash = false;
    private bool renzoku = false;
    private bool renzokubef = false;
    [SerializeField]
    private Text txt;
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject swordroot;
    [SerializeField]
    private GameObject Cube;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        Input.gyro.enabled = true;
        //StartCoroutine("Sample");
        Input.compass.enabled = true;
        Input.location.Start();
    }

    // Update is called once per frame
    void Update()
    {
        animator.ResetTrigger("HitR");
        animator.ResetTrigger("HitL");
        sword.GetComponent<Animator>().ResetTrigger("SlashL");
        sword.GetComponent<Animator>().ResetTrigger("SlashR");

        acceleration = Input.acceleration;
        //gravity=this.transform;
        //gravity.rotation = Input.gyro.attitude;

        //power = acceleration - gravity.forward;

        Debug.Log("x" + Input.compass.rawVector.x + " y" + Input.compass.rawVector.y + " z" + Input.compass.rawVector.z);
        Debug.Log("Head" + Input.compass.magneticHeading + "truehead" + Input.compass.trueHeading);
        if ((acceleration - accelerationbef).magnitude > slashpowermax)
        {
            slash = true;
            gyrozbef = Input.gyro.attitude.z;
        }
        if (slash )
        {
            kakudo += Input.gyro.rotationRateUnbiased.z;
            if ((acceleration - accelerationbef).magnitude<slashpowermin&&kakudo<0 && animator.GetCurrentAnimatorStateInfo(0).IsName("2Hand-Sword-Idle"))
            {
                animator.SetTrigger("HitR");
                animator.ResetTrigger("HitL");
                sword.GetComponent<Animator>().SetTrigger("SlashL");
                sword.GetComponent<Animator>().ResetTrigger("SlashR");
                this.GetComponent<AudioSource>().Play();
                slash = false;
                time = 0;
                kakudo = 0;
                //Vibration.Vibrate(1000);
            }
            else if ((acceleration - accelerationbef).magnitude < slashpowermin && kakudo > 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("2Hand-Sword-Idle"))
            {
                animator.SetTrigger("HitL");
                animator.ResetTrigger("HitR");
                sword.GetComponent<Animator>().SetTrigger("SlashR");
                sword.GetComponent<Animator>().ResetTrigger("SlashL");
                slash = false;
                this.GetComponent<AudioSource>().Play();
                time = 0;
                kakudo = 0;
                Vibration.Vibrate(100);
            }
        }
        if (Input.gyro.rotationRateUnbiased.z > 0) Cube.GetComponent<MeshRenderer>().enabled = false;
        else Cube.GetComponent<MeshRenderer>().enabled = true;

        if (swordroot.GetComponent<SwordController>().swordtrans < -0.2f&& animator.GetCurrentAnimatorStateInfo(0).IsName("2Hand-Sword-Attack4"))
        {
            animator.SetTrigger("Hajiku");
            Debug.Log("Hajiku");
            this.GetComponents<AudioSource>()[1].Play();
        }
        if(swordroot.GetComponent<SwordController>().swordtrans > 0.2f&&animator.GetCurrentAnimatorStateInfo(0).IsName("2Hand-Sword-Attack2"))
        {
            animator.SetTrigger("Hajiku");
            Debug.Log("Hajiku");
            this.GetComponents<AudioSource>()[1].Play();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("2Hand-Sword-Idle")) animator.ResetTrigger("Hajiku");
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("2Hand-Sword-Attack4") || animator.GetCurrentAnimatorStateInfo(0).IsName("2Hand-Sword-Attack2")) Debug.Log("Attack");
        accelerationbef = acceleration;
        
    }

    private IEnumerator Sample()
    {
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(3.0f);
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine("Sample");
    }
}
