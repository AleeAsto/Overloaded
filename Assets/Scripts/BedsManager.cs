using UnityEngine;

public class BedsManager : MonoBehaviour
{
    [Header("UI de Tareas")]
    public MonoBehaviour tareasUI;       
    public int tareaIndex = -1;          
    public string tareaTexto = "Tender Cama";

    [Header("Estado (solo lectura)")]
    [SerializeField] private int totalCamas = 0;
    [SerializeField] private int bedMade = 0;
    [SerializeField] private bool tareaCompletada = false;

    public void RegisterBed(Beds w)
    {
        totalCamas++;
        if (tareaCompletada && bedMade < totalCamas)
            tareaCompletada = false;
    }

    public void OnBedMade(Beds w)
    {
        bedMade++;
        CheckEstado();
    }

    void CheckEstado()
    {
        if (!tareaCompletada && totalCamas > 0 && bedMade >= totalCamas)
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

    // opcional HUD
    public int GetTotal() => totalCamas;
    public int GetRemaining() => Mathf.Max(0, totalCamas - bedMade);
}
