using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TooltipTextManager : MonoBehaviour {
    public Text tooltipText;
    public float timeFadeIn = 0.5f;
    public float timeFadeOut = 0.5f;
    public float changeTimeSeconds = 5;
    public float endAlpha = 1;

    public IEnumerator FadeInOut(string newText) {
        FadeIn(newText);
        yield return new WaitForSeconds(changeTimeSeconds);
        FadeOut();
    }

    public void FadeIn(string newText) {
        tooltipText.text = newText;
        endAlpha = 1;
        tooltipText.CrossFadeAlpha(endAlpha, timeFadeIn, false);
    }

    public void FadeOut() {
        endAlpha = 0;
        tooltipText.CrossFadeAlpha(endAlpha, timeFadeOut, false);
    }
}
