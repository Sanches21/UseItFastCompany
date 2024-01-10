using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 TargetPosition;
    private bool IsMoving;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            SetTargetPosition();
        }
        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, 1 * Time.deltaTime);

            if (Vector3.Distance(transform.position, TargetPosition) <= 0.05f)
            {

                IsMoving = false;
            }
        }

        
        
    }

    private void SetTargetPosition()
    {
        
        
        //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (hit.collider != null)
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            TargetPosition = hit.transform.position;
            if (Vector3.Distance(transform.position, TargetPosition) <= 1.3f)
            {
                IsMoving = true;
            }
            else
            {

            }
            
        }
    }

    
}
