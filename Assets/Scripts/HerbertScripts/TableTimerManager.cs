
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableTimerManager : MonoBehaviour
{
    [SerializeField] private List<Table> tableList;

    [Header("Timer Settings")]
    [SerializeField] private float timerDuration = 20f;
    [SerializeField] private float timerDecreaseAmount = 2f;
    [SerializeField] private float minimumTimerDuration = 5f;

    [Header("Difficulty Settings")]
    [SerializeField] private int roundsBeforeDecrease = 5;
    [SerializeField] private int tablesAtStart = 3;

    private int roundCounter = 0;

    private void Start()
    {
        StartCoroutine(TimerRoutine());
        ChooseInitialTables();
    }

    IEnumerator TimerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timerDuration);

            AffectRandomObject();

            roundCounter++;

            if (roundCounter >= roundsBeforeDecrease)
            {
                roundCounter = 0;

                timerDuration = Mathf.Max(
                    timerDuration - timerDecreaseAmount,
                    minimumTimerDuration
                );

                Debug.Log($"Timer decreased! New duration: {timerDuration} seconds.");
            }
        }
    }

    void AffectRandomObject()
    {
        List<Table> availableTables = new List<Table>();

        foreach (Table table in tableList)
        {
            if (table != null && !table.IsEating && !table.IsWaiting)
            {
                availableTables.Add(table);
            }
        }

        if (availableTables.Count == 0)
        {
            Debug.Log("No available tables!");
            return;
        }

        int randomIndex = Random.Range(0, availableTables.Count);

        Table selectedTable = availableTables[randomIndex];

        Debug.Log("Affected: " + selectedTable.name);

        selectedTable.ChooseDish();
    }
    void ChooseInitialTables()
    {
        List<Table> availableTables = new List<Table>(tableList);
    
        availableTables.RemoveAll(table => table == null);
    
        int count = Mathf.Min(tablesAtStart, availableTables.Count);
    
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableTables.Count);
    
            Table selectedTable = availableTables[randomIndex];
    
            Debug.Log("Initial table selected: " + selectedTable.name);
    
            selectedTable.ChooseDish();
    
            availableTables.RemoveAt(randomIndex);
        }
    }
}
