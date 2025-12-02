using UnityEngine;
using TMPro;

public class DotProductDemo : MonoBehaviour
{
    public Transform player;
    public float fieldOfView = 45f;
    public bool inFOV;
    public bool wall;
    public TextMeshProUGUI textMeshPro;

    void Update()
    {
        CheckIfPlayerIsInFront();
        CheckFieldOfView();

        wall = false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, directionToPlayer);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                wall = true;
            }
        }

        if (!wall && inFOV)
        {
            LookAtPlayer();
        }
    }

    // ---------------------------------------------
    // 1. Check if the player is in front of the object
    // ---------------------------------------------
    void CheckIfPlayerIsInFront()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, toPlayer);
        float dotRight = Vector3.Dot(transform.right, toPlayer);

        if (dot > 0.5)
        {
            Debug.Log("Player is IN FRONT of this object.");
            textMeshPro.text = "Player is IN FRONT of this object.";

        }
        else if (dot < -0.5) 
        {
            Debug.Log("Player is BEHIND this object.");
            textMeshPro.text = "Player is BEHIND this object.";
        }
        else if(dotRight > 0)
        {
            Debug.Log("Player is to the LEFT of this object.");
            textMeshPro.text = "Player is to the LEFT of this object.";
        }
        else if(dotRight < 0)
        {
            Debug.Log("Player is to the LEFT of this object.");
            textMeshPro.text = "Player is to the LEFT of this object.";
        }
            
        Debug.Log(HitFromFront(toPlayer));
    }

    // ---------------------------------------------
    // 2. Field of View (FOV) Detection
    // ---------------------------------------------
    void CheckFieldOfView()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, toPlayer);

        float threshold = Mathf.Cos(fieldOfView * Mathf.Deg2Rad);

        if (dot > threshold)
        {
            inFOV = true;
            Debug.Log("Player is INSIDE the field of view.");
        }
        else
        {
            inFOV = false;
            Debug.Log("Player is OUTSIDE the field of view.");
        }
            
    }

    // ---------------------------------------------
    // 3. Hit direction detection
    // ---------------------------------------------
    public bool HitFromFront(Vector3 hitDirection)
    {
        hitDirection.Normalize();
        float dot = Vector3.Dot(transform.forward, hitDirection);
        return dot > 0;
    }

    // ---------------------------------------------
    // (Bonus) Draw Gizmos for Visualization
    // ---------------------------------------------
    void OnDrawGizmos()
    {
        if (player == null) return;

        Vector3 start = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(start, direction, out RaycastHit hit))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(start, hit.point);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(start, start + direction);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position);

        // FOV cone lines
        Gizmos.color = Color.yellow;
        Quaternion leftRot = Quaternion.Euler(0, -fieldOfView, 0);
        Quaternion rightRot = Quaternion.Euler(0, fieldOfView, 0);

        Gizmos.DrawLine(transform.position, transform.position + leftRot * transform.forward * 3);
        Gizmos.DrawLine(transform.position, transform.position + rightRot * transform.forward * 3);
    }

    void LookAtPlayer()
    {
        if(inFOV)
            transform.LookAt(player.position);
    }
}
