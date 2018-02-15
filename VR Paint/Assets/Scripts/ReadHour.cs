using UnityEngine;
using System.Collections;

public class ReadHour : MonoBehaviour {
	uSkyManager skyboxManager;
    public SteamVR_TrackedObject trackedObj;
    public int hour = 0;

    void Start () {
        hour = System.DateTime.Now.Hour;
        skyboxManager = GameObject.Find("skybox").GetComponent<uSkyManager>();
		skyboxManager.Timeline = hour;
	}

    void Update() {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
            if (hour == 23) hour = 0;
            hour++;
            skyboxManager.Timeline = hour;
        }
    }
}
