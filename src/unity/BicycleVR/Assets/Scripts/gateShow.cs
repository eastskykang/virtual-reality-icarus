using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateShow : MonoBehaviour {

	private string objname = "goal";
	private float alpha = 0.5f;

	// current goal
	private int currentGoal = 1;

	// time left
	private float timeLeft = 30.0f;

	// Use this for initialization
	void Start () {

		// set up colors
		for (int i = 1; i <= 13; i++) {
			GameObject goal_i = GameObject.Find (objname + i);

			Color rcolor = goal_i.GetComponent<Renderer> ().material.color;
			rcolor.a = 0f;

			goal_i.GetComponent<Renderer> ().material.color = rcolor;
		}

		GameObject goal1 = GameObject.Find (objname + "1");
		Color rcolor1 = goal1.GetComponent<Renderer> ().material.color;
		rcolor1.a = alpha;

		goal1.GetComponent<Renderer> ().material.color = rcolor1;
	}
	
	// Update is called once per frame
	void Update () {

		// time update
		timeLeft -= Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.name.Contains (objname)) {

			var token = new string[] { objname };
			string goalTouchedNumber = other.name.Split (token, System.StringSplitOptions.RemoveEmptyEntries)[0];

			if (int.Parse (goalTouchedNumber) == currentGoal) {
				// touched goal!

				GameObject prevGoal = GameObject.Find (objname + currentGoal);

				Color prevrcolor = prevGoal.GetComponent<Renderer> ().material.color;
				prevrcolor.a = 0f;
				prevGoal.GetComponent<Renderer> ().material.color = prevrcolor;

				// next goal (new goal)
				currentGoal += 1;

				GameObject newGoal = GameObject.Find (objname + currentGoal);

				Color newrcolor = newGoal.GetComponent<Renderer> ().material.color;
				newrcolor.a = alpha;
				newGoal.GetComponent<Renderer> ().material.color = newrcolor;

				// timer update
				timeLeft = 30.0f;
			}
		}
	}
}
