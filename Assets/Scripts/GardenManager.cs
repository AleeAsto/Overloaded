using System;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    [Header("UI de Tareas")]
    public MonoBehaviour tareasUI;       // componente que tiene CompletarTarea / CompletarTareaPorTexto
    public int tareaIndex = -1;          // usa índice si >=0
    public string tareaTexto = "Regar las plantas";

    [Header("Estado (solo lectura)")]
    [SerializeField] private int totalPlants = 0;
    [SerializeField] private int completedPlants = 0;
    [SerializeField] private bool tareaCompletada = false;

    public void RegisterPlant(Plant p)
    {
        totalPlants++;
        // si ya estaba completa y aparece otra planta, podrías reabrir la tarea si lo deseas
        if (tareaCompletada && completedPlants < totalPlants)
            tareaCompletada = false;
    }

    public void OnPlantCompleted(Plant p)
    {
        completedPlants++;
        CheckEstado();
    }

    void CheckEstado()
    {
        if (!tareaCompletada && totalPlants > 0 && completedPlants >= totalPlants)
        {
            tareaCompletada = true;
            TryMarcarUI();
        }
    }

    void TryMarcarUI()
    {
        if (tareasUI == null) return;
        var type = tareasUI.GetType();

        if (tareaIndex >= 0)
        {
            var m = type.GetMethod("CompletarTarea");
            if (m != null) m.Invoke(tareasUI, new object[] { tareaIndex });
        }
        else if (!string.IsNullOrEmpty(tareaTexto))
        {
            var m2 = type.GetMethod("CompletarTareaPorTexto");
            if (m2 != null) m2.Invoke(tareasUI, new object[] { tareaTexto });
        }
    }

    // opcional: para HUD de progreso
    public int GetTotal() => totalPlants;
    public int GetRemaining() => Mathf.Max(0, totalPlants - completedPlants);
}
