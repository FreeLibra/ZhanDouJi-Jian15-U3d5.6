using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuJiBaoMoveCtrl : MonoBehaviour
{
	[Range(0f, 999f)]public float PushVal = 50f;
	Rigidbody BuJiBaoRig;
	// Use this for initialization
	void Start()
	{
		BuJiBaoRig = GetComponent<Rigidbody>();
		//StartMoveBuJiBao((byte)((Random.Range(0, 100) % 8) + 1)); //test.
	}

	public void StartMoveBuJiBao(byte dirVal = 0)
	{
		if (dirVal == 0) {
			dirVal = (byte)((Random.Range(0, 100) % 8) + 1);
		}
		Vector3 dirVec = GetMoveDirection(dirVal);
		BuJiBaoRig.AddForce(dirVec * PushVal);
	}

	/**
	 * dirVal == 1 -> 前.
	 * dirVal == 2 -> 右.
	 * dirVal == 3 -> 后.
	 * dirVal == 4 -> 左.
	 * dirVal == 5 -> 前右.
	 * dirVal == 6 -> 后右.
	 * dirVal == 7 -> 后左.
	 * dirVal == 8 -> 前左.
	 */
	Vector3 GetMoveDirection(byte dirVal)
	{
		Vector3 dirVec = Vector3.zero;
		switch (dirVal) {
			case 1:
				dirVec = transform.forward;
				break;
			case 2:
				dirVec = transform.right;
				break;
			case 3:
				dirVec = -transform.forward;
				break;
			case 4:
				dirVec = -transform.right;
				break;
			case 5:
				dirVec = Vector3.Lerp(transform.forward, transform.right, 0.5f);
				break;
			case 6:
				dirVec = Vector3.Lerp(-transform.forward, transform.right, 0.5f);
				break;
			case 7:
				dirVec = Vector3.Lerp(-transform.forward, -transform.right, 0.5f);
				break;
			case 8:
				dirVec = Vector3.Lerp(transform.forward, -transform.right, 0.5f);
				break;
			default:
				dirVec = transform.forward;
				break;
		}
		dirVec.y = 0f;
		return dirVec;
	}
}