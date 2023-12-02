using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivotPoint;
    [SerializeField] private float timeDelay = 0.1f;
    [SerializeField] private float respawnDelay = 2f;

    private Rigidbody2D currentBallRigidBody;
    private SpringJoint2D currentBallSpringJoint;

    private bool _isDragging;

    void Start()
    {
        SpawnBall();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (currentBallRigidBody==null) { return; }
        /*        if (!Touchscreen.current.primaryTouch.press.isPressed)
                {
                    if (_isDragging)
                    {
                        LaunchBall();
                    }
                    _isDragging = false;

                    return;
                }*/

        /*currentBallRigidBody.isKinematic = true;

        _isDragging = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector2 screenToWorldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        currentBallRigidBody.position = screenToWorldPosition;*/



        if ((Touch.activeTouches.Count==0))
        {
            if (_isDragging)
            {
                LaunchBall();
            }
            _isDragging = false;

            return;
        }//multi touch

        Vector2 touchPosition = new Vector2();
        foreach (Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition;
        }
        touchPosition /= Touch.activeTouches.Count;

        currentBallRigidBody.isKinematic = true;

        _isDragging = true;

        Vector2 screenToWorldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        currentBallRigidBody.position = screenToWorldPosition;


    }

    private void SpawnBall()
    {
        GameObject ballInstance =Instantiate(ballPrefab,pivotPoint.position,Quaternion.identity);
        currentBallRigidBody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivotPoint;
    }

    private void LaunchBall()
    {
        currentBallRigidBody.isKinematic = false;
        currentBallRigidBody = null;

        Invoke(nameof(DetachBall), timeDelay);
        

    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnBall), respawnDelay);
    }
}
