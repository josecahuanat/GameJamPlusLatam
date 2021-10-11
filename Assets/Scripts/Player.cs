using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [System.NonSerialized] public float defaultSpeed;
    public float speed;
    public GameObject drumstick;
    public float throwStrength;
    public float topLimit, RightLimit, LeftLimit, BottomLimit;

    Vector3 direction;

    public List<GameObject> drumsticks;

    void Start()
    {
        defaultSpeed = speed;
        enabled = false;
        drumsticks = new List<GameObject>();
    }

    void Update()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);


        Move();

        drumsticks.RemoveAll((d) => d == null);

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) && drumsticks.Count < 2)
        {
            Throw();
        }
    }

    void Move()
    {
        direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A) && transform.position.x > LeftLimit)
        {
            direction.x = -1;
        }
        else if (Input.GetKey(KeyCode.D) && transform.position.x < RightLimit)
        {
            direction.x = 1;
        }

        if (Input.GetKey(KeyCode.W) && transform.position.y < topLimit)
        {
            direction.y = 1;
        }
        else if (Input.GetKey(KeyCode.S) && transform.position.y > BottomLimit)
        {
            direction.y = -1;
        }

        transform.position += direction * speed * Time.deltaTime;
    }



    void Throw()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        var drumstickGB = GameObject.Instantiate(drumstick, transform.position, transform.rotation);
        drumstickGB.GetComponent<Drumstick>().speed = throwStrength;

        drumsticks.Add(drumstickGB);
    }
}
