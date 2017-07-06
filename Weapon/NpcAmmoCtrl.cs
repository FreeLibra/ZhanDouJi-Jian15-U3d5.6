using UnityEngine;
using System.Collections;

public class NpcAmmoCtrl : MonoBehaviour {
	
	public PlayerAmmoType AmmoType = PlayerAmmoType.PuTongAmmo;
	[Range(1f, 4000f)] public float MvSpeed = 50f; //km/h
	[Range(0f, 100f)] public float LifeTime = 0f;
	public GameObject AmmoExplode;
	public GameObject MetalParticle;		//金属.
	public GameObject ConcreteParticle;		//混凝土.
	public GameObject DirtParticle;			//土地.
	public GameObject WoodParticle;			//树木.
	public GameObject WaterParticle;		//水.
	public GameObject SandParticle;			//沙滩.
	public GameObject GlassParticle;		//玻璃.
	public GameObject TestSpawnNpc;
	Vector3 AmmoEndPos;
	Vector3 AmmoStartPos;
	GameObject ObjAmmo;
	Transform AmmoTran;
	XKNpcMoveCtrl NpcScript;
	bool IsAimFeiJiPlayer;
	bool IsAimPlayer = true;
	bool IsRemoveAmmo;
	float TimeScaleVal;
	bool IsOnFinishMove;
	bool IsCannotAddNpcAmmoList;
	public static LayerMask NpcAmmoHitLayer;
	bool IsDestoryNpcAmmo;
	float TimeTrail;
	TrailRenderer TrailScript;
	// Use this for initialization
	void Awake()
	{
		ObjAmmo = gameObject;
		AmmoTran = transform;
		SpawnTime = Time.time;
		if (AmmoType == PlayerAmmoType.GenZongAmmo) {
			if (LifeTime <= 0) {
				Invoke("DelayRemoveNpcAmmo", 10f);
			}
			else {
				Invoke("DelayRemoveNpcAmmo", LifeTime);
			}
			Invoke("DelayAddNpcAmmoList", 0.05f);
		}
		
		SetGenZongDanInfo();
	}

	void Update()
	{
		if (AmmoType == PlayerAmmoType.GenZongAmmo) {
			AmmoGenZongDanUpdate();
		}

		if (IsOnFinishMove) {
			return;
		}

		if (TimeScaleVal == Time.timeScale) {
			return;
		}
		TimeScaleVal = Time.timeScale;

		iTween iTweenScript = GetComponent<iTween>();
		if (iTweenScript == null) {
			return;
		}
		iTweenScript.isPaused = true;
		DestroyObject(iTweenScript);

		MvSpeed *= Time.timeScale;
		//Debug.Log("MvSpeed "+MvSpeed+", timeScale "+Time.timeScale);
		Vector3[] posArray = new Vector3[2];
		posArray[0] = AmmoTran.position;
		posArray[1] = AmmoEndPos;
		iTween.MoveTo(ObjAmmo, iTween.Hash("path", posArray,
		                                   "speed", MvSpeed,
		                                   "orienttopath", true,
		                                   "easeType", iTween.EaseType.linear,
		                                   "oncomplete", "MoveAmmoOnCompelteITween"));
	}

	public void SetNpcScriptInfo(XKNpcMoveCtrl scriptVal)
	{
		if (scriptVal == null) {
			return;
		}

		NpcScript = scriptVal;
		IsAimPlayer = NpcScript.GetIsAimPlayer();
		TestSpawnNpc = NpcScript.gameObject;
	}

	public void SetIsAimFeiJiPlayer(bool isAimFeiJi)
	{
		if (AmmoType == PlayerAmmoType.GenZongAmmo) {
			return;
		}

		if (!ObjAmmo.activeSelf) {
			ObjAmmo.SetActive(true);
			if (IsInvoking("CheckNpcAmmoState")) {
				CancelInvoke("CheckNpcAmmoState");
			}
		}

		IsAimFeiJiPlayer = isAimFeiJi;
		DelayAddNpcAmmoList();
		MoveAmmoByItween();
	}

	public void SetIsAimPlayer(bool isAim)
	{
		IsAimPlayer = isAim;
	}

