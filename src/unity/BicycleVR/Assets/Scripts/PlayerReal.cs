using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

enum SensorDataType {ANGLE, VELOCITY, SOUND, INVALID};

public class PlayerReal: MonoBehaviour {

	private SerialPort	sp;

	public float forwardSpeed = 20.0f;
	private float flyingSpeed = 200.0f;
	public float rotationSpeed = 2.0f;
	private Rigidbody bikeRigidbody;
	private float angle_prev = 0.0f;
	public float lambda = 0.2f;
	public float scale = 1.0f;
	public float soundTreshold = 150.0f;
	private GameController gameController;

	// Use this for initialization
	void Start () {

		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameControllerObject == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}

		bikeRigidbody = GetComponent<Rigidbody>();

		switch (SystemInfo.operatingSystemFamily) 
		{
		case OperatingSystemFamily.Windows:
			sp = new SerialPort ("COM5", 9600);
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

		transform.position += transform.forward * Time.deltaTime * forwardSpeed;

		RaycastHit hit;
		Ray landingRay = new Ray (transform.position, Vector3.down);
		Physics.Raycast(landingRay, out hit);

		if (Input.GetKey(KeyCode.Space))
		{
			bikeRigidbody.AddForce(transform.up * (flyingSpeed - 2*hit.distance));
		}

		SensorData data1 = new SensorData();
		SensorData data2 = new SensorData();
		SensorData data3 = new SensorData();

		if (sp.IsOpen)
		{
			try
			{
				string packet1;
				string packet2;
				string packet3;

				if ((packet1 = sp.ReadLine()) != null) {
					Debug.Log("Serial Out : " + packet1);
					//sp.BaseStream.Flush();

					data1 = ParsePacket(packet1);
				}

				if ((packet2 = sp.ReadLine()) != null) {
					Debug.Log("Serial Out : " + packet2);
					//sp.BaseStream.Flush();

					data2 = ParsePacket(packet2);
				}

				if ((packet3 = sp.ReadLine()) != null) {
					Debug.Log("Serial Out : " + packet3);
					//sp.BaseStream.Flush();

					data3 = ParsePacket(packet3);
				}

				// angle of values
				// 0    - 511   : left    -90 to  0
				// 512  - 1023  : right   0   to  +90
				//					double angleInput = double.Parse(packet1) / 1024.0 * 180.0 - 90.0;
				//					transform.Rotate(0, (float) angleInput * Time.deltaTime, 0);
				float angle = GetAngle (data1, data2, data3);
				float angle_new = lambda * angle_prev + (1 - lambda) * angle;
				transform.eulerAngles = new Vector3(0.0f, angle_new, 0.0f);
				angle_prev = angle_new;

				// frequency of wheel rotation 
				// unit: Hz
				float velocity = GetVelocity (data1, data2, data3);
				Debug.Log(+velocity);
				bikeRigidbody.AddForce(transform.up * (flyingSpeed - 2*hit.distance) * velocity * scale);

				// signal value from sound sensor
				float sound = GetSound (data1, data2, data3);
				if (sound > soundTreshold)
				{
					gameController.TakeCoin (-2.0f);
				}
			}
			catch (UnityException)
			{
				Debug.Log ("System Exception");
			}
		}
	}

	SensorData ParsePacket (string str) {
		// Packets
		string anglePrefix = "AN";
		string velocityPrefix = "VE";
		string soundPrefix = "DB";

		string[] strs = str.Split(new string[] {"::"}, System.StringSplitOptions.None);

		if (strs.Length != 2)
			// exception
			Debug.Log ("ParsePacket: Packet is not valid");

		if (string.Equals (strs[0], anglePrefix)) {
			return new SensorData (SensorDataType.ANGLE, float.Parse (strs [1]));
		}
		else if (string.Equals(strs[0], velocityPrefix)) {
			return new SensorData (SensorDataType.VELOCITY, float.Parse (strs [1]));
		}
		else if (string.Equals(strs[0], soundPrefix)) {
			return new SensorData (SensorDataType.SOUND, float.Parse (strs [1]));
		}
		else {
			// exception 
			throw new UnityException ("ParsePacket: Packet is not valid"); 
		}
	}

	float GetAngle(SensorData data1, SensorData data2, SensorData data3) {
		if (data1 != null && data1.dataType == SensorDataType.ANGLE)
			return data1.value;
		else if (data2 != null && data2.dataType == SensorDataType.ANGLE)
			return data2.value;
		else if (data3 != null && data3.dataType == SensorDataType.ANGLE)
			return data3.value;
		else
			// exception
			throw new UnityException ("GetAngle: Packet is not valid"); 
	}

	float GetVelocity(SensorData data1, SensorData data2, SensorData data3) {
		if (data1 != null && data1.dataType == SensorDataType.VELOCITY)
			return data1.value;
		else if (data2 != null && data2.dataType == SensorDataType.VELOCITY)
			return data2.value;
		else if (data3 != null && data3.dataType == SensorDataType.VELOCITY)
			return data3.value;
		else
			// exception
			throw new UnityException ("GetVelocity: Packet is not valid"); 
	}

	float GetSound(SensorData data1, SensorData data2, SensorData data3) {
		if (data1 != null && data1.dataType == SensorDataType.SOUND)
			return data1.value;
		else if (data2 != null && data2.dataType == SensorDataType.SOUND)
			return data2.value;
		else if (data3 != null && data3.dataType == SensorDataType.SOUND)
			return data3.value;
		else
			// exception
			throw new UnityException ("GetSound: Data is not valid"); 
	}
}

class SensorData {

	// 0: angle
	// 1: velocity
	// 2: sound
	public SensorDataType dataType;

	public float value;

	public SensorData() {
		this.dataType = SensorDataType.INVALID;
		this.value = 0;
	}

	public SensorData(SensorDataType dataType, float value) {
		this.dataType = dataType;
		this.value = value;
	}
}