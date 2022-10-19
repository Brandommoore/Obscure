using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



/*############# METODO 1 ###############*/


/*public class Neo_movement : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSpeed = 5f;
    public float Speed = 2f;
    public float JumpForce = 130;
    public bool grounded;

    private float Horizontal;
    private Rigidbody2D rb2d;
    private Animator anim;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(Horizontal * Speed, rb2d.velocity.y);
    }

    private void Jump()
    {
        rb2d.AddForce(Vector2.up * JumpForce);
    }

    // Update is called once per frame
    void Update()
    {
        // Seteamos valores para el animador
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("Grounded", grounded);

        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }

}*/



/*############# METODO 2 ###############*/

public class Neo_movement : MonoBehaviour {

    [SerializeField]
    private LayerMask themasks;

    public float maxSpeed = 5f;
    public float speed = 2f;
    public bool grounded;
    public int scene;

    public GameObject JumpSound;
    public GameObject PlatformJump;
    public GameObject DeadSound;
    public GameObject GemaSound;

    public Vector2 originalPos;
    private float Horizontal;
    public bool dead = false;
    private float height;
    private float width;
    private bool justEntered = false;

    private float JumpForce = 150f;
    private bool hasPressedJump = false;
    private bool canDobleJump = false;
    private bool isOnShard = false;
    private string shardName = "";
    private int numberOfJumps = 0;

    private Rigidbody2D rb2d;
    private Animator anim;
    private BoxCollider2D thebox;
    private Object De;

    private bool firstButtonPressed = false;
    private bool reset = false;
    private float timeOfFirstButton;

    public DashState dashState;
    public float dashTimer = 0.1f;
    public float maxDash = 50f;

    public Vector2 savedVelocity;
    private bool canDash = false;

    public bool winned = false;

    void Start()
    {
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        thebox = GetComponent<BoxCollider2D>();
    }

    void Awake()
    {
        originalPos = new Vector2(transform.position.x, transform.position.y);
    }

    void Update()
    {
        if (dead) return ;
        anim.SetFloat("Speed", rb2d.velocity.x);
        anim.SetBool("Grounded", grounded);

        Horizontal = Input.GetAxis("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (Physics2D.Raycast(transform.position, Vector3.down, 0.05f))
        {
            grounded = true;
            numberOfJumps = 0;

        }
        else grounded = false;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            hasPressedJump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Debug.Log("Dash");
            if (Horizontal < 0.0f)
                dashState = DashState.Left;
            else if (Horizontal > 0.0f)
                dashState = DashState.Rigth;
        }

        if (dashState == DashState.Left || dashState == DashState.Rigth)
        {
            if (dashTimer <= 0)
            {
                dashState = DashState.End;
                rb2d.velocity = Vector2.zero;
                dashTimer = 0.1f;
                Debug.Log("TIme 0");
            }
            else
            {
                Debug.Log("Time x");
                dashTimer -= Time.deltaTime;
                if (dashState == DashState.Left)
                    rb2d.AddForce(Vector2.left * 110f);
                else if (dashState == DashState.Rigth)
                    rb2d.AddForce(Vector2.right * 110f);
            }
        }
    }

    private void Jump(float extraForce)
    {
        Instantiate(JumpSound);
        rb2d.AddForce(Vector2.up * (JumpForce + extraForce));
    }

    void FixedUpdate()
    {
        if (dead) return ;
        float h = Input.GetAxis("Horizontal");

        rb2d.AddForce(Vector2.right * speed * h);

        float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
        rb2d.velocity = new Vector2(Horizontal * speed, rb2d.velocity.y);

        if (((canDobleJump && numberOfJumps == 1) || (grounded && numberOfJumps == 0))
            && hasPressedJump)
        {
            Jump(0f);
            numberOfJumps++;
            hasPressedJump = false;
        }

        if (hasPressedJump && isOnShard)
        {
            if (shardName == "JumpShardStrong")
            {
                Jump(60f);
            }
            else if (shardName == "JumpShardMedium")
            {
                Jump(20f);
            }
            else if (shardName == "JumpShardSmall")
            {
                Jump(10f);
            }

            hasPressedJump = false;
        }
    }

    private bool ground()
    {
        RaycastHit2D Raycasting = Physics2D.BoxCast(thebox.bounds.center, thebox.bounds.size, 0f, Vector2.down * 1f, themasks);

        return Raycasting.collider != null;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject death_screen = GameObject.Find("Death_Screen");
        if ((other.gameObject.tag == "Void" || other.gameObject.tag == "Spikes")  && dead == false)
        {
            dead = true;
            Instantiate(DeadSound);
            anim.SetBool("Dead", dead);
            rb2d.AddForce(Vector2.zero);
            rb2d.AddForce(Vector2.up * JumpForce);
            death_screen.GetComponent<Canvas>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        GameObject win_screen = GameObject.Find("Win_screem");
        if (other.gameObject.tag == "BlueGem" && dead == false)
        {
            canDobleJump = true;
            GameObject.FindGameObjectWithTag("BlueGem").SetActive(false);
            Instantiate(GemaSound);
        }
        else if (other.gameObject.tag.Contains("JumpShard") && dead == false)
        {
            isOnShard = true;
            shardName = other.gameObject.tag;

        }else if (other.gameObject.tag == "CheckPoint" && dead == false)
        {
            originalPos = new Vector2(transform.position.x, transform.position.y);
        }
        else if (other.gameObject.tag == "GreenGem" && dead == false)
        {
            canDash = true;
            GameObject.FindGameObjectWithTag("GreenGem").SetActive(false);
            //win_screen.GetComponent<Canvas>().enabled = true;
            //winned = true;
            Instantiate(GemaSound);
        }
        else if (other.gameObject.tag == "RedGem" && dead == false)
        {
            GameObject.FindGameObjectWithTag("RedGem").SetActive(false);
            win_screen.GetComponent<Canvas>().enabled = true;
            winned = true;
            Instantiate(GemaSound);
        }
        else if (other.gameObject.tag == "Next_level" && dead == false)
        {
            SceneManager.LoadScene(scene);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Contains("JumpShard") && dead == false)
        {
            isOnShard = false;
            shardName = "";
        }
    }
}

public enum DashState
{
    Left,
    Rigth,
    End
}