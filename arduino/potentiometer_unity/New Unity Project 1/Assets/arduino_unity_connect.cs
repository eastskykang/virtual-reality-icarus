using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class arduino_unity_connect: MonoBehaviour {

	SerialPort sp = new SerialPort("COM3",9600);

	// Use this for initialization
	void Start () {
		sp.Open ();
		sp.ReadTimeout = 50;
		//sp.ReadTimeout = 100;
	}

	//string tempS = "";
	//byte tempB;
	
	// Update is called once per frame
	void Update () {
		
		if (sp.IsOpen)
		{
			try
			{
				string value;

				if ((value = sp.ReadLine()) != null) {
					Debug.Log("serial out " + value);
					sp.BaseStream.Flush();
				}

				//int test = sp.ReadByte();

			}
			catch (System.Exception)
			{
				Debug.Log ("Shit!");

			}
		}
	}
}
