using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float castDistance = 5f;
    [SerializeField] private Vector3 rayCastOffset = new Vector3(0, 1f, 0);

    private InputAction interactInput;
    private void Start()
    {
        interactInput = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (interactInput.WasPressedThisFrame())
        {
            if(DoInteractionTest(out IInteractable interactable))
            {
                if (interactable.CanInteract())
                {
                    interactable.Interact(this);
                }
            }
        }
    }
    private bool DoInteractionTest(out IInteractable interactable)
    {
        interactable = null;

        Ray ray = new Ray(transform.position + rayCastOffset, transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hitinfo, castDistance))
        {
            interactable = hitinfo.collider.GetComponent<IInteractable>();

            if(interactable != null)
            {
                return true;
            }

            return false;
        }

        return false;
    }
}