	Vector3 GetFeiJiPlayerFirePos()
	{
		int randVal = Random.Range(0, 100);
		Vector3 firePos = Vector3.zero;
		if (randVal < 25) {
			firePos = XkPlayerCtrl.PlayerTranFeiJi.position;
		}
		else if (randVal < 50){
			firePos = XKPlayerCamera.FeiJiCameraTan.position;
		}
		else {
			int lengthVal = XkPlayerCtrl.GetInstanceFeiJi().NpcFirePosArray.Length;
			if (lengthVal < 1) {
				if (randVal % 2 == 0) {
					firePos = XkPlayerCtrl.PlayerTranFeiJi.position;
				}
				else {
					firePos = XKPlayerCamera.FeiJiCameraTan.position;
				}
			}
			else {
				int index = randVal % lengthVal;
				firePos = XkPlayerCtrl.GetInstanceFeiJi().NpcFirePosArray[index].position;
			}
		}
		return firePos;
	}

	Vector3 GetTanKePlayerFirePos()
	{
		int randVal = Random.Range(0, 100);
		Vector3 firePos = Vector3.zero;
		if (randVal < 25) {
			firePos = XkPlayerCtrl.PlayerTranTanKe.position;
		}
		else if (randVal < 50){
			firePos = XKPlayerCamera.TanKeCameraTan.position;
		}
		else {
			int lengthVal = XkPlayerCtrl.GetInstanceTanKe().NpcFirePosArray.Length;
			if (lengthVal < 1) {
				if (randVal % 2 == 0) {
					firePos = XkPlayerCtrl.PlayerTranTanKe.position;
				}
				else {
					firePos = XKPlayerCamera.TanKeCameraTan.position;
				}
			}
			else {
				int index = randVal % lengthVal;
				firePos = XkPlayerCtrl.GetInstanceTanKe().NpcFirePosArray[index].position;
			}
		}
		return firePos;
	}
	
	void MoveAmmoByItween()
	{
		Vector3[] posArray = new Vector3[2];
		Vector3 firePos = Vector3.zero;
		switch (XkGameCtrl.GameModeVal) {
		case GameMode.DanJiFeiJi:
			firePos = GetFeiJiPlayerFirePos();
			break;

		case GameMode.DanJiTanKe:
			firePos = GetTanKePlayerFirePos();
			break;

		case GameMode.LianJi:
			if (IsAimPlayer && (XkPlayerCtrl.PlayerTranFeiJi != null || XkPlayerCtrl.PlayerTranTanKe != null)) {
				if (IsAimFeiJiPlayer) {
					if (XkPlayerCtrl.PlayerTranFeiJi != null) {
						firePos = GetFeiJiPlayerFirePos();
					}
					else {
						firePos = GetTanKePlayerFirePos();
					}
				}
				else {
					if (XkPlayerCtrl.PlayerTranTanKe != null) {
						firePos = GetTanKePlayerFirePos();
					}
					else {
						firePos = GetFeiJiPlayerFirePos();
					}
				}
			}
			break;
		}

		if (!ScreenDanHeiCtrl.IsStartGame || XKTriggerClosePlayerUI.IsClosePlayerUI) {
			IsAimPlayer = false;
		}

		if (!IsAimPlayer) {
			float disPlayer = 0f;
			if (XkPlayerCtrl.PlayerTranFeiJi != null) {
				disPlayer = Vector3.Distance(AmmoTran.position, XkPlayerCtrl.PlayerTranFeiJi.position);
				if (XkPlayerCtrl.PlayerTranTanKe != null) {
					float disPlayerTmp = Vector3.Distance(AmmoTran.position, XkPlayerCtrl.PlayerTranTanKe.position);
					disPlayer = disPlayer >= disPlayerTmp ? disPlayer : disPlayerTmp;
				}
			}
			else {
				if (XkPlayerCtrl.PlayerTranTanKe != null) {
					disPlayer = Vector3.Distance(AmmoTran.position, XkPlayerCtrl.PlayerTranTanKe.position);
				}
			}

			if (!XKTriggerClosePlayerUI.IsClosePlayerUI) {
				firePos = AmmoTran.position + AmmoTran.forward * Random.Range(disPlayer, 1.1f * disPlayer);
			}
			else {
				firePos = AmmoTran.position + AmmoTran.forward * Random.Range(3f * disPlayer, 5f * disPlayer);
			}
		}

		RaycastHit hitInfo;
		Vector3 startPos = AmmoTran.position;
		Vector3 forwardVal = Vector3.Normalize(firePos - startPos);
		startPos += forwardVal * 3f;
		float disVal = Vector3.Distance(firePos, startPos);
		if (disVal <= 0f) {
			disVal = 500f;
		}

		Physics.Raycast(startPos, forwardVal, out hitInfo, disVal, NpcAmmoHitLayer);
		if (hitInfo.collider != null){
			firePos = hitInfo.point;
			//Debug.Log("*****npcAmmoHitObj "+hitInfo.collider.name);
		}

		TimeScaleVal = Time.timeScale;
		posArray[0] = AmmoTran.position;
		posArray[1] = firePos;
		AmmoStartPos = posArray[0];
		AmmoEndPos = firePos;
		iTween.MoveTo(ObjAmmo, iTween.Hash("path", posArray,
		                                   "speed", MvSpeed,
		                                   "orienttopath", true,
		                                   "easeType", iTween.EaseType.linear,
		                                   "oncomplete", "MoveAmmoOnCompelteITween"));
	}

