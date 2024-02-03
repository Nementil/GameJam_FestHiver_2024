using System.Text.RegularExpressions;
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
    private string test;
    [SerializeField] public Monstre monstre;
    void Start()
    {
        field=GetComponent<TextMeshProUGUI>();
        tf=target.GetComponent<Transform>();
        rb=target.GetComponent<Rigidbody>();
    }

    void Update()
    {
        field.text=RetrievePhysicsData(rb,tf);
        field.text+="\n" + AddText();
    }

    public string RetrievePhysicsData(Rigidbody rb, Transform tf)
    {
        string result="";
        
        result+="Velocity "+rb.velocity.magnitude.ToString("0.0 m/s")+"\n";
        result+="Up "+rb.velocity.y.ToString("0.0 m/s")+"\n";
        return result;
    }
    public void UpdateText(Component sender,object data)
    {
        Debug.Log($"Receiving:{data.GetType()}");
        Debug.Log($"Sender:{sender.GetType()}");
        //if(data is Vector3)
        
        
        test=data.ToString();
    }
    public string AddText()
    {
        string result="";
        result+=monstre.state.ToString() +"\n";
    	result+="Can See Player? "+monstre.canSeePlayer +"\n";
        result+="Anger:"+ monstre.rageCount;
        return result;
    }
}
