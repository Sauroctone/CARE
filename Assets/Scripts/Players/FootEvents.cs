using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootEvents : MonoBehaviour {

    public ParticleSystem snow;
    public ParticleSystem leftFootprint;
    public ParticleSystem rightFootprint;


    void OnLeftFootEnter()
    {
        snow.Play();
    }

    void OnLeftFootExit()
    {
        var main = leftFootprint.main;
        main.startRotation = transform.parent.rotation.eulerAngles.y * Mathf.Deg2Rad;
        leftFootprint.Play();
    }

    void OnRightFootEnter()
    {
        snow.Play();
    }

    void OnRightFootExit()
    {
        var main = rightFootprint.main;
        main.startRotation = transform.parent.rotation.eulerAngles.y * Mathf.Deg2Rad;
        rightFootprint.Play();
    }
}