	void SpawnAmmoParticleObj()
	{
		GameObject objParticle = null;
		GameObject obj = null;
		Transform tran = null;
		Vector3 hitPos = transform.position;
		
		RaycastHit hit;
		Vector3 forwardVal = Vector3.Normalize(AmmoEndPos - AmmoStartPos);
		if (AmmoType == PlayerAmmoType.PuTongAmmo) {
			float disVal = Vector3.Distance(AmmoEndPos, AmmoStartPos) + 10f;
			Physics.Raycast(AmmoStartPos, forwardVal, out hit, disVal, NpcAmmoHitLayer);
			if (hit.collider != null) {
				hitPos = hit.point;
				XKAmmoParticleCtrl ammoParticleScript = hit.collider.GetComponent<XKAmmoParticleCtrl>();
				if (ammoParticleScript != null && ammoParticleScript.PuTongAmmoLZ != null) {
					objParticle = ammoParticleScript.PuTongAmmoLZ;
				}
				else {
					string tagHitObj = hit.collider.tag;
					switch (tagHitObj) {
					case "metal":
						if (MetalParticle != null) {
							objParticle = MetalParticle;
						}
						break;
						
					case "concrete":
						if (ConcreteParticle != null) {
							objParticle = ConcreteParticle;
						}
						break;
						
					case "dirt":
						if (DirtParticle != null) {
							objParticle = DirtParticle;
						}
						break;
						
					case "wood":
						if (WoodParticle != null) {
							objParticle = WoodParticle;
						}
						break;
						
					case "water":
						if (WaterParticle != null) {
							objParticle = WaterParticle;
						}
						break;
						
					case "sand":
						if (SandParticle != null) {
							objParticle = SandParticle;
						}
						break;
						
					case "glass":
						if (GlassParticle != null) {
							objParticle = GlassParticle;
						}
						break;
					}
					
					if (objParticle == null) {
						objParticle = AmmoExplode;
					}
				}
			}
			else {
				objParticle = AmmoExplode;
			}
		}
		else {
			float disVal = Vector3.Distance(AmmoEndPos, AmmoStartPos) + 10f;
			Physics.Raycast(AmmoStartPos, forwardVal, out hit, disVal, NpcAmmoHitLayer);
			if (hit.collider != null) {
				hitPos = hit.point;
				string tagHitObj = hit.collider.tag;
				switch (tagHitObj) {
				case "dirt":
					if (DirtParticle != null) {
						objParticle = DirtParticle;
					}
					break;
				}
				
				if (objParticle == null) {
					objParticle = AmmoExplode;
				}
			}
			else {
				objParticle = AmmoExplode;
			}
		}
		
		if (objParticle == null) {
			return;
		}
		
		if (AmmoType == PlayerAmmoType.DaoDanAmmo) {
			Vector3 AmmoPos = transform.position - (transform.forward * 3f);
			Physics.Raycast(AmmoPos, forwardVal, out hit, 13f, XkGameCtrl.GetInstance().LandLayer);
			if (hit.collider != null) {
				hitPos = hit.point;
				Vector3 normalVal = hit.normal;
				Quaternion rotVal = Quaternion.LookRotation(normalVal);
				obj = (GameObject)Instantiate(objParticle, hitPos, rotVal);
				obj.transform.up = normalVal;
			}
			else {
				obj = (GameObject)Instantiate(objParticle, hitPos, transform.rotation);
			}
		}
		else {
			obj = (GameObject)Instantiate(objParticle, hitPos, transform.rotation);
		}
		
		tran = obj.transform;
		tran.parent = XkGameCtrl.NpcAmmoArray;
		
		XkAmmoTieHuaCtrl tieHuaScript = obj.GetComponent<XkAmmoTieHuaCtrl>();
		if (tieHuaScript != null && tieHuaScript.TieHuaTran != null) {
			Transform tieHuaTran = tieHuaScript.TieHuaTran;
			Vector3 AmmoPos = transform.position - (transform.forward * 3f);
			Physics.Raycast(AmmoPos, forwardVal, out hit, 13f, XkGameCtrl.GetInstance().PlayerAmmoHitLayer);
			if (hit.collider != null) {
				tieHuaTran.up = hit.normal;
			}
		}
	}

