using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFunctions : MonoBehaviour {

	public static void PlaySound (AudioSource sourceSound, bool randomPitch, float minPitch, float maxPitch, bool checkLoop) {

		if (randomPitch) {
			sourceSound.pitch = Random.Range (minPitch, maxPitch);
		}

		if (checkLoop) {
			if (!sourceSound.isPlaying) {
				sourceSound.Play ();
			}
		} else {
			sourceSound.Play ();
		}

	}

}
