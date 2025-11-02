using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Energy Settings")]
    [SerializeField] private Image energyBar; // Imagen con Fill tipo Horizontal
    [SerializeField] private float maxEnergy = 100f;
    private float currentEnergy;

    [Header("Skills Settings")]
    [SerializeField] private Image[] skillCooldownImages; // 4 imágenes circulares (Fill radial)
    [SerializeField] private float[] cooldownDurations = { 5f, 8f, 10f, 12f }; // segundos por habilidad
    private bool[] isOnCooldown;

    private void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyUI();

        // Inicializar cooldowns (iconos visibles desde el inicio)
        isOnCooldown = new bool[skillCooldownImages.Length];
        foreach (var img in skillCooldownImages)
        {
            if (img != null)
                img.fillAmount = 1; // 0 = sin cooldown (icono visible)
        }
    }

    private void Update()
    {
        // Ejemplo: activar habilidades con teclas numéricas
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseSkill(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseSkill(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseSkill(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseSkill(3);
    }

    // ==========================
    // 🔋 MÉTODOS DE ENERGÍA
    // ==========================

    public void ChangeEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, maxEnergy);
        UpdateEnergyUI();
    }

    private void UpdateEnergyUI()
    {
        if (energyBar != null)
        {
            energyBar.fillAmount = currentEnergy / maxEnergy;
        }
    }

    // ==========================
    // ⚔️ MÉTODOS DE HABILIDADES
    // ==========================

    public void UseSkill(int index)
    {
        if (index < 0 || index >= skillCooldownImages.Length) return;
        if (isOnCooldown[index]) return;

        Debug.Log($"Usando habilidad {index + 1}");
        StartCoroutine(CooldownRoutine(index));
    }

    private IEnumerator CooldownRoutine(int index)
    {
        isOnCooldown[index] = true;
        float cooldown = cooldownDurations[index];
        float timer = 0f;

        // Llenamos el overlay de cooldown (de 0 → 1)
        skillCooldownImages[index].fillAmount = 1f;

        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            skillCooldownImages[index].fillAmount = 1f - (timer / cooldown);
            yield return null;
        }

        skillCooldownImages[index].fillAmount = 1f; // cooldown terminado
        isOnCooldown[index] = false;
    }

    // ==========================
    // ⚙️ MÉTODOS EXTERNOS
    // ==========================

    public void SetEnergy(float value)
    {
        currentEnergy = Mathf.Clamp(value, 0, maxEnergy);
        UpdateEnergyUI();
    }

    public void RecoverEnergy(float amount)
    {
        ChangeEnergy(amount);
    }
}
