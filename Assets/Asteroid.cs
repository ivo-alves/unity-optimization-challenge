using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Vector3 PlanetPosition;

    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 dir = PlanetPosition - transform.position;
        float distance = dir.magnitude;
        float force = Spawner.G / Mathf.Pow(distance, 2);

        body.AddForce(dir.normalized * force);
    }
}
