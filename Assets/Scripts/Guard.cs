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

    // Naive Bayes Classifier
    // Chance of getting shot at by player on sight > 80%
    public bool playerAggressive;

    // Has the guard been shot at during this attack phase
    public bool shotAt;

    // Has the player been spotted during this attack phase
    public bool playerSighted;

    public bool atPlayerLastLocation;
    public int nearbyRoomTripped = -1;
    public bool arrivedAtRoom;
    public Vector3 playerLastSighting;
    public Vector3 playerLastLastSighting;

    // Number of rooms the guard has checked to find the player
    public int checkedRooms;

    public int patrolFrom;
    public int patrolTo;

    public bool atPatrol;

    public Path path;
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
        //path = AStarPathFinding.FindPath(0, 27);
        //path.CalcParams();
        // Drawing path for debugging
        //path.DrawPath();

    }

    void Update()
    {
        if (playerInSight)
        {
            //Debug.Log("Player detected!!");
            GetComponentInChildren<AudioSource>().enabled = true;
            playerLastSighting = player.transform.position;
            GameObject.Find("Node (42)").transform.position = playerLastSighting;
        }
        else if(!playerInSight && !GetComponentInChildren<AudioSource>().isPlaying)
        {
            GetComponentInChildren<AudioSource>().enabled = false;
        }

        // If player is nearby
        if(Vector3.Distance(transform.position, player.transform.position) < 10)
        {
            playerNearby = true;
        }
        else
        {
            playerNearby = false;
        }

        //FollowPath(path, pathOffset);

        //GuardActions(decisionMaking.currentAction);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position += velocity * Time.fixedDeltaTime;
        gameObject.transform.localEulerAngles = GetNewOrientation();

        velocity += linear * Time.fixedDeltaTime;

        gameObject.GetComponentInChildren<Animator>().SetFloat("Forward", velocity.magnitude);
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
                        if (GetComponent<GuardDecisionMaking>().sm.currentState.action == "attack")
                        {
                            playerSighted = true;
                        }
                    }
                }
            }
        }
    }

    public bool FollowPath(Path path, float pathOffset)
    {
        float currentParam;
        Vector3 futurePos = gameObject.transform.position + velocity * Time.fixedDeltaTime;
        currentParam = path.getParam(futurePos);
        float targetParam = currentParam + pathOffset;
        Vector3 targetPos = path.getPosition(targetParam);
        //Debug.DrawLine(transform.position, targetPos, Color.magenta, 999);
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

    bool Arrive(Vector3 target)
    {
        Vector3 direction = target - gameObject.transform.position;
        float distance = Vector3.Distance(transform.position, target);

        // If we are at target
        if (distance < radius)
        {
            angular = Vector3.zero;
            rotation = Vector3.zero;
            velocity = Vector3.zero;
            linear = Vector3.zero;
            //path = null;
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

    public void FindPathTo(int to)
    {
        path = AStarPathFinding.FindPath(FindClosestNode(), to);
        path.CalcParams();
        // Drawing path for debugging
        //path.DrawPath();
    }

    public void FindPathToRoom(int room)
    {
        int chosenNode = -1;
        //List<int> possibleNodes;
        switch (room)
        {
            case 0:
                //possibleNodes = new List<int>() {2,3,4};
                //chosenNode = possibleNodes[Random.Range(0, 3)];
                chosenNode = 27;
                break;
            case 1:
                //possibleNodes = new List<int>() { 14, 44 };
                //chosenNode = possibleNodes[Random.Range(0, 2)];
                chosenNode = 28;
                break;
            case 2:
                //possibleNodes = new List<int>() { 11, 12, 25, 26 };
                //chosenNode = possibleNodes[Random.Range(0, 4)];
                chosenNode = 29;
                break;
            case 3:
                //possibleNodes = new List<int>() { 14, 43 };
                //chosenNode = possibleNodes[Random.Range(0, 4)];
                chosenNode = 32;
                break;
            case 4:
                //possibleNodes = new List<int>() { 16, 45, 46, 47 };
                //chosenNode = possibleNodes[Random.Range(0, 4)];
                chosenNode = 31;
                break;
            case 5:
                //possibleNodes = new List<int>() { 16, 45, 46, 47 };
                //chosenNode = possibleNodes[Random.Range(0, 4)];
                chosenNode = 30;
                break;
        }
        path = AStarPathFinding.FindPath(FindClosestNode(), chosenNode);
        path.CalcParams();
        // Drawing path for debugging
        //path.DrawPath();
    }

    public int FindClosestNode()
    {
        int closestNode = -1;
        float closestDistance = float.MaxValue;
        foreach(Transform child in GameObject.Find("PathNodes").transform)
        {
            float distance = Vector3.Distance(transform.position, child.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                string name = child.name;
                closestNode = int.Parse(name.Substring(name.IndexOf("(") + 1, name.IndexOf(")") - name.IndexOf("(") - 1));
            }
        }
        //Debug.Log("Closest Node: " + closestNode);
        return closestNode;
    }

    public void ChasePlayer()
    {
        Arrive(player.transform.position);
    }

    public bool Patrol()
    {
        return FollowPath(path, pathOffset);
    }

}
