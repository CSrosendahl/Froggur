using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleFriend : MonoBehaviour
{
    public static TurtleFriend instance;
    private void Awake()
    {
        instance = this;
    }

    public Animator turtleAnim;

    public void PlayAnimation(string animationName)
    {
        if (turtleAnim != null)
        {
            turtleAnim.Play(animationName);
        }
        else
        {
            Debug.LogError("Animator component is not assigned.");
        }
    }


    private void OnMouseDown()
    {
        PlayAnimation("HappyJump"); // Replace "YourAnimationName" with the name of your animation
    }

    private void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch phase is Began (similar to mouse click)
            if (touch.phase == TouchPhase.Began)
            {
                // Convert touch position to a Ray
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Check if the ray hits this object's collider
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        PlayAnimation("HappyJump"); // Replace "YourAnimationName" with the name of your animation
                    }
                }
            }
        }
    }

}
