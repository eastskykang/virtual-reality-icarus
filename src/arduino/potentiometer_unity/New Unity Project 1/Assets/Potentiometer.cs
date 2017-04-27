using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Potentiometer: MonoBehaviour {

	private SerialPort	sp;

	// Use this for initialization
	void Start () {

		switch (SystemInfo.operatingSystemFamily) 
		{
		case OperatingSystemFamily.Windows:
			sp = new SerialPort ("COM3", 9600);
			break;
		case OperatingSystemFamily.MacOSX:
			sp = new SerialPort ("/deb/tty.usbmodem411", 9600);
			break;
		default:
			throw new System.Exception();
		}

		sp.Open ();
		sp.ReadTimeout = 50;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (sp.IsOpen)
		{
			try
			{
				string value;

				if ((value = sp.ReadLine()) != null) {
					Debug.Log("Serial Out : " + value);
					sp.BaseStream.Flush();

					// angle of values
					// 0    - 511   : left    -90 to  0
					// 512  - 1023  : right   0   to  +90
					double angleInput = int.Parse(value) / 1024.0 * 180.0 - 90.0;
					transform.Rotate(0, (float) angleInput * Time.deltaTime, 0);
				}
			}
			catch (System.Exception)
			{
				Debug.Log ("System Exception");
			}
		}
	}
}
