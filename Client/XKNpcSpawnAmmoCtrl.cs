using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XKNpcSpawnAmmoCtrl : MonoBehaviour
{
	public float[] TimeFireAmmo;//发射子弹间隔时间.
	public GameObject[] AmmoPrefabTeShu;
	public GameObject[] AmmoLZPrefabTeShu;
	GameObject[] AmmoLZObjTeShu;
	public AudioSource[] AudioTeShuNpcFire;
	public Transform[] AmmoSpawnTranTeShu;
	float[] TimeTeShuFire;
	XKNpcMoveCtrl NpcScript;

	// Use this for initialization
	void Start()
	{
		if (TimeFireAmmo != null || TimeFireAmmo.Length > 0) {
			TimeTeShuFire = new float[TimeFireAmmo.Length];
		}
		NpcScript = GetComponent<XKNpcMoveCtrl>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (!IsSpawnAmmo || TimeTeShuFire.Length < 1) {
			return;
		}

		if (CountAmmo >= MaxAmmo) {
			//停止发射子弹.
			IsSpawnAmmo = false;
			return;
		}

		GameObject obj = null;
		Transform tran = null;
		for (int i = 0; i < TimeTeShuFire.Length; i++) {
			TimeTeShuFire[i] += Time.deltaTime;
			if (TimeTeShuFire[i] >= TimeFireAmmo[i]) {
				TimeTeShuFire[i] = 0f; //fire ammo
				//Debug.Log("teShuFireNpc -> i = "+i);

				if (CountAmmo >= MaxAmmo) {
					return;
				}

				if (i < AudioTeShuNpcFire.Length && AudioTeShuNpcFire[i] != null) {
					if (AudioTeShuNpcFire[i].isPlaying) {
						AudioTeShuNpcFire[i].Stop();
					}
					AudioTeShuNpcFire[i].Play();
				}
				
				if (AmmoLZPrefabTeShu != null &&
				    AmmoLZPrefabTeShu.Length >= TimeTeShuFire.Length &&
				    AmmoLZPrefabTeShu[i] != null &&
				    AmmoLZObjTeShu[i] == null) {
					obj = (GameObject)Instantiate(AmmoLZPrefabTeShu[i],
					                              AmmoSpawnTranTeShu[i].position, AmmoSpawnTranTeShu[i].rotation);
					
					tran = obj.transform;
					AmmoLZObjTeShu[i] = obj;
					XkGameCtrl.CheckObjDestroyThisTimed(obj);
					tran.parent = AmmoSpawnTranTeShu[i];
				}

				obj = (GameObject)Instantiate(AmmoPrefabTeShu[i],
				                              AmmoSpawnTranTeShu[i].position,
				                              AmmoSpawnTranTeShu[i].rotation);
				if (obj == null) {
					return;
				}
				tran = obj.transform;
				tran.parent = XkGameCtrl.NpcAmmoArray;
				tran.position = AmmoSpawnTranTeShu[i].position;
				tran.rotation = AmmoSpawnTranTeShu[i].rotation;
				
				NpcAmmoCtrl ammoNpcScript = obj.GetComponent<NpcAmmoCtrl>();
				if (ammoNpcScript != null) {
					ammoNpcScript.SetAmmoTargetObject(null);
					ammoNpcScript.SetNpcScriptInfo(NpcScript);
					ammoNpcScript.SetIsAimFeiJiPlayer(false);
				}
				CountAmmo++;
			}
		}	
	}

	bool IsSpawnAmmo;
	int MaxAmmo;
	public int CountAmmo;
	public void StartSpawnNpcAmmo(int ammoMax)
	{
		IsSpawnAmmo = true;
		CountAmmo = 0;
		MaxAmmo = ammoMax;
	}
}