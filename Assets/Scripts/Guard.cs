using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public float maxSpeed = 1;
    public float radius = 1;
    public float slowRadius = 2;
    //public float predictTime = 0.1f;
    public float pathOffset = 0.1f;
    public Vector3 velocity;
    public Vector3 rotation;
    public Vector3 linear;
    public Vector3 angular;
    public float maxAcceleration = 1;
    public float maxAngularAcceleration = 1;
    public float maxRotation = 180;
    private Path path;
    // Start is called before the first frame update
    void Start()
    {
        AStarPathFinding a = new AStarPathFinding();
        path = a.FindPath(0, 16);
        path.CalcParams();
        // Drawing path for debugging
        path.DrawPath();

    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = gameObject.transform.position + velocity * Time.fixedDeltaTime;
        gameObject.transform.localEulerAngles = rotation * Time.fixedDeltaTime;

        velocity += linear * Time.fixedDeltaTime;
        rotation += angular * Time.fixedDeltaTime;

        if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        FollowPath(path, pathOffset);
    }

    void FollowPath(Path path, float pathOffset)
    {
        float currentParam;
        Vector3 futurePos = gameObject.transform.position + velocity * Time.fixedDeltaTime;
        currentParam = path.getParam(futurePos);
        float targetParam = currentParam + pathOffset;
        Vector3 targetPos = path.getPosition(targetParam);
        Seek(targetPos);
    }

    void Seek(Vector3 target)
    {
        Vector3 linearCalc = target - gameObject.transform.position;

        linearCalc.Normalize();
        linearCalc *= maxSpeed;
        linear = linearCalc;
    }

    void Align(Transform target)
    {
        Vector3 rotationCalc = target.localEulerAngles - gameObject.transform.localEulerAngles;
        while(rotationCalc.y > 180)
        {
            rotationCalc.y -= 180;
        }
        while(rotationCalc.y < -180)
        {
            rotationCalc.y += 180;
        }
        float rotationSize = Mathf.Abs(rotationCalc.y);
        Vector3 targetRotation;
        if(!(rotationSize < radius))
        {
            if(rotationSize > slowRadius)
            {
                targetRotation = new Vector3(0, maxRotation, 0);
            }
            else
            {
                targetRotation = new Vector3(0, maxRotation, 0) * rotationSize / slowRadius;
            }

            targetRotation *= rotationCalc.y / rotationSize;

            Vector3 angularCalc = targetRotation - gameObject.transform.localEulerAngles;

            float angularAcceleration = Mathf.Abs(angularCalc.y);
            if(angularAcceleration > maxAngularAcceleration)
            {
                angularCalc /= angularAcceleration;
                angularCalc *= maxAngularAcceleration;
            }

            angular = angularCalc;
        }
    }

    void Arrive(Transform target)
    {
        Vector3 direction = target.position - gameObject.transform.position;
        float distance = direction.magnitude;
        if(!(distance < radius))
        {
            float targetSpeed;
            if(distance > slowRadius)
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
            
            if(linearCalc.magnitude > maxAcceleration)
            {
                linearCalc.Normalize();
                linearCalc *= maxAcceleration;
            }

            linear = linearCalc;
        }
    }
    
    private float GetNewOrientation(float currentOrientation, Vector3 velocity)
    {
        if(velocity.magnitude > 0)
        {
            return Mathf.Atan2(-velocity.x, -velocity.z);
        }
        return currentOrientation;
    }
}
