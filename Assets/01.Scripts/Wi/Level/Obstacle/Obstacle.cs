using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
	[Range(0, 1f)]
	[SerializeField] float resistance = 0.0f;

    private Rigidbody rigid;
	private Vector3 originPos;
	private Quaternion originRot;
	private bool active = true;

	private Coroutine disableCo;

	private const float mass = 10.0f;

	public void Initialize()
	{
		if (disableCo != null)
			StopCoroutine(disableCo);
		disableCo = null;
		gameObject.SetActive(true);
		active = true;
		rigid.mass = mass;
		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;
		rigid.useGravity = false; 
		transform.SetPositionAndRotation(transform.parent.TransformPoint(originPos), originRot);
	}

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		rigid.useGravity = false;
		originPos = transform.parent.InverseTransformPoint(transform.position);
		originRot = transform.rotation;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.articulationBody is null) return;
		rigid.useGravity = true;
		if (disableCo == null)
			disableCo = StartCoroutine(DisableCo());
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody != null)
		{
			if (active)
			{
				other.attachedRigidbody.velocity *= (1.0f - resistance);
				active = false;
			}
			rigid.velocity = other.attachedRigidbody.velocity * 1.5f + Vector3.up * 2f;
			float v = Random.Range(-10f, 10f);
			rigid.angularVelocity = Vector3.one * v;
			rigid.useGravity = true;

			if (disableCo == null)
				disableCo = StartCoroutine(DisableCo());
		}
	}

	private IEnumerator DisableCo()
	{
		yield return new WaitForSeconds(5f);
		gameObject.SetActive(false);
	}
}
