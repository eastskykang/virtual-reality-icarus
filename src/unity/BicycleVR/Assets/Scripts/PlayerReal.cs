using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

enum SensorDataType {ANGLE, VELOCITY, SOUND, INVALID};

public class PlayerReal: MonoBehaviour {

	private SerialPort	sp;

	public float upSpeed = 20.0f;

	private float forwardSpeed = 0.0f;
	public float rotationSpeed = 2.0f; 
	private float jump = 500.0f;
	private bool contact = true;

	private Rigidbody bikeRigidbody;

	private float angle_prevest = 0.0f;
	public float K_pa = 0.5f; // Gain for angle measurement
	public float decFactor= 0.5f;
	private float velocity_prevest = 0.0f;
	public float velocity_scale = 2000.0f;
	public float velocity_thres = 0.0f;
	private float velocity_eventTime = 0.0f;

	public const float K_pv = 0.5f; // Gain for angle measurement
	public const float angFactor = 0.001f;
	public float soundTreshold = 100.0f;
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
		sp.ReadTimeout = 5000;
	}

	void OnCollisionStay (Collision col)
	{
		forwardSpeed += 0.8f;
		transform.position += Vector3.forward * Time.deltaTime * forwardSpeed;
		Debug.Log (forwardSpeed);
		if (forwardSpeed > 30.0f)
		{
 			bikeRigidbody.AddForce (transform.up * jump);
		}
	}

	void OnCollisionExit (Collision col)
	{
		contact = false;
	}

	// Update is called once per frame
	void Update () {
		

		////////// Forward ////////// 
		transform.position += transform.forward * Time.deltaTime * forwardSpeed;

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
					//Debug.Log("Serial Out : " + packet1);
					//sp.BaseStream.Flush();

					data1 = ParsePacket(packet1);
				}

				if ((packet2 = sp.ReadLine()) != null) {
					//Debug.Log("Serial Out : " + packet2);
					//sp.BaseStream.Flush();

					data2 = ParsePacket(packet2);
				}

				if ((packet3 = sp.ReadLine()) != null) {
					//Debug.Log("Serial Out : " + packet3);
					//sp.BaseStream.Flush();

					data3 = ParsePacket(packet3);
				}

				if (contact == false){
					////////// Rotation //////////
					// angle of values
					// 0    - 511   : left    -90 to  0
					// 512  - 1023  : right   0   to  +90
					//					double angleInput = double.Parse(packet1) / 1024.0 * 180.0 - 90.0;
					//					transform.Rotate(0, (float) angleInput * Time.deltaTime, 0);

					float angle_meas = GetAngle (data1, data2, data3);
					float angle_err = angle_meas - angle_prevest;
					float angle_est =  angle_prevest + K_pa * angle_err;

  					bikeRigidbody.angularVelocity = new Vector3(0.0f, -angle_est * angFactor, 0.0f);

					//transform.eulerAngles.x = 0.0f;
					//transform.eulerAngles.z = 0.0f;
					angle_prevest = angle_est;

					////////// Flying ////////// 
					// frequency of wheel rotation 
					// unit: Hz
					float velocity_est = 0.0f; // Initialize velocity estimationb
					float velocity_meas = 0.0f;
					float velocity_err = 0.0f;
					float velocity_event =	GetVelocity (data1, data2, data3);

					if( velocity_event > 0){					
						float diffTime = Time.time - velocity_eventTime;
						velocity_eventTime = Time.time;

						velocity_meas = 2.0f / diffTime;
						velocity_err = velocity_meas - velocity_prevest;
						velocity_est =  velocity_prevest + K_pv * velocity_err;

						velocity_prevest = velocity_est; 
					}
					else{
						velocity_est = decFactor * velocity_prevest; 
						velocity_prevest = velocity_est;
					}

					float velocity_input = velocity_scale*(velocity_est - velocity_thres);
						
					Debug.Log("Velocity: Meas: " + velocity_meas +"Est: " + velocity_est+"In: " + velocity_input);

					RaycastHit hit;
					Ray landingRay = new Ray (transform.position, Vector3.down);
					Physics.Raycast(landingRay, out hit);
					bikeRigidbody.AddForce(transform.up * (velocity_input));
					// - 2*hit.distance
				}

				// signal value from sound sensor
				float sound = GetSound (data1, data2, data3);
				if (sound > soundTreshold)
				{
					gameController.TakeCoin (-1.0f);
				}
			}
			catch (UnityException)
			{
				Debug.Log ("System Exception");
			}
		}

		bikeRigidbody.velocity = Vector3.ClampMagnitude(bikeRigidbody.velocity, upSpeed);
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