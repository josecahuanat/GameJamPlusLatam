using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector2 input;
    float shipAngle;

    public float speed;
    public float rotationInterpolation = 0.4f;// velocidad de firo del personaje
    public bool isMoving;

    public Transform mira;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        if (input.x != 0 || input.y != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Detectar mouse y posicionar putnero
        mira.position = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            -Camera.main.transform.position.z
            ));

        if (Input.GetButtonDown("Fire1")) disparar();

    }

    void disparar()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (mira.position - mira.position).normalized, 1000f, ~(1 << 6));
        if (hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = input * speed * Time.fixedDeltaTime;
        GetRotation();
    }

    void GetRotation()
    {
        Vector2 lookDir = new Vector2(-input.x, input.y);
        if (isMoving)
        {
            shipAngle = Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg;
        }

        if (rb.rotation <= -90 && shipAngle >= 90)
        {
            rb.rotation += 360;
            rb.rotation = Mathf.Lerp(rb.rotation, shipAngle, rotationInterpolation);
        }else if(rb.rotation >= 90 && shipAngle <= -90)
        {
            rb.rotation -= 360;
            rb.rotation = Mathf.Lerp(rb.rotation, shipAngle, rotationInterpolation);
        }
        else
        {
            rb.rotation = Mathf.Lerp(rb.rotation, shipAngle, rotationInterpolation);
        }
    }
}
