using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XKTriggerAutoFire : MonoBehaviour
{
	public XKSpawnNpcPoint SpawnPoint;
	public AiPathCtrl TestPlayerPath;
	// Use this for initialization
	void Start()
	{
		MeshRenderer meshCom = GetComponent<MeshRenderer>();
		if (meshCom != null) {
			meshCom.enabled = false;
		}

		if (SpawnPoint == null) {
			gameObject.SetActive(false);
		}
	}
	
	void OnDrawGizmosSelected()
	{
		if (!XkGameCtrl.IsDrawGizmosObj) {
			return;
		}
		
		if (!enabled) {
			return;
		}
		
//		if (SpawnPointArray != null || SpawnPointArray.Length > 1) {
//			for (int i = 0; i < SpawnPointArray.Length; i++) {
//				if (SpawnPointArray[i] == null) {
//					Debug.LogWarning("SpawnPointArray was wrong! index "+(i+1));
//					GameObject obj = null;
//					obj.name = "null";
//					break;
//				}
//				SpawnPointArray[i].AddTestTriggerSpawnNpc(this);
//			}
//		}
		
		if (TestPlayerPath != null) {
			TestPlayerPath.DrawPath();
		}
	}

	void OnTriggerEnter(Collider other)
	{	
		if (JiFenJieMianCtrl.GetInstance() != null && JiFenJieMianCtrl.GetInstance().GetIsShowFinishTask()) {
			return;
		}
		
		if (!XkGameCtrl.IsMoveOnPlayerDeath) {
			if (!XkGameCtrl.IsActivePlayerOne && !XkGameCtrl.IsActivePlayerTwo) {
				return;
			}
		}

		XkPlayerCtrl ScriptPlayer = XkGameCtrl.GetPlayerScript(other.gameObject);
		if (ScriptPlayer == null) {
			return;
		}
		CheckAutoFireCom();
	}

	void CheckAutoFireCom()
	{
		GameObject npcObj = SpawnPoint.GetNpcLoopObj();
		if (npcObj == null) {
			return;
		}

		XKNpcMoveCtrl npcScript = npcObj.GetComponent<XKNpcMoveCtrl>();
		if (npcScript != null) {
			npcScript.SetFireDistance(0);
		}
	}
}