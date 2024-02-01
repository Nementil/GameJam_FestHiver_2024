using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;


public class UI_Debug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI field;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform tf;
    [SerializeField] private Rigidbody rb;
    void Start()
    {
        field=GetComponent<TextMeshProUGUI>();
        tf=target.GetComponent<Transform>();
        rb=target.GetComponent<Rigidbody>();
    }

    void Update()
    {
        field.text=RetrievePhysicsData(rb,tf);
    }

    public string RetrievePhysicsData(Rigidbody rb, Transform tf)
    {
        string result="";
        
        result+="Velocity "+rb.velocity.magnitude.ToString("0.0 m/s")+"\n";
        result+="Up "+rb.velocity.y.ToString("0.0 m/s")+"\n";
        return result;
    }
}
