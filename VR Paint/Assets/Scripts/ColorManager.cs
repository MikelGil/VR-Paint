using UnityEngine;
using System.Collections;

public class ColorManager : MonoBehaviour {

	public static ColorManager Instance;

    private Color color;

    void Awake(){
		if (Instance == null)
			Instance = this;
	}

	void OnDestroy(){
		if (Instance == this)
			Instance = null;
	}

	void OnColorChange(HSBColor color){
		this.color = color.ToColor();
	}

	public Color GetCurrentColor(){
		return this.color;
	}
}
