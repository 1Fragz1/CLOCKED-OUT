

using UnityEngine;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 50f;

    [Header("Combat")]
    public float knockback = 10f;
    public float percentage = 0f;
    public float damagePerHit = 10f;

    [Header("UI")]
    [SerializeField] private Text healthText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on " + gameObject.name);
            enabled = false;
            return;
        }

        UpdatePercentage();
    }

    void FixedUpdate()
    {
        // Horizontal movement
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
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x * 0.97f,
                rb.linearVelocity.y
            );
        }

        // Jump
        if (Input.GetKey(KeyCode.W) &&
            Mathf.Abs(rb.linearVelocity.y) < 0.001f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Only damage other objects tagged Player
        if (!collision.collider.CompareTag("Player"))
            return;

        PlayerScript otherPlayer =
            collision.collider.GetComponentInParent<PlayerScript>();

        // Ignore missing scripts and ourselves
        if (otherPlayer == null || otherPlayer == this)
            return;

        Rigidbody2D otherRb = otherPlayer.GetComponent<Rigidbody2D>();

        if (otherRb == null)
            return;

        // Calculate knockback using the opponent's percentage
        float knockbackMultiplier =
            1f + (otherPlayer.percentage / 100f);

        // Calculate damage based on relative velocity
        float relativeSpeed =
            (rb.linearVelocity - otherRb.linearVelocity).magnitude;

        damagePerHit =
            (10f + (otherPlayer.percentage / 10f)
            + knockback * knockbackMultiplier)
            * relativeSpeed / 30f;

        // Prevent negative damage
        damagePerHit = Mathf.Max(0f, damagePerHit);

        // Apply damage to the opponent
        otherPlayer.percentage += damagePerHit;
        otherPlayer.UpdatePercentage();

        Debug.Log(
            otherPlayer.name + " damage: " +
            otherPlayer.percentage.ToString("F1") + "%"
        );

        // Apply knockback
        Vector2 direction =
            (otherPlayer.transform.position - transform.position).normalized;

        // Prevent a zero direction
        if (direction == Vector2.zero)
            direction = Vector2.up;

        otherRb.AddForce(
            direction * knockback * knockbackMultiplier,
            ForceMode2D.Impulse
        );
    }

    private void UpdatePercentage()
    {
        if (healthText != null)
        {
            healthText.text = $"Player: {percentage:F0}%";
        }
    }
}
