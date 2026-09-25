using DG.Tweening;
using TMPro;
using UnityEngine;

public class DoubleDoorInt : MonoBehaviour, IInteractable
{
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private Vector3 targetRotationLeft1 = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 targetRotationLeft2 = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 targetRotationRight1 = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 targetRotationRight2 = new Vector3(0, 0, 0);

    [SerializeField] private GameObject doorleft;
    [SerializeField] private GameObject doorright;

    private bool isOpen = false;

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (isOpen)
        {
            doorleft.transform.DORotate(targetRotationLeft1, rotationSpeed);
            doorright.transform.DORotate(targetRotationRight1, rotationSpeed);
        }
        else
        {
            doorleft.transform.DORotate(targetRotationLeft2, rotationSpeed);
            doorright.transform.DORotate(targetRotationRight2, rotationSpeed);
        }

        isOpen = !isOpen;

        return true;
    }
}
