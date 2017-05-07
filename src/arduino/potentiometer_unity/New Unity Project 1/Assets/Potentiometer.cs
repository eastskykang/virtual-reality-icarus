using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

enum SensorDataType {ANGLE, VELOCITY, SOUND};

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
			SensorData data1;
			SensorData data2;
			SensorData data3;

			try
			{
				string packet1;
				string packet2;
				string packet3;

				if ((packet1 = sp.ReadLine()) != null) {
					Debug.Log("Serial Out : " + value1);
					sp.BaseStream.Flush();

					data1 = ParsePacket(packet1);
				}

				if ((packet2 = sp.ReadLine()) != null) {
					Debug.Log("Serial Out : " + packet2);
					sp.BaseStream.Flush();

					data2 = ParsePacket(packet2);
				}

				if ((packet3 = sp.ReadLine()) != null) {
					Debug.Log("Serial Out : " + packet3);
					sp.BaseStream.Flush();

					data3 = ParsePacket(packet3);
				}
			}
			catch (System.Exception)
			{
				Debug.Log ("System Exception");
			}

			// angle of values
			// 0    - 511   : left    -90 to  0
			// 512  - 1023  : right   0   to  +90
			//					double angleInput = double.Parse(packet1) / 1024.0 * 180.0 - 90.0;
			//					transform.Rotate(0, (float) angleInput * Time.deltaTime, 0);
			double angle = GetAngle (data1, data2, data3);

			// frequency of wheel rotation 
			// unit: Hz
			double velocity = GetVelocity (data1, data2, data3);

			// signal value from sound sensor
			double sound = GetSound (data1, data2, data3);
		}
	}

	SensorData ParsePacket (string str) {
		// Packets
		string anglePrefix = "AN";
		string velocityPrefix = "VE";
		string soundPrefix = "DB";

		string[] strs = str.Split("::");

		if (strs.Length != 2)
			// exception
			Debug.Log ("something wrong");

		if (string.Compare(strs[0], anglePrefix)) {
			return new SensorData (SensorDataType.ANGLE, strs [1]);
		} 
		else if (string.Compare(strs[0], velocityPrefix)) {
			return new SensorData (SensorDataType.VELOCITY, strs [1]);
		}
		else if (string.Compare(strs[0], soundPrefix)) {
			return new SensorData (SensorDataType.SOUND, strs [1]);;
		}
		else {
			// exception 
		}
	}

	double GetAngle(SensorData data1, SensorData data2, SensorData data3) {
		if (data1 != null && data1.dataType == SensorDataType.ANGLE)
			return data1.value;
		else if (data2 != null && data2.dataType == SensorDataType.ANGLE)
			return data2.value;
		else if (data3 != null && data3.dataType == SensorDataType.ANGLE)
			return data3.value;
		else
			// exception
			return 0;
	}

	double GetVelocity(SensorData data1, SensorData data2, SensorData data3) {
		if (data1 != null && data1.dataType == SensorDataType.VELOCITY)
			return data1.value;
		else if (data2 != null && data2.dataType == SensorDataType.VELOCITY)
			return data2.value;
		else if (data3 != null && data3.dataType == SensorDataType.VELOCITY)
			return data3.value;
		else
			// exception
			return 0;
	}

	double GetSound(SensorData data1, SensorData data2, SensorData data3) {
		if (data1 != null && data1.dataType == SensorDataType.SOUND)
			return data1.value;
		else if (data2 != null && data2.dataType == SensorDataType.SOUND)
			return data2.value;
		else if (data3 != null && data3.dataType == SensorDataType.SOUND)
			return data3.value;
		else
			// exception
			return 0;
	}
}

class SensorData {

	// 0: angle
	// 1: velocity
	// 2: sound
	private SensorDataType dataType;

	private double value;

	public SensorData(SensorDataType dataType, double value) {
		this.dataType = dataType;
		this.value = value;
	}
}