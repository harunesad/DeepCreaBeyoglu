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
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3, item))
        {
            uIManager.InteractUpdate(true, "Collect");
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.transform.gameObject.layer == 6)
                {
                    if (hit.transform.CompareTag("Coin"))
                    {
                        hit.transform.gameObject.SetActive(false);
                    }
                    else
                    {
                        hit.transform.gameObject.SetActive(false);
                        uIManager.NextLevel();
                    }
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
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 3, Color.red);
    }
    void FixedUpdate()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);

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
        //transform.Rotate(Vector3.up *  Input.GetAxisRaw("Horizontal"));
    }
}
