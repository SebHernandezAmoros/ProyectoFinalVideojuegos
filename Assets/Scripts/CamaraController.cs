using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public GameObject Target;
    private Vector3 TargetPos;

    public float HaciaAdelante;
    public float Suavizado;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TargetPos = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);

        if (Target.transform.localScale.x == 5)
        {
            TargetPos = new Vector3(TargetPos.x +HaciaAdelante, TargetPos.y, transform.position.z);
        }
        else
        {
            TargetPos = new Vector3(TargetPos.x - HaciaAdelante, TargetPos.y, transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position, TargetPos, Suavizado * Time.deltaTime);
    }
}
