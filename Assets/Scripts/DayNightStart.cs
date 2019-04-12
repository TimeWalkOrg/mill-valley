using UnityEngine;
using System.Collections;
public class DayNightStart : MonoBehaviour
{
    public float minutesInDay = 1.0f;
    public float timer;
    public float percentageOfDay;
    public float turnSpeed;

    // Use this for initialization
    void Start()
    {
        timer = 0.0f; //Reset Timer
    }

    // Update is called once per frame
    void Update()
    {
        checkTime();
        UpdateLights();
        turnSpeed = 360.0f / (minutesInDay * 60.0f) * Time.deltaTime;
        transform.RotateAround(transform.position, transform.right, turnSpeed);
        Debug.Log(percentageOfDay);
    }
    void UpdateLights()
    {
        Light l = GetComponent<Light>();
        if (isNight())
        {
            if (l.intensity > 0.0f)
            {
                l.intensity -= 0.05f;
            }
        }
        else
        {
            if (l.intensity < 1.0f)
            {
                l.intensity += 0.05f;
            }
        }
    }
    bool isNight()
    {
        bool c = false;
        if (percentageOfDay > 0.5f)
        {
            c = true;
        }
        return c;
    }
    void checkTime()
    {
        timer += Time.deltaTime;
        percentageOfDay = timer / (minutesInDay * 60.0f);
        if (timer > (minutesInDay * 60.0f))
        {
            timer = 0.0f;
        }
    }
}
