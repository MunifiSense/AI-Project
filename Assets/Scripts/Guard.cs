using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Guard AI main file

public class Guard : MonoBehaviour
{
    public float maxSpeed = 1;
    public float radius = 1;
    public float slowRadius = 2;
    public float radiusRotate = 10;
    public float slowRadiusRotate = 30;
    //public float predictTime = 0.1f;
    public float pathOffset = 1.0f;
    public Vector3 velocity;
    public Vector3 rotation;
    public Vector3 linear;
    public Vector3 angular;
    public float maxAcceleration = 1;
    public float maxAngularAcceleration = 30;
    public float maxRotation = 5;

    public float viewDistance = 10;
    public float viewAngle = 110;

    // For decision making
    public bool playerInSight;
    public bool playerNearby;
    public bool playerAggressive;
    public bool haveBackup;
    public Vector3 playerLastSighting;

    public int patrolFrom;
    public int patrolTo;

    private Path path;
    private GameObject player;

    // Decision Making
    GuardDecisionMaking decisionMaking;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInSight = false;
        decisionMaking = GetComponent<GuardDecisionMaking>();

        // Generate patrol path
        path = AStarPathFinding.FindPath(patrolFrom, patrolTo);
        path.CalcParams();
        //path = AStarPathFinding.FindPath(0, 16);
        //path.CalcParams();
        // Drawing path for debugging
        //path.DrawPath();

    }

    void Update()
    {
        if (playerInSight)
        {
            Debug.Log("Player detected!!");

            // Trigger alert state
        }

        GuardActions(decisionMaking.currentAction);
    }

    public void GuardActions(string action)
    {
        switch (action)
        {
            case "patrol":
                if(Patrol(patrolFrom, patrolTo))
                {
                    int temp = patrolFrom;
                    patrolFrom = patrolTo;
                    patrolTo = temp;
                    path = AStarPathFinding.FindPath(patrolFrom, patrolTo);
                    path.CalcParams();
                }
                break;
            case "alarm":
                break;
            case "attack":
                break;
            case "wait":
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position += velocity * Time.fixedDeltaTime;
        //gameObject.GetComponent<CharacterController>().SimpleMove(velocity);
        gameObject.transform.localEulerAngles = GetNewOrientation();
        //gameObject.transform.localEulerAngles += rotation * Time.fixedDeltaTime;

        velocity += linear * Time.fixedDeltaTime;

        gameObject.GetComponentInChildren<Animator>().SetFloat("Forward", velocity.magnitude);
        //rotation +=   angular * Time.fixedDeltaTime;

        /*if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }*/

        FollowPath(path, pathOffset);
    }

    private void OnTriggerStay(Collider other)
    {
        playerInSight = false;
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Player in trigger");


            // Vision
            Vector3 targetDir = player.transform.position - transform.position;
            float angle = Vector3.Angle(targetDir, -transform.forward);

            //Debug.Log("angle = " + angle);

            // Is player in field of view
            if (angle < viewAngle * 0.5f)
            {
                //Debug.Log("Player in angle");
                RaycastHit hit;

                if (Physics.Raycast(transform.position + transform.up, targetDir.normalized, 
                    out hit, GetComponent<BoxCollider>().size.z, LayerMask.GetMask("LocalPlayer")))
                {
                    if(hit.collider.gameObject.name == "Trigger")
                    {
                        playerInSight = true;
                        playerLastSighting = hit.collider.gameObject.transform.position;
                    }
                }
            }
        }
    }

    bool FollowPath(Path path, float pathOffset)
    {
        float currentParam;
        Vector3 futurePos = gameObject.transform.position + velocity * Time.fixedDeltaTime;
        currentParam = path.getParam(futurePos);
        float targetParam = currentParam + pathOffset;
        Vector3 targetPos = path.getPosition(targetParam);
        return Arrive(targetPos);
        //LookWhereYoureGoing();
    }

    void Seek(Vector3 target)
    {
        Vector3 linearCalc = target - gameObject.transform.position;

        linearCalc.Normalize();
        linearCalc *= maxSpeed;
        linear = linearCalc;
    }

    void LookWhereYoureGoing()
    {
        if(velocity.magnitude == 0)
        {
            angular = Vector3.zero;
            rotation = Vector3.zero;
        }
        else
        {
            Vector3 target = new Vector3(0, Mathf.Atan2(-velocity.x, velocity.z)*Mathf.Rad2Deg, 0);
            //Debug.Log("Target: " + target);
            Align(target);
        }
    }

    void Align(Vector3 target)
    {
        Vector3 rotationCalc = target - gameObject.transform.localEulerAngles;
        if(rotationCalc.y > 180)
        {
            rotationCalc.y -= 360;
        }
        if (rotationCalc.y < -180)
        {
            rotationCalc.y += 360;
        }
        rotation = rotationCalc;
        Debug.Log(rotation);
        float rotationSize = Mathf.Abs(rotationCalc.y);
        Vector3 targetRotation;

        //Debug.Log("rotationSize: " + rotationSize + " radiusRotate: " + radiusRotate);

        if (rotationSize < radiusRotate)
        {
            angular = Vector3.zero;
            rotation = Vector3.zero;
        }
        else
        {
            if(rotationSize > slowRadius)
            {
                targetRotation = new Vector3(0, maxRotation, 0);
            }
            else
            {
                targetRotation = new Vector3(0, maxRotation, 0) * rotationSize / slowRadiusRotate;
            }

            targetRotation *= rotationCalc.y / rotationSize; 

            Debug.Log("TargetRotation: " + targetRotation);

            Vector3 angularCalc = targetRotation - gameObject.transform.localEulerAngles;
            angularCalc /= Time.fixedDeltaTime;

            float angularAcceleration = Mathf.Abs(angularCalc.y);
            if(angularAcceleration > maxAngularAcceleration)
            {
                angularCalc /= angularAcceleration;
                angularCalc *= maxAngularAcceleration;
            }

            angular = angularCalc;
        }
    }

    bool Arrive(Vector3 target)
    {
        Vector3 direction = target - gameObject.transform.position;
        float distance = direction.magnitude;

        // If we are at target
        if (distance < radius)
        {
            linear = Vector3.zero;
            velocity = Vector3.zero;
            return true;
        }
        else
        {
            float targetSpeed;
            if (distance > slowRadius)
            {
                targetSpeed = maxSpeed;
            }
            else
            {
                targetSpeed = maxSpeed * distance / slowRadius;
            }

            Vector3 targetVelocity = direction;
            targetVelocity.Normalize();
            targetVelocity *= targetSpeed;


            Vector3 linearCalc = targetVelocity - velocity;
            linearCalc /= Time.fixedDeltaTime;

            if (linearCalc.magnitude > maxAcceleration)
            {
                linearCalc.Normalize();
                linearCalc *= maxAcceleration;
            }

            linear = linearCalc;
        }
        return false;
    }
    
    private Vector3 GetNewOrientation()
    {
        if(velocity.magnitude > 0)
        {
            return new Vector3(0, Mathf.Atan2(-velocity.x, -velocity.z)*Mathf.Rad2Deg, 0);
        }
        return gameObject.transform.localEulerAngles;
    }

    public void FindPath(int from, int to)
    {
        path = AStarPathFinding.FindPath(from, to);
        path.CalcParams();
        // Drawing path for debugging
        //path.DrawPath();
    }

    private bool Patrol(int from, int to)
    {
        return FollowPath(path, pathOffset);
    }
}
