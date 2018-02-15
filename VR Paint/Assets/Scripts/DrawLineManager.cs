using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DrawLineManager : MonoBehaviour {

	// Our controller
    public SteamVR_TrackedObject trackedObj;
    public GameObject colorSelector; 

	// Our custom material
    public Material material;

    public float lineWidth = 0.01f;
    private float minLineWidth = 0.01f;
    private float maxLineWidth = 0.16f;

    // Our custom mesh renderer
    private MeshLineRenderer currentLine;
    private int numClicks = 0;

    private Color colorLine;

    private TooltipTextManager tooltipTextManager;

    public List<GameObject> drawedList;
    public int drawedObject = 0;

    void Start() {
        drawedList = new List<GameObject>();
    }

    void Update() {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
			// We create a new empty gameobject and we add meshfilter and meshrenderer
            GameObject go = new GameObject("line" + drawedObject);
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            //go.layer = LayerMask.NameToLayer("Ignore Light");

            drawedList.Add(go);
            drawedObject++;
        
            currentLine = go.AddComponent<MeshLineRenderer>();

			currentLine.material = new Material(material);
            currentLine.setWidth(lineWidth);

            //numClicks = 0;
        } else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) {
            currentLine.AddPoint(trackedObj.transform.position);
            numClicks++;
		} else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)){
			numClicks = 0;
			currentLine = null;
		}

		if (currentLine != null) {
            if (colorSelector.activeSelf) {
                colorLine = colorSelector.GetComponent<ColorSelector>().finalColor;
            }
			currentLine.material.color = colorLine;
		}

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip)) {
            DeletelastLine();
        }
    }

    public void IncreaseLineWidth() {
        if (lineWidth < maxLineWidth) {
            lineWidth *= 2;
            tooltipTextManager.FadeInOut("Tamaño x2");
        }
    }

    public void DecreaseLineWidth() {
        if (lineWidth > minLineWidth) {
            lineWidth /= 2;
            tooltipTextManager.FadeInOut("Tamaño /2");
        }
    }

    public void DeletelastLine() {
        Destroy(drawedList[drawedList.Count-1]);
        drawedList.RemoveAt(drawedList.Count - 1);
        drawedObject--;
    }
}


