using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{
    public float lateralSpeed = 7f; //Kart Upgrade Field --> speed moving L & R
    public float jumpPower = 6f; //Kart Upgrade Field --> jump height
    public float duckTime = 0.5f; //Driver Upgrade Field --> duck length


    private Rigidbody rb;
    private Vector3 originalScale;

    private bool isGrounded;
    private bool isDucked = false;
    private bool isDriving = false;




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
        lateralSpeed = PlayerPrefs.GetFloat("Ls", 7f);
        jumpPower = PlayerPrefs.GetFloat("Jump", 6f);
        duckTime = PlayerPrefs.GetFloat("Duck", 0.5f);
    }

    public void LoadAndUpdateAttributes()
    {
        //load values from PlayerPrefs
        lateralSpeed = PlayerPrefs.GetFloat("Ls");
        jumpPower = PlayerPrefs.GetFloat("Jump");
        duckTime = PlayerPrefs.GetFloat("Duck");
    }

    void Update()
    {
        if (!isDriving)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(horizontalInput * lateralSpeed, rb.velocity.y, rb.velocity.z);

        //jumping logic
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false; //assume not grounded as soon as we jump
        }

        //ducking logic
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isGrounded && !isDucked) //check player is eligible to duck
        {
            Duck();
            StartCoroutine(ChangeShapeBackAfterDelay(duckTime));
        }
    }

    //crash logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    //ground check logic
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
            {
                isGrounded = true;
            }
        }
    }

    //ducking logic
    void Duck()
    {
        transform.localScale = new Vector3(1, 0.5f, 1);
        isDucked = true;

        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            //collider.radius = 0.3825129f; //adjusts the collider's radius
            collider.height = 1.2f; //adjusts the collider's height
        }
    }

    //logic to unduck
    IEnumerator ChangeShapeBackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.localScale = originalScale;
        isDucked = false;

        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            collider.radius = 0.3825129f; //reset to original radius
            collider.height = 1.600305f; //reset to original height
        }
    }

    //start game value set to true
    public void startDriving()
    {
        isDriving = true;

    }


}
