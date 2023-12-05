using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
	private CarMovement carMovement;

	private void Awake()
	{
		carMovement = FindObjectOfType<CarMovement>();
	}

	private void OnTriggerEnter(Collider other)
	{
		carMovement.IsBooster = true;
	}

	private void OnTriggerExit(Collider other)
	{
		carMovement.ActiveBoosterForSecond(2f);
	}


}
