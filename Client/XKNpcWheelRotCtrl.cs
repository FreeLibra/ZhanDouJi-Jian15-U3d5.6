using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XKNpcWheelRotCtrl : MonoBehaviour
{
	public TweenRotation[] WheelTweenRot;
	public UVA[] WheelUV;
	// Use this for initialization
	void Start()
	{
		StopWheelRot();
	}

	public void StopWheelRot()
	{
		if (WheelTweenRot != null) {
			for (int i = 0; i < WheelTweenRot.Length; i++) {
				WheelTweenRot[i].enabled = false;
			}
		}
		
		if (WheelUV != null) {
			for (int i = 0; i < WheelUV.Length; i++) {
				WheelUV[i].enabled = false;
			}
		}
	}

	public void PlayWheelRot()
	{
		if (WheelTweenRot != null) {
			for (int i = 0; i < WheelTweenRot.Length; i++) {
				WheelTweenRot[i].enabled = true;
			}
		}
		
		if (WheelUV != null) {
			for (int i = 0; i < WheelUV.Length; i++) {
				WheelUV[i].enabled = true;
			}
		}
	}
}
