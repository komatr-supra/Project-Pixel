using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//really easy camera controller
namespace ProjectPixel.System
{
    public class SimpleCameraController : MonoBehaviour
    {
        float pixel = 1 / 32f;
        [SerializeField] Transform follow;
        [SerializeField] Vector2 offset;
        void LateUpdate()
        {
            transform.position = new Vector3(follow.position.x + offset.x * pixel, follow.position.y + offset.y * pixel, -10);
        }
    }
}