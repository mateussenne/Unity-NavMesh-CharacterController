using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    /*ATENTION: You need to add both de CharacterController and NavMesh Agent components to your player object in order for this code to work.
    /File developed by Mateus Senne, follow me at Twitter to keep up with the stuff that I'm doing @MyGameDevPath

    */

    //Components variables
    private CharacterController _character;
    private NavMeshAgent _agent;

    //Speed variable
    [SerializeField]
    private float _speed = 5f;

    //Jump size variable
    [SerializeField]
    private float _jump = 16f;

    //Gravity force variable
    [SerializeField]
    private float _gravity = 1f;

    //This is the control variable to your physics based character, DO NOT delete this
    private float _yVelocity;

    //We get the both components and assign to it's respective variables
    void Start()
    {
        _character = GetComponent<CharacterController>();
        if (_character == null)
        {
            Debug.LogError("Character controller is missing");

        }

        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            Debug.LogError("NavMesh Agent is missing");
        }
        
    }


    //Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }


    //Movment Method 
    void CalculateMovement()
    {
        //This was done with the GetAxis Inputs which can be found in you project configuration. Feel free to change this as necessary
        float zInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(xInput, 0, zInput);
        Vector3 velocity = direction * _speed;

        //Physics based jump and gravity
        if (_character.isGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                _yVelocity = _jump;
            }

        }

        else
        {
            _yVelocity -= _gravity;
        }


        velocity.y = _yVelocity; 
        velocity = transform.TransformDirection(velocity); //Workaround to set local space to world space to fix inverse movment keys
        _character.Move(velocity * Time.deltaTime);

        //Player Bounds, the magic happens here. We get the player transform and set it's position to the NavMesh Agent's next position.
        //This will limit where our character is moving, by clamping the player object to the NavMesh bake, as the Agent cannot leave it.
        Vector3 playerBounds = new Vector3(_agent.nextPosition.x, transform.position.y, _agent.nextPosition.z);
        transform.position = playerBounds;

    }

    
}
