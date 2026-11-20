using UnityEngine;
using UnityEngine.UI;

public class StressBar : MonoBehaviour
{
    [SerializeField] private float stressMult = 0.005f;
    [SerializeField] private float currentStress;
    [SerializeField] private float maxStress = 1f;

    [SerializeField] private Image fill;

    private bool isMaxStress;

    private void Awake()
    {
        currentStress = maxStress;
        isMaxStress = false;
    }
    private void Update()
    {
        if (isMaxStress == false)
        {
            currentStress -= Time.deltaTime * stressMult;
            GetCurrentFill();
        }

        if (currentStress <= 0)
        {
            isMaxStress = true;
        }
    }
    public void AddStress(float stressToAdd)
    {
        currentStress -= stressToAdd * stressMult;
    }

    public void GetCurrentFill()
    {
        fill.fillAmount = currentStress;
    }
}
