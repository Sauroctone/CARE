using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommands : MonoBehaviour {

    public Animator anim1;
    public Animator anim2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim1.SetTrigger("hugs");
            anim2.SetTrigger("hugs");
        }
    }
}
