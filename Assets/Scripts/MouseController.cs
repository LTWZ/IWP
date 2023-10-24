using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInteract
{
    void Interact();
}

public class MouseController : MonoBehaviour
{
    private Vector3 Direction;
    public event Action Fire;
    private float InteractionRange = 10f; // change it to increase/decrease interaction range

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            Fire?.Invoke();

        if (Input.GetKeyDown(KeyCode.E))
            UpdateInteraction();

        Direction = PlayerMovement.GetInstance().GetMousePosition() - PlayerMovement.GetInstance().transform.position;
        Direction.z = 0;

    }

    private void UpdateInteraction()
    {
        Vector2 pos = PlayerMovement.GetInstance().transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, InteractionRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider = colliders[i];
            if (collider.transform.GetComponent<IInteract>() != null)
            {
                collider.transform.GetComponent<IInteract>().Interact();
                Debug.Log("t");
            }
        }
    }

    public Vector3 GetDirection()
    {
        return Direction;
    }
}
