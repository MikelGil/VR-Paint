using UnityEngine;
using System.Collections;

public class MaterialChanger : MonoBehaviour {

	DrawLineManager drawLine;
	public Material[] materialList;
	int iterator = 0;
	
	void Start () {
		drawLine = GameObject.Find("DrawLiner").GetComponent<DrawLineManager>();
		if (materialList[0] != null) {
			drawLine.material = materialList[iterator];
		}
	}
	
	public void iterateMaterial() {
		iterator++;
		iterator = iterator % materialList.Length;
		drawLine.material = materialList[iterator];
	}

}