	//npc子弹打中玩家的概率值.
	[Range(0, 100)]public float AmmoHitPlayer = 50;
	void MoveAmmoOnCompelteITween()
	{
		//Debug.Log("MoveAmmoOnCompelteITween...");
		if (IsOnFinishMove) {
			return;
		}
		IsOnFinishMove = true;

		float randVal = Random.Range(0, 1000) % 100;
		if (randVal < AmmoHitPlayer) {
			XkGameCtrl.GetInstance().SubGamePlayerHealth(PlayerDamage, IndexPlayerAim);
		}
		//SpawnAmmoParticleObj();
		CancelInvoke("DelayRemoveNpcAmmo");
		RemoveAmmo();
	}

	void RemoveAmmo()
	{
		if (IsRemoveAmmo) {
			return;
		}
		IsRemoveAmmo = true;
		XkGameCtrl.RemoveNpcAmmoList(gameObject);

		if (IsDestoryNpcAmmo) {
			Destroy(ObjAmmo, 0.05f);
		}
		else {
			if (TrailScript == null) {
				TrailScript = GetComponentInChildren<TrailRenderer>();
			}
			
			if (TrailScript != null) {
				TimeTrail = TrailScript.time;
				TrailScript.time = 0f;
			}
			Invoke("DelayHiddenNpcAmmo", 0.05f);
		}
	}

	public void MakeNpcAmmoDestory()
	{
		if (IsDestoryNpcAmmo) {
			return;
		}
		IsDestoryNpcAmmo = true;

		if (!ObjAmmo.activeSelf) {
			Destroy(ObjAmmo);
		}
	}

	void DelayHiddenNpcAmmo()
	{
		IsOnFinishMove = false;
		IsRemoveAmmo = false;
		if (TrailScript != null) {
			TrailScript.time = TimeTrail;
		}
		ObjAmmo.SetActive(false);
//		if (IsInvoking("CheckNpcAmmoState")) {
//			CancelInvoke("CheckNpcAmmoState");
//		}
//		Invoke("CheckNpcAmmoState", 10f);
	}

	void CheckNpcAmmoState()
	{
		if (ObjAmmo == null) {
			return;
		}

		if (ObjAmmo.activeSelf) {
			return;
		}
		Destroy(ObjAmmo);
	}

	void DelayRemoveNpcAmmo()
	{
		iTween itweenScript = GetComponent<iTween>();
		if (itweenScript != null) {
			itweenScript.isRunning = false;
		}
		RemoveAmmo();
	}

	public void GameNeedRemoveAmmo()
	{
		if (IsRemoveAmmo) {
			return;
		}
//		Debug.Log("GameNeedRemoveAmmo...ammo "+gameObject.name);

		iTween itweenScript = GetComponent<iTween>();
		if (itweenScript != null) {
			itweenScript.isRunning = false;
		}
		CancelInvoke("DelayRemoveNpcAmmo");
		RemoveAmmo();
	}
	
	public void SetIsCannotAddNpcAmmoList()
	{
		IsCannotAddNpcAmmoList = true;
	}

	void DelayAddNpcAmmoList()
	{
		if (IsCannotAddNpcAmmoList) {
			return;
		}
		XkGameCtrl.AddNpcAmmoList(gameObject);
	}

	/**
	 * npc子弹对玩家的伤害值.
	 */
	[Range(0f, 10000f)]public int PlayerDamage = 1;
	/**
	 * npc子弹的生命值.
	 */
	[Range(0f, 1000f)]public float AmmoHealth = 0f;
	/**
	 * MuBiaoXuanDing == 0 -> 随机选择.
	 * MuBiaoXuanDing == 1 -> 当前血量最高玩家.
	 */
	[Range(0, 1)]public int MuBiaoXuanDing = 0;
	[Range(0f, 90f)]public float ShanXingJiaoDu = 45f;
	float CosJiaoDuGZ;
	public LayerMask IgnoreLayers;
	const float HitRadius = 1.0f;
	//延迟跟踪转向时间.
	float SeekPrecision = 1.3f;
	Vector3 DirAmmo;
	float SpawnTime;
	GameObject TargetObject;
//	public GameObject TestTarget;
	float forceAmount = 10f;
	PlayerEnum IndexPlayerAim;

