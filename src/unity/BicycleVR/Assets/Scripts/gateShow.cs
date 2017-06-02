using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateShow : MonoBehaviour {

	private string objname = "goal";
	private float alpha = 0.9f;

	// current goal
	private int currentGoal = 1;

	// time left
	private float timeLeft = 30.0f;

	// Use this for initialization
	void Start () {

		// set up colors
		for (int i = 1; i <= 13; i++) {
			GameObject goal_i = GameObject.Find (objname + i);
			Behaviour h = (Behaviour)goal_i.GetComponent ("Halo");
			h.enabled = false;

			Color rcolor = goal_i.GetComponent<Renderer> ().material.color;
			rcolor.a = alpha;

			goal_i.GetComponent<Renderer> ().material.color = rcolor;
			goal_i.GetComponent<Renderer> ().enabled = false;
		}

		GameObject goal1 = GameObject.Find (objname + "1");
		goal1.GetComponent<Renderer> ().enabled = true;
		Behaviour h1 = (Behaviour)goal1.GetComponent ("Halo");
		h1.enabled = true;
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
			
				prevGoal.GetComponent<Renderer> ().enabled = false;
				Behaviour prevH = (Behaviour)prevGoal.GetComponent ("Halo");
				prevH.enabled = false;

				// next goal (new goal)
				currentGoal += 1;

				if (currentGoal > 13)
					currentGoal = 1;

				GameObject newGoal = GameObject.Find (objname + currentGoal);

				newGoal.GetComponent<Renderer> ().enabled = true;
				Behaviour newH = (Behaviour)newGoal.GetComponent ("Halo");
				newH.enabled = true;

				// timer update
				timeLeft = 30.0f;
			}
		}
	}
}
