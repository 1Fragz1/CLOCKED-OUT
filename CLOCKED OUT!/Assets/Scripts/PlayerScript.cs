using System.Collections.Specialized;

using System.Security.Cryptography;
using System.Security.Permissions;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;

    public float knockback = 10f;
    public float moveSpeed = 5f;
    public float jumpForce = 50f;
    private float percentage = 0f;
    public Camera cam; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Force);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Force);

        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.97f, rb.linearVelocity.y);
        }
        if (Input.GetKey(KeyCode.W) && Mathf.Abs(rb.linearVelocity.y) < 0.001f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Hit();
        // }
    }

    //void Hit()
    //{
    //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

    //

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null && collision.collider.CompareTag("Player"))
        {
            Debug.Log("Hit " + collision.collider.name);
            percentage = 1f - (Vector2.Distance(transform.position, collision.collider.transform.position) / 5f);

            collision.collider.GetComponent<Rigidbody2D>().AddForce((collision.collider.transform.position - transform.position).normalized * knockback * percentage, ForceMode2D.Impulse);
        }
    }


}
