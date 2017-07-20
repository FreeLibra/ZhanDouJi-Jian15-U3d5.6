using UnityEngine;
using System.Collections;

public class UVA : MonoBehaviour {
	[Range(1, 100)]public int CountImg = 1;
	public float Speed = 0.5f;
	/**
	 * 改变UV的材质序号.
	 */
	public int ArrayIndex = 0;
	Material UVMaterial;
	public float MinOffset;
	int Count;

	// Use this for initialization.
	void Start()
	{
		MinOffset = 1f / CountImg;
		UVMaterial = GetComponent<Renderer>().materials[ArrayIndex];
		UVMaterial.SetTextureScale("_MainTex", new Vector2 (MinOffset, 1f));
	}

	// Update is called once per frame.
	void Update()
	{
		float offset = Time.time * Speed;
		if (CountImg > 1) {
			float offsetTmp = MinOffset * Count;
			if (offset < offsetTmp) {
				offset = offsetTmp;
			}
			else {
				Count++;
				offset = MinOffset * Count;
			}	
		}
		UVMaterial.SetTextureOffset("_MainTex", new Vector2 (offset, 0f));
	}
}