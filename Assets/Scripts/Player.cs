using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Commented out lines are those added during the tutorial
//uncomment them for the final script
public class Platformer : MonoBehaviour {
    public bool grounded;

    public float walkForce = 10;
    public float jumpForce = 20;
    Rigidbody2D rigid;
    

    private void Start() {
        rigid = GetComponent<Rigidbody2D>();
        
    }

    private void Update() {
        if (grounded) {
            if (Input.GetButtonDown("Jump")) {
                Jump();
            }
        }
    }

    private void FixedUpdate() {
        grounded = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down, 0.05f);

        if (grounded) {
            Walk(Input.GetAxisRaw("Horizontal"));
        }
    }

    private void Walk(float walkInput) {
        rigid.AddForce(Vector2.right * walkInput * walkForce);
        //Animation code;
        // if (Mathf.Abs(walkInput) > 0.1)
        //     transform.GetChild(0).localScale = new Vector3(Mathf.Sign(walkInput), 1, 1);
    }

    private void Jump() {
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);


    }
    // Визуализация точки проверки земли в редакторе
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
