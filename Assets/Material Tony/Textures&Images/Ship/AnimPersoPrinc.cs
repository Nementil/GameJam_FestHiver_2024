using UnityEngine;
using System.Collections;


public class AnimAK47 : MonoBehaviour { 

    private Animator Anim;

    public Animator AnimFlamme;
    void Start() {
        Anim = GetComponent<Animator>();
    }


   
    void Update() {

        // Fire Animation
        if (Input.GetButton("Fire1"))
        {
            Anim.SetBool("LightShot", true);
           
        }
        else
        {
            Anim.SetBool("LightShot", false);

        }

  
       //Walk Animation
       if(Input.GetAxis("Vertical")!=0 && !Input.GetKey(KeyCode.LeftShift))
       {
            Anim.SetBool("Walk", true);
    
       }else
        {
            Anim.SetBool("Walk", false);
        }
       // Run Animation
        if (Input.GetAxis("Vertical") != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            Anim.SetBool("Run", true);

        }
        else
        {
            Anim.SetBool("Run", false);
        }
    }
}