	void SetGenZongDanInfo()
	{
		if (AmmoType != PlayerAmmoType.GenZongAmmo) {
			return;
		}

		CosJiaoDuGZ = Mathf.Cos(ShanXingJiaoDu);
		if (XkGameCtrl.GetInstance() != null) {
			switch (MuBiaoXuanDing) {
			case 1:
				IndexPlayerAim = XkGameCtrl.GetInstance().GetMaxHealthPlayer();
				break;
			default:
				IndexPlayerAim = XkGameCtrl.GetInstance().GetRandAimPlayerObj();
				break;
			}
		}
		TargetObject = XKPlayerCamera.GetInstanceFeiJi().gameObject;
		//TargetObject = TestTarget; //test
	}

	void AmmoGenZongDanUpdate()
	{
		if (Time.time > SpawnTime + LifeTime) {
			DestroyNpcAmmo(ObjAmmo);
			return;
		}

		bool isGenZong = false;
		if (TargetObject) {
			Vector3 targetPos = TargetObject.transform.position;
			float cosVal = 0f;
			Vector3 vecA = AmmoTran.forward;
			Vector3 vecB = targetPos - AmmoTran.position;
			vecB.y = vecA.y = 0f;
			cosVal = Vector3.Dot(vecA, vecB.normalized);
			if (cosVal > CosJiaoDuGZ) {
				isGenZong = true;
				//targetPos += transform.right * (Mathf.PingPong (Time.time, 1.0f) - 0.5f) * noise;
				Vector3 targetDirAmmo = (targetPos - AmmoTran.position);
				float targetDist = targetDirAmmo.magnitude;
				targetDirAmmo /= targetDist;
				
				DirAmmo = Vector3.Slerp (DirAmmo, targetDirAmmo, Time.deltaTime * SeekPrecision);
				AmmoTran.rotation = Quaternion.LookRotation(DirAmmo);
				AmmoTran.position += (DirAmmo * MvSpeed) * Time.deltaTime;
			}
		}

		if (!isGenZong) {
			AmmoTran.position += (AmmoTran.forward * MvSpeed) * Time.deltaTime;
		}
		
		// Check if this one hits something
		Collider[] hits = Physics.OverlapSphere(AmmoTran.position, HitRadius, ~IgnoreLayers.value);
		bool collided = false;
		foreach (Collider c in hits) {
			// Don't collide with triggers
			if (c.isTrigger) {
				continue;
			}
			
			XKPlayerCamera playerScript = c.GetComponent<XKPlayerCamera>();
			if (playerScript != null/* && !playerScript.GetIsWuDiState()*/) {
				XkGameCtrl.GetInstance().SubGamePlayerHealth(PlayerDamage, IndexPlayerAim);
			}
//			if (playerScript != null && !playerScript.GetIsWuDiState()) {
//				XkGameCtrl.GetInstance().SubGamePlayerHealth(playerScript.PlayerIndex, PlayerDamage);
//			}

			// Get the rigidbody if any
			if (c.GetComponent<Rigidbody>()) {
				// Apply force to the target object
				Vector3 force = AmmoTran.forward * forceAmount;
				force.y = 0f;
				c.GetComponent<Rigidbody>().AddForce (force, ForceMode.Impulse);
			}
			collided = true;
		}
		
		if (collided) {
			if (AmmoExplode != null) {
				GameObject expObj = (GameObject)Instantiate(AmmoExplode, AmmoTran.position, AmmoTran.rotation);
				if (XkGameCtrl.NpcAmmoArray != null) {
					expObj.transform.parent = XkGameCtrl.NpcAmmoArray;
				}
			}
			DestroyNpcAmmo(ObjAmmo);
		}
	}
	
	void DestroyNpcAmmo(GameObject ammoObj)
	{
		if (ammoObj != null) {
			Destroy(ammoObj);
		}
	}

	public void SetAmmoTargetObject(GameObject obj)
	{
		TargetObject = obj;
	}
}