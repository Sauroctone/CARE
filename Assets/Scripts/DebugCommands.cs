using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommands : MonoBehaviour {

    public Animator anim1;
    public Animator anim2;
    public Transform player1;
    public Transform player2;
    public SkinnedMeshRenderer rend2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim1.SetTrigger("hugs");
            anim2.SetTrigger("hugs");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            rend2.enabled = false;
            player2.position = player1.position;
        }
    }
}
