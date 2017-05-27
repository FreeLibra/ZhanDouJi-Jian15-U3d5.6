using UnityEngine;
using System.Collections;

public class XKTriggerSpawnNpc : MonoBehaviour {

	public XKSpawnNpcPoint[] SpawnPointArray;
	public AiPathCtrl TestPlayerPath;
	static bool IsDonnotSpawnNpcTest = false;
	void Start()
	{
		MeshRenderer meshCom = GetComponent<MeshRenderer>();
		if (meshCom != null) {
			meshCom.enabled = false;
		}

		for (int i = 0; i < SpawnPointArray.Length; i++) {
			if (SpawnPointArray[i] == null) {
				Debug.LogWarning("SpawnPointArray was wrong! index "+(i+1));
				GameObject obj = null;
				obj.name = "null";
				break;
			}
			SpawnPointArray[i].SetIsSpawnTrigger();
		}
		//Invoke("DelayChangeBoxColliderSize", 0.2f);
	}

	void DelayChangeBoxColliderSize()
	{
		XkGameCtrl.GetInstance().ChangeBoxColliderSize(transform);
	}

	void OnTriggerEnter(Collider other)
	{
		if (IsDonnotSpawnNpcTest) {
			return; //test;
		}

		if (JiFenJieMianCtrl.GetInstance() != null && JiFenJieMianCtrl.GetInstance().GetIsShowFinishTask()) {
			return;
		}

		if (!XkGameCtrl.IsMoveOnPlayerDeath) {
			if (!XkGameCtrl.IsActivePlayerOne && !XkGameCtrl.IsActivePlayerTwo) {
				return;
			}
		}
		
		if (Network.peerType == NetworkPeerType.Client) {
			return;
		}

		XkPlayerCtrl ScriptPlayer = XkGameCtrl.GetPlayerScript(other.gameObject);
		if (ScriptPlayer == null) {
			return;
		}

		//test
//		if (ScriptPlayer.PlayerSt == PlayerTypeEnum.FeiJi) {
//			IsDonnotSpawnNpcTest = true;
//		}
//		else {
//			return;
//		}

		//Debug.Log("XKTriggerSpawnNpc::OnTriggerEnter -> hit "+other.name);
		for (int i = 0; i < SpawnPointArray.Length; i++) {
			SpawnPointArray[i].SpawnPointAllNpc();
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

		if (SpawnPointArray != null || SpawnPointArray.Length > 1) {
			for (int i = 0; i < SpawnPointArray.Length; i++) {
				if (SpawnPointArray[i] == null) {
					Debug.LogWarning("SpawnPointArray was wrong! index "+(i+1));
					GameObject obj = null;
					obj.name = "null";
					break;
				}
				SpawnPointArray[i].AddTestTriggerSpawnNpc(this);
			}
		}

		if (TestPlayerPath != null) {
			TestPlayerPath.DrawPath();
		}
	}

	public void RemoveCartoonSpawnNpc()
	{
		int max = SpawnPointArray.Length;
		for (int i = 0; i < max; i++) {
			SpawnPointArray[i].RemovePointAllNpc();
		}
	}
}