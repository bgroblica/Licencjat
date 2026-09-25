using DG.Tweening;
using TMPro;
using UnityEngine;

public class TrapdoorInt : MonoBehaviour, IInteractable
{
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private Vector3 targetRotation1 = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 targetRotation2 = new Vector3(0, 0, 0);

    [SerializeField] private GameObject trapdoor;

    private bool isOpen = false;

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (isOpen)
        {
            trapdoor.transform.DORotate(targetRotation1, rotationSpeed);
        }
        else
        {
            trapdoor.transform.DORotate(targetRotation2, rotationSpeed);
        }

        isOpen = !isOpen;

        return true;
    }
}
