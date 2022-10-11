using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPhysicsBehaviour : MonoBehaviour
{

    public Transform target;
    private Rigidbody m_RigidBody;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        m_RigidBody.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

        Quaternion rotationDiff = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDiff.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDiffInDegree = angleInDegree * rotationAxis;
        m_RigidBody.angularVelocity = (rotationDiffInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }
}
