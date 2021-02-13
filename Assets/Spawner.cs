using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    const float variation = .05f;

    public static int G = 45000;

    [SerializeField] GameObject _prefab;
    [SerializeField] int _spawnQuanity = 5;
    [SerializeField] float _initialSpeed = 25;
    [SerializeField] Text _text;

    int _spawnCount;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            G *= 5;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            G /= 5;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Time.timeScale = 1;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Time.timeScale = 2;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            Time.timeScale = 5;

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Backspace))
            G *= -1;

        if (Input.GetKey(KeyCode.Space))
        {
            for (int i = 0; i < _spawnQuanity; i++)
            {
                Spawn();
            }
        }

        _text.text = "object count: " + _spawnCount;
    }

    private void Spawn()
    {
        var go = Instantiate(_prefab, transform);
        _spawnCount++;
        go.transform.position = transform.position + (Random.insideUnitSphere * _spawnQuanity);
        Rigidbody a = go.GetComponent<Rigidbody>();
        var velocityVariation = new Vector3(Random.Range(-variation, variation), Random.Range(-variation, variation), Random.Range(-variation, variation));
        a.velocity = (transform.forward + velocityVariation) * _initialSpeed;
    }
}
