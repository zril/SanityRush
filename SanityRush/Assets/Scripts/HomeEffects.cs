using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class HomeEffects : MonoBehaviour {

    private PostProcessVolume active;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;

    public Text titleText;
    public Color titleColor1;
    public Color titleColor2;

    // Use this for initialization
    void Start () {
        var cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (cam != null)
        {
            active = cam.GetComponent<PostProcessVolume>();
            if (active != null)
            {
                active.profile.TryGetSettings(out chromaticAberration);
                if (chromaticAberration != null) { chromaticAberration.enabled.Override(true); }
                active.profile.TryGetSettings(out lensDistortion);
                if (lensDistortion != null) { lensDistortion.enabled.Override(true); }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        float ChromaticValue = 0.5f + 0.4f * Mathf.Cos(0.6f * Mathf.PI * Time.fixedTime) + 0.02f * Mathf.Cos(20f * Mathf.PI * Time.fixedTime + 0.1f);
        if (chromaticAberration != null)
            chromaticAberration.intensity.Override(ChromaticValue);

        float LensValue = -35f + 25f * Mathf.Cos(0.2f * Mathf.PI * Time.fixedTime);
        if (lensDistortion != null)
            lensDistortion.intensity.Override(LensValue);

        titleText.color = Color.Lerp(titleColor1, titleColor2, 0.5f + 0.5f * Mathf.Cos(1f * Mathf.PI * Time.fixedTime));
    }
}
