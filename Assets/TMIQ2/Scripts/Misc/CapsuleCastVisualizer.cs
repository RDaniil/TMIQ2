using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCastVisualizer : MonoBehaviour
{
    public Material mat;
    void RenderVolume(Vector3 p1, Vector3 p2, float radius, Vector3 dir, float distance)
    {
        if (!shape)
        { // if shape doesn't exist yet, create it
            shape = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            Destroy(shape.GetComponent<Collider>()); // no collider, please!
            shape.GetComponent<Renderer>().material = mat; // assign the selected material to it
        }
        Vector3 scale; // calculate desired scale
        float diam = 2 * radius; // calculate capsule diameter
        scale.x = diam; // width = capsule diameter
        scale.y = Vector3.Distance(p2, p1) + diam; // capsule height
        scale.z = distance + diam; // volume length
        shape.localScale = scale; // set the rectangular volume size
                                  // set volume position and rotation
        shape.position = (p1 + p2 + dir.normalized * distance) / 2;
        shape.rotation = Quaternion.LookRotation(dir, p2 - p1);
        shape.GetComponent<Renderer>().enabled = true; // show it
    }

    void HideVolume()
    { // hide the volume
        if (shape) shape.GetComponent<Renderer>().enabled = false;
    }
    private Transform shape;
    public float range = 10; // range of the capsule cast
    private float freeDistance = 0;
    void Update()
    {
        if (Input.GetKey("p"))
        { // while P pressed...
            RaycastHit hit;
            CharacterController charContr = GetComponent<CharacterController>();
            var radius = charContr.radius;
            // find centers of the top/bottom hemispheres
            Vector3 p1 = transform.position + charContr.center;
            var p2 = p1;
            var h = charContr.height / 2 - radius;
            p2.y += h;
            p1.y -= h;
            // draw CapsuleCast volume:
            RenderVolume(p1, p2, radius, transform.forward, range);
            // cast character controller shape range meters forward:
            if (Physics.CapsuleCast(p1, p2, radius, transform.forward, out hit, range))
            {
                // if some obstacle inside range, save its distance 
                freeDistance = hit.distance;
            }
            else
            {
                // otherwise shows that the way is clear up to range distance
                freeDistance = range;
            }
        }
        if (Input.GetKeyUp("p"))
        {
            HideVolume(); // hide volume when P is released
        }
    }
}
