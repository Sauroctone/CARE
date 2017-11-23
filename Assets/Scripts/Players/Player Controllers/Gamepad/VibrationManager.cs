using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


public class VibrationManager : MonoBehaviour {

	bool isVibrating;

	public void VibrateUntil(float leftPower, float rightPower, float timer)
	{
		if (!isVibrating)
			StartCoroutine ("VibrationTimer", timer);
		
		GamePad.SetVibration (PlayerIndex.One, leftPower, rightPower);
	}

	public void Vibrate(float leftPower, float rightPower)
	{
		GamePad.SetVibration (PlayerIndex.One, leftPower, rightPower);
	}

	IEnumerator VibrationTimer(float timer)
	{
		isVibrating = true;
		yield return new WaitForSeconds (timer);
		isVibrating = false;
	}
}
