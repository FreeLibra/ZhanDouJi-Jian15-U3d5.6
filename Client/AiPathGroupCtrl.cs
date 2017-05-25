using UnityEngine;
using System.Collections;

public class AiPathGroupCtrl : MonoBehaviour {
	public PlayerTypeEnum PathState = PlayerTypeEnum.FeiJi;
    void Start()
    {
        switch (XkGameCtrl.GameJiTaiSt) {
            case GameJiTaiType.FeiJiJiTai:
                if (PathState == PlayerTypeEnum.TanKe){
                    gameObject.SetActive(false);
                }
                break;
            case GameJiTaiType.TanKeJiTai:
                if (PathState == PlayerTypeEnum.FeiJi) {
                    gameObject.SetActive(false);
                }
                break;
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

		AiPathCtrl[] PathArray = transform.GetComponentsInChildren<AiPathCtrl>();
		for (int i = 0; i < PathArray.Length; i++) {
			PathArray[i].name = PathState + "AiPath_" + (i+1);
		}
	}
}