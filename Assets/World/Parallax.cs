using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] SpriteRenderer back1;
    [SerializeField] SpriteRenderer back2;
    [SerializeField] SpriteRenderer back3;
    [SerializeField] SpriteRenderer back4;
    [SerializeField] SpriteRenderer back5;
    float xPrevious;
    void Start()
    {
        
    }

    void Update()
    {
        float xCam = transform.position.x;
        float offset = xPrevious + xCam;
        xPrevious = xCam;

        back5.material.mainTextureOffset = new Vector2(offset / 64, 0);
        back4.material.mainTextureOffset = new Vector2(offset / 128, 0);
        back3.material.mainTextureOffset = new Vector2(offset / 256, 0);
        back2.material.mainTextureOffset = new Vector2(offset / 512, 0);
    }
}
