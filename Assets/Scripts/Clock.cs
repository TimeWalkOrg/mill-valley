using UnityEngine;

public class Clock : MonoBehaviour {
	
    // The game object which represents the hour hand of the clock.
	public Transform hourHand;
    // The game object which represents the minute hand of the clock.
	public Transform minuteHand;

    // The number of degrees per hour on our clock face.
	float hoursToDegrees = 360f / 12f;
    // The number of degrees per minute on our clock face.
	float minutesToDegrees = 360f / 60f;

    // A reference to the DayNightController script.
	DayNightController controller;

	void Awake() {
        // Find the DayNightController game object by its name and get the DayNightController script on it.
		controller = GameObject.Find("DayNightController").GetComponent<DayNightController>();
	}

	void Update() {
        // Calculate the current hour and minute according to the currentTimeOfDay
        // variable in the DayNightController.
        // The extra calculation for the current minute is to make sure it stays
        // between 0 and 60 and not keeps increasing as the hours increase.
        float currentHour = 24 * controller.currentTimeOfDay;
        float currentMinute = 60 * (currentHour - Mathf.Floor(currentHour));

        // Rotate the hands of the clock face according to the values we've defined.
        // If the hands rotate on the wrong axis change the axis here.
        hourHand.localRotation = Quaternion.Euler(currentHour * hoursToDegrees, 0, 0);
		minuteHand.localRotation = Quaternion.Euler(currentMinute * minutesToDegrees, 0, 0);
	}
}
