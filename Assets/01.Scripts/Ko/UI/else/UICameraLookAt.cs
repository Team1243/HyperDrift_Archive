using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraLookAt : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private Vector3 offset;
    private void FixedUpdate()
    {
        //transform.LookAt(transform.position + offset + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
        //transform.position = _pivot.transform.localPosition;
        transform.position = _pivot.position + offset;
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
