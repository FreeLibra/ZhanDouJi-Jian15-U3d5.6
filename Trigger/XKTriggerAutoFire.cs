using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XKTriggerAutoFire : MonoBehaviour
{
	public XKSpawnNpcPoint SpawnPoint;
	/**
	 * 队友npc发射普通子弹的持续时间.
	 */
	[Range(0, 1000)] public float TimeFirePT = 3;
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

		if (TimeFirePT > 0) {
			Invoke("CloseDuiYouFirePTAmmo", TimeFirePT);
		}
	}

	//关闭队友npc
	void CloseDuiYouFirePTAmmo()
	{
		//Debug.Log("CloseDuiYouFirePTAmmo...");
		GameObject npcObj = SpawnPoint.GetNpcLoopObj();
		if (npcObj == null) {
			return;
		}
		
		XKNpcMoveCtrl npcScript = npcObj.GetComponent<XKNpcMoveCtrl>();
		if (npcScript != null) {
			npcScript.SetFireDistance(-1);
		}
	}
}