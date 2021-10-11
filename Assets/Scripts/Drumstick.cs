using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Drumstick : MonoBehaviour
{
    public float speed;

    void Start()
    {
        transform.GetChild(0).DORotate(Vector3.forward * 1000f, 1f, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental);

        DOVirtual.DelayedCall(4f, () =>
        {
            transform.GetChild(0).DOKill();
            Destroy(gameObject);
        });
    }

    void Update()
    {
        transform.localPosition += transform.TransformDirection(Vector3.right) * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Wall"))
        {
            if (transform.childCount > 0)
            {
                transform.GetChild(0).DOKill();
                Destroy(gameObject);
            }
        }
    }
}
