using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiShiQuanCtrl : MonoBehaviour
{
	public PlayerEnum IndexPlayer;
	public Transform FollowTr;
	Camera CameraMain;
	byte IndexVal;
	// Use this for initialization
	void Start()
	{
		IndexVal = (byte)((byte)IndexPlayer - 1);
		if (IndexVal >= 0 && IndexVal <= 1) {
			XKPlayerAutoFire playerAutoFire = XkPlayerCtrl.GetInstanceFeiJi().GetPlayerAutoFireScript();
			CameraMain = playerAutoFire.GunCamera[IndexVal];
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if (FollowTr == null || CameraMain == null) {
			return;
		}
		UpdateTiShiQuanPos();
	}

	void UpdateTiShiQuanPos()
	{
		Vector3 pos = CameraMain.WorldToScreenPoint(FollowTr.position);
		pos.z = 0f;
		pos.x = (XkGameCtrl.ScreenWidth * pos.x) / (0.5f * Screen.width);
		pos.x -= 1360 * IndexVal;
		pos.y = (XkGameCtrl.ScreenHeight * pos.y) / Screen.height;
		transform.localPosition = pos;
	}
}