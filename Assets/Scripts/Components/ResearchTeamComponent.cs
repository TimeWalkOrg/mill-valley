using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchTeamComponent : MonoBehaviour
{
	private Text researchTeamText;

	private void Awake()
	{
		researchTeamText = GetComponentInChildren<Text>();
	}

	public void SetResearchTeamData(ResearchTeamData data)
	{
		string tempBuildingS = "<b>" + data.buildingName + "</b>" + "\n";
		string tempStreetS = data.street + "\n\n";
		string tempHeaderS = "Research Team: \n";
		string tempTeamS = "";
		for (int i = 0; i < data.team.Length; i++)
		{
			tempTeamS += "- " + data.team[i] + "\n";
		}
		string tempDateS = "\n" + "<color=grey>" + data.date + "</color>";

		researchTeamText.text = tempBuildingS + tempStreetS + tempHeaderS + tempTeamS + tempDateS;
		tempBuildingS = null;
		tempStreetS = null;
		tempHeaderS = null;
		tempTeamS = null;
		tempDateS = null;
	}
}
