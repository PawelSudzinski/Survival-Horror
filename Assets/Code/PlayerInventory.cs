using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int batteryCount = 0; // Number of batteries in inventory
    private List<string> keys = new List<string>();
    public int fuseCount = 0; // Liczba posiadanych bezpieczników

    // Add a battery to the inventory
    public void AddBattery()
    {
        batteryCount++;
        Debug.Log("Battery added! Current count: " + batteryCount);
    }

    // Use a battery from the inventory
    public int GetBatteryCount()
    {
        return batteryCount;
    }
    public bool ConsumeBattery()
    {
        if (batteryCount > 0)
        {
            batteryCount--;
            return true;
        }
        return false;
    }
    public void AddKey(string keyName)
    {
        keys.Add(keyName);
        Debug.Log($"Key added: {keyName}");
    }
    public bool HasKey(string keyName)
    {
        return keys.Contains(keyName);
    }
    public bool HasFuses(int amount)
    {
        return fuseCount >= amount;
    }

    public void UseFuses(int amount)
    {
        if (HasFuses(amount))
        {
            fuseCount -= amount;
        }
    }

    public void AddFuse(int amount)
    {
        fuseCount += amount;
        Debug.Log("Dodano bezpiecznik. Liczba bezpieczników: " + fuseCount);
    }
    public bool HasEnoughFuses(int count)
    {
        return fuseCount >= count;
    }
    void OnGUI()
    {
        
            
            GUI.Label(new Rect(10, 50, 200, 20), "Fuses: " + fuseCount);
        }
    }

