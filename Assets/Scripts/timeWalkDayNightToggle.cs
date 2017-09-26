using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeWalkDayNightToggle : MonoBehaviour {
    public bool nowIsDay;
	public Material dayMaterial;
    public Material nightMaterial;
	public Color colorMoon;
	public Color colorSun;
	public Color colorSkyColorDay;
	public Color colorSkyColorNight;
	public float dayIntensity = 0.4f;
    public float nightIntensity = 0.1f;
    public Light sunLightObject;
    public GameObject nightLights;
    public GameObject forceNightTimeObject;
    private bool lastNowIsDay; // remembers last time Day/Night was set deliberately

    // Use this for initialization
    void Start () {
        sunLightObject = gameObject.GetComponent<Light>();
        lastNowIsDay = nowIsDay;
	}
	
	// Update is called once per frame
	void Update () {
        if (forceNightTimeObject.activeSelf)
        {
            nowIsDay = false;
        }
        else { nowIsDay = lastNowIsDay; }
        if (Input.GetKeyDown(KeyCode.N))
        {
            nowIsDay = !nowIsDay;
            lastNowIsDay = nowIsDay;
        }

        if (nowIsDay == false) // NIGHT settings
        {
            RenderSettings.skybox = nightMaterial;
            RenderSettings.ambientIntensity = nightIntensity;
            RenderSettings.ambientLight = colorSkyColorNight;
            sunLightObject.color = colorMoon;
            nightLights.SetActive(true);
            RenderSettings.fog = false;
        }
        else // DAY settings
        {
            RenderSettings.skybox = dayMaterial;
            RenderSettings.ambientLight = colorSkyColorDay;
            sunLightObject.color = colorSun;
            RenderSettings.ambientIntensity = dayIntensity;
            nightLights.SetActive(false);
            RenderSettings.fog = true;
        }
    }
}
