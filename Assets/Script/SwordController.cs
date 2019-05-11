using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public float swordtrans=0;
    private float swordtransoffset = 0;
    private Vector3 pos;
    [SerializeField]
    private float offset = 5;
    [SerializeField]
    private GameObject enem;
    [SerializeField]
    private GameObject swordmesh;
    private Animator swordanimator;
    private bool first = true;
    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position;
        Input.gyro.enabled = true;
        swordanimator = enem.GetComponent<Animator>();
        Input.compass.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if((swordtrans<0.45f&&Input.gyro.rotationRateUnbiased.z>0)||(swordtrans>-0.45f&&Input.gyro.rotationRateUnbiased.z<0))swordtrans += Input.gyro.rotationRateUnbiased.z*offset;

        //Debug.Log(swordtrans);
        if (swordmesh.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("New State")) this.transform.position = new Vector3(swordtrans, 0, 0) + pos;
        else
        {
            this.transform.position = pos;
        }
        
    }
}
