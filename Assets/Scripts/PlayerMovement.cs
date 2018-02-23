using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    Rigidbody2D rbody;
    Animator animator;
	// Use this for initialization
	void Start () {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        if(movement!=Vector2.zero) {
            animator.SetBool("iswalking", true);
            animator.SetFloat("input_x", movement.x);
            animator.SetFloat("input_y", movement.y);
        } else {
            animator.SetBool("iswalking", false);
        }
        
        rbody.MovePosition(rbody.position + movement * Time.deltaTime);
	}

    public void teleportTo(Vector3 pos)
    {
        transform.position = pos + new Vector3(0f, 0.2f, 0f);
    }
}
