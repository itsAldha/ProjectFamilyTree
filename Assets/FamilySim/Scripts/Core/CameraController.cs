using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FamilySim.Core
{
    public class CameraController : MonoBehaviour
    {

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit && hit.transform.GetComponent<Person>())
                {
                    hit.transform.position += new Vector3(Input.GetAxis("Mouse X") * 0.25f, Input.GetAxis("Mouse Y") * 0.25f, 0);
                }
            }

            if (Input.GetMouseButton(1))
            {
                transform.position -= new Vector3(Input.GetAxis("Mouse X")*0.5f, Input.GetAxis("Mouse Y") * 0.5f, 0);
            }
        }
    }
}
