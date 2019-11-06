using UnityEngine;

public class Movement : MonoBehaviour
{
    public float horizontalSpeed = 10;
    public float verticalSpeed = 3;
    Rigidbody2D body;

    float horizontal;
    float vertical;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(horizontal * horizontalSpeed, vertical * verticalSpeed);
    }
}
