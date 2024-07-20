using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] LayerMask item;
    [SerializeField] UIManager uIManager;
    [SerializeField] Shake shake;
    RaycastHit hit;
    void Start()
    {

    }
    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 10, out hit, 100, item))
        {
            uIManager.InteractUpdate(true, "Collect");
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.transform.gameObject.layer == 6)
                {

                }
                else if (hit.transform.gameObject.layer == 7)
                {
                    uIManager.time -= 10;
                    shake.ShakeCamera(1, .5f);
                }
            }
        }
        else
        {
            uIManager.InteractUpdate(false, "Collect");
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);
    }
    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            MovePlayer();
        }
    }
    void MovePlayer()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement = cameraForward * Input.GetAxisRaw("Vertical") + cameraRight * Input.GetAxisRaw("Horizontal");
        movement.Normalize();

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
