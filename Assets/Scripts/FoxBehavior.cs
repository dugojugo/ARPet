using System;
using System.Collections;
using UnityEngine;

public class FoxBehavior : MonoBehaviour
{
    [SerializeField]
    public float speed = 0.1f;
    Animator anim;
    public float rotationSpeed = 0.5f; // Adjust the rotation speed as needed
    public float gravityStrength = 50f; // Adjust the gravity strength as needed
    private Rigidbody rb;
    int timer;
    Collider col;
    private bool turnR = true;
    private bool turnL = true;
    private int waitS = 0;
    bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponentInChildren<Collider>();
        if (col == null)
        {
            Debug.Log("No collider found");
        }
        else
        {
            Debug.Log("Found: " + col.name.ToString());
        }
        timer = 0;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Ensure that the Rigidbody component is not null
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the GameObject. Attach a Rigidbody to enable gravity.");
        }
        else
        {
            // Enable gravity on the Rigidbody
            rb.useGravity = false; // We'll apply custom gravity forces
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject targetPos = GameObject.FindGameObjectWithTag("move");
        if (targetPos != null && collided==false)
        {

            MoveTowards(targetPos);
        }

        //ApplyGravity();
        if (targetPos == null )
        {
            collided = false;
            //RandomBehavior();
        }
        if (collided == true)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            collided = true;
            anim.StopPlayback();
            //StopOn();
            //anim.Play("Fox_Attack_Paws");
        }

    }
    void StopOn()
    {

        transform.Translate(Vector3.forward * 0);
        //anim.StartPlayback();
        if (!isPlaying(anim, "Fox_Idle"))
        {
            anim.Play("Fox_Idle");
        }
        
    }
    void moveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if(!isPlaying(anim, "Fox_Run"))
        {
            anim.Play("Fox_Run");
        }
        waitS += 1;
        if(waitS >= 10)
        {
            turnR = true;
            turnL = true;   
            waitS = 0;
        }
        //Debug.Log(waitS);
        
        
    }
    void turnRight()
    {
        
        if(turnR == true)
        {
            turnL = false;
            float rotationStep = rotationSpeed * Time.deltaTime;

            // Rotate the object gradually towards the target angle
            transform.Rotate(Vector3.up, rotationStep);
            
        }
        

       
        
    }
    void turnLeft()
    {
        if(turnL == true)
        {
            turnR = false;
            float rotationStep = rotationSpeed * Time.deltaTime;

            // Rotate the object gradually towards the target angle
            transform.Rotate(-Vector3.up, rotationStep);
        }
        



    }
    void ApplyGravity()
    {
        // Ensure that the Rigidbody component is not null
        if (rb != null)
        {
            // Calculate the gravity force vector
            Vector3 gravityForce = Vector3.down * gravityStrength;

            // Apply the gravity force to the Rigidbody
            rb.AddForce(gravityForce, ForceMode.Acceleration);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log(collision.gameObject.name.ToString());
        if (collision.gameObject.tag == "ball")
        {

            anim.Play("Fox_Attack_Paws");
        }
        if(collision.gameObject.tag == "move")
        {
            anim.Play("Fox_Attack_Paws");

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            collided = true;
            //anim.StopPlayback();
            //StopOn();
            //anim.Play("Fox_Idle");
            StartCoroutine(DelayDestroy(collision.gameObject));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "move")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            collided = true;
            anim.StopPlayback();
            StopOn();
            anim.Play("Fox_Idle");
        }
    }
        void RandomBehavior()
    {
        int randomAction = UnityEngine.Random.Range(0, 4); // 0: moveForward, 1: turnRight, 2: turnLeft, 3: stop

        switch (randomAction)
        {
            case 0:
                moveForward();
                break;
            case 1:
                /* for(int i = 0; i< Random.Range(20, 150); i++)
                 {
                     //turnRight();

                 }
                */
                StartCoroutine(LoopWithDelay(UnityEngine.Random.Range(80, 150)));
                turnR = false;

                break;
            case 2:
                /*for (int i = 0; i < UnityEngine.Random.Range(20, 150); i++)
                {
                    turnLeft();
                }
                */
                StartCoroutine(LoopWithDelay1(UnityEngine.Random.Range(80, 150)));

                turnL = false;

                break;
            case 3:
                for (int i = 0; i < UnityEngine.Random.Range(200, 350); i++)
                {
                    StopOn();
                }
                
                break;
        }
    }
    bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
        anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
    IEnumerator LoopWithDelay(int n )
    {
        // Loop from 0 to 10 with increment of 1
        for (int i = 0; i <= n; i++)
        {

            turnRight();
            //Debug.Log("Current value of i: " + i);

            // Do some processing here...

            // Wait for 1 second after every increment
            yield return new WaitForSeconds(1.0f);
        }
    }
    IEnumerator LoopWithDelay1(int n)
    {
        // Loop from 0 to 10 with increment of 1
        for (int i = 0; i <= n; i++)
        {

            turnLeft();
            //Debug.Log("Current value of i: " + i);

            // Do some processing here...

            // Wait for 1 second after every increment
            yield return new WaitForSeconds(1.0f);
        }
    }
    IEnumerator DelayDestroy(GameObject k)
    {
        Animator animCat = k.gameObject.GetComponentInChildren<Animator>();
        animCat.Play("hit");
        yield return new WaitForSeconds(5f);
        Destroy(k);
    }
        void MoveTowards(GameObject target)
    {

        anim.Play("Fox_Run");
        Vector3 directionToTarget = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime*0.1f);

        // Move towards the target
        //moveForward();
        transform.Translate(Vector3.forward * speed * Time.deltaTime*0.01f);

    }
}
