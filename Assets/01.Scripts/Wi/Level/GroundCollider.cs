using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform col;

	private void Update()
	{
		transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
	}
}
