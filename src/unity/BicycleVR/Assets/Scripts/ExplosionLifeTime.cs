using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionLifeTime : MonoBehaviour {

	public float lifeTime = 2.5f;

	void Start()
	{
		Destroy(gameObject, lifeTime);
	}
}
