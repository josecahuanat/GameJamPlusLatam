using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public AudioClip[] sounds;

    public float speed;

    void Start()
    {
        var rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            GameManager.playerHealth--;
        }
        else if (col.transform.CompareTag("Drumstick"))
        {
            GameManager.enemies--;
            if (col.transform.childCount > 0)
                col.transform.GetChild(0).DOKill();
            int randomSound = Random.Range(0, sounds.Length);
            Camera.main.GetComponent<AudioSource>().PlayOneShot(sounds[randomSound]);
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
