﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneRoomController : MonoBehaviour {

	public static ThroneRoomController instance;
	public Camera throneCam;

	public float dissolveDistance;

	public float DissolveDistance {
		get {
			return dissolveDistance;
		}
		set {
			dissolveDistance = value;
			Shader.SetGlobalFloat ("_DissolveDistance", value);
		}
	}

	public float dissolveSpeed;

	Animation throneAnimation;

	public GameObject particleContainer;
	public ParticleSystem[] particles;

	public AudioSource wooshAudio, flameAudio;
	// Use this for initialization
	void Awake () {
		instance = this;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.H)) {
			flameAudio.Play ();
		}
	}
	// Update is called once per frame
	void Start () {
		Shader.SetGlobalFloat ("_ExternalDistModifier", 0);
		Shader.SetGlobalFloat ("_ExternalClipModifier", 0);
		Shader.SetGlobalFloat ("_ExternalDetailEmissionModifier", -3f);

		particles = particleContainer.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem p in particles) {
			p.Stop ();
			Light l = p.GetComponent<Light> ();
			if (l != null) {
				l.enabled = false;
			}

		}
	}

	public void StartSequence () {
		StartCoroutine (Sequence ());
	}

	IEnumerator Sequence () {
		print ("Sequence Started");
		Fade.fade.StartPulseFade (0.5f);
		yield return new WaitForSeconds (2);
		Player.player.interactPrompt.SetActive (false);
		Player.player.gameObject.SetActive (false);
		throneCam.gameObject.SetActive (true);
		yield return new WaitForSeconds (2);
//		throneAnimation.Play ();
		for (float f = 0; f < 1; f += Time.deltaTime) {
			throneCam.transform.position += throneCam.transform.forward * 1 * Time.deltaTime;
			throneCam.transform.position += throneCam.transform.up * 0.5f * Time.deltaTime;
			yield return null;
		}
			
		for (float f = 0; f < 2; f += Time.deltaTime) {
			throneCam.transform.rotation *= Quaternion.Euler (0, 90 * Time.deltaTime, 0);
			yield return null;
		}

		for (float f = 0; f < 1; f += Time.deltaTime) {
			throneCam.transform.position -= throneCam.transform.forward * 1 * Time.deltaTime;
			throneCam.transform.position += throneCam.transform.up * 0.1f * Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds (1);
		wooshAudio.Play ();
		yield return new WaitForSeconds (1);
		print ("S");

		for (float f = 0; f < 65; f += Time.deltaTime * dissolveSpeed) {
			dissolveDistance = f;
			Shader.SetGlobalFloat ("_ExternalDistModifier", dissolveDistance);
			Shader.SetGlobalFloat ("_ExternalClipModifier", dissolveDistance);
			yield return null;
		}
		flameAudio.Play ();
		foreach (ParticleSystem p in particles) {
			p.Play ();
			Light l = p.GetComponent<Light> ();
			if (l != null) {
				l.enabled = true;
			}
		}

		for (float f = -3f; f < -0.5f; f += Time.deltaTime) {
			Shader.SetGlobalFloat ("_ExternalDetailEmissionModifier", f);
			yield return null;
		}
		print ("SS");
		yield return null;
	}
}