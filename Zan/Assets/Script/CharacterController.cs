using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 25.0F;
    public float jumppower = 40.0f;
    public float offsety = 0.0f;
    public Camera camera;
    public float coverdistance = 0.4f;


    private Vector3 moveDirection = Vector3.zero;
    private Animator animator;
    private CharacterController controller;
    private Vector3 movement = new Vector3(0, 0, 1.0f);
    private Vector3 cameravecforward = Vector3.zero;
    private Vector3 cameravecright = Vector3.zero;
    private float velocityY = 0.0f;
    private RaycastHit hit;
    private Ray ray;
    private Ray forwardfromcharacter;
    private Vector3 groudpos;
    private Vector3 beforerot;
    
    private AudioSource audiosource;
    

    



    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        if (!controller || !animator) Debug.Log("controller not found");

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            AnimationReset();
        
            movePlane();



            if (controller.isGrounded) velocityY = 0;
            if (Input.GetButton("Jump") && Physics.Raycast(ray, out hit, 0.3f))
            {
                animator.SetBool("Jump", true);
                velocityY = jumppower;
            }

            if (!controller.isGrounded) velocityY -= gravity * Time.deltaTime ;//重力の処理

            //最終的な移動の処理
            moveDirection.y = velocityY;
             controller.Move(moveDirection * Time.deltaTime);
            //最終的な移動の処理
            

            moveDirection.y = offsety;



            



            if (Input.GetKeyDown(KeyCode.R)) transform.position = new Vector3(0f, 0.8f, 0.4f);
            //場所のリセット

            

        

    }

    /// <summary>
    /// 攻撃時の処理
    /// </summary>
    void Attack()
    {
        //攻撃のボタン判定
        if (Input.GetButtonDown("Attack") || Input.GetMouseButtonDown(0))
        {
            AnimationReset();
            animator.SetTrigger("Attack");
            Debug.Log("Attack!");
        }

        //最後の打ち上げのキャラクターが上がる処理
        

        

        

    }

    

    /// <summary>
    /// アニメーションをリセット
    /// </summary>
    void AnimationReset()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("Jab", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Death", false);
        animator.speed = 1.0f;
    }

    /// <summary>
    /// 平面方向への移動
    /// </summary>
    void movePlane()
    {

        cameravecforward = camera.transform.TransformDirection(Vector3.forward).normalized;
        cameravecforward = new Vector3(cameravecforward.x, 0, cameravecforward.z).normalized;
        cameravecright = new Vector3(cameravecforward.z, 0, -cameravecforward.x).normalized;
        moveDirection = cameravecforward * (Input.GetAxis("Vertical")) + cameravecright * Input.GetAxis("Horizontal");

        moveDirection *= speed;
        
    }

   

    

    
}