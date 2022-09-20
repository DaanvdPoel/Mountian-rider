using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour // Sten
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float rotationSpeed;

    private Rigidbody2D rigidbody;

    private Transform ray_point;

    private float yRotate = 0; // Needed to make the player continue rotating

    private void Start()
    {
        rigidbody = this.transform.GetComponent<Rigidbody2D>();
        ray_point = this.transform.Find("Ground Part");
    }


    void Update()
    {
        Move();
        Jump();
        RotatePlayer();

        // Ups the speed of palyer to make it harder over time
        speed += Time.deltaTime / 100;

        // Sets the distance score to x position
        GameManager.instance.SetDistance((int)this.transform.position.x);
    }

    // Sends a raycast to the ray_point for checking if the player is on the ground and can jump
    bool IsOnFloor()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.transform.position, ray_point.position - this.transform.position, 1);

        // Checks all hits to check for a floor
        for (int i = 0; i < hits.Length; i++)
            if (hits[i].transform.GetComponent<Floor>())
                return true;

        return false;
    }

    void Move()
    {
        if (rigidbody.velocity.x > speed) // Stops the player from going over the maximum speed limit
        {
            Vector3 oldVelocity = rigidbody.velocity;
            rigidbody.velocity = new Vector2(speed, oldVelocity.y);
        }
        else if (rigidbody.velocity.x < speed) // Moves the player forward if speed is not correct 
        {
            rigidbody.AddForce(new Vector3(speed, 0));
        }
    }

    IEnumerator Spin()
    {
        int used = 0;
        float rotation = 0;

        yield return new WaitForSeconds(0.1f);

        while (IsOnFloor() == false)
        {


            if (Input.GetKey(KeyCode.LeftArrow) && (used == 0 || used == 2))
            {
                used = 1;
                rotation = this.transform.rotation.z + 25;
            }

            if (Input.GetKey(KeyCode.RightArrow) && (used == 0 || used == 1))
            {
                used = 2;
                rotation = this.transform.rotation.z - 25;
            }

            if (used != 0 && this.transform.rotation.z > rotation && this.transform.rotation.z < rotation)
                print("DAMN");



            yield return new WaitForSeconds(0.1f);
        }
    }

    // Checks if the player can jump & if he does, he jumps
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsOnFloor() == true)
        {
            rigidbody.AddForce(new Vector2(0, jumpPower * 10)); // Will push the player into the air with jumppower * 10
            StartCoroutine(Spin());
        }
    }

    // Allowes the player to rotate with arrow keys
    void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            yRotate = yRotate + Time.deltaTime * (rotationSpeed / 50); // Rotate left
        else if (Input.GetKey(KeyCode.RightArrow))
            yRotate = yRotate - Time.deltaTime * (rotationSpeed / 50); // Rotate right

        if (IsOnFloor() == true) // Stops the player from rotating on the ground
        {
            yRotate = 0;
            return;
        }
        else if (yRotate > 1) // makes it stay in boundry
            yRotate = 1;
        else if (yRotate < -1) // makes it stay in boundry
            yRotate = -1;

        // Stops the player from rotating into the wrong direction which makes it harder to rotate the opposite way
        rigidbody.angularVelocity = 0;

        this.transform.Rotate(0, 0, yRotate * rotationSpeed * Time.deltaTime); // rotates the player
    }
}
