using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RigidBodyCharacter : MonoBehaviour
{
    #region Variables
    private NavMeshAgent agent;
    public float speed = 5f;
    public float jumpHeight = 10f;
    private Rigidbody rigidbody;
    private Vector3 inputDirection = Vector3.zero;
    private bool isGrounded = false;
    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;
    public Camera cam;
    #endregion

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;

    }

    // Update is called once per frame
    void Update()
    {

        CheckGroundStatus();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
            {
                Debug.Log("We hit" + hit.collider.name + " " + hit.point);
                agent.SetDestination(hit.point);
            }
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                rigidbody.MovePosition(agent.velocity * Time.deltaTime);
            }
            else
            {
                rigidbody.MovePosition(Vector3.zero);
            }
        }

        transform.position = agent.nextPosition;
    }

    #region Helper Methods
    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f),
            Vector3.down, out hitInfo, groundCheckDistance, groundLayerMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;

        }
    }
    #endregion
}
