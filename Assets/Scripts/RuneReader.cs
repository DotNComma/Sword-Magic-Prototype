using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneReader : MonoBehaviour
{
    [Header("Rune Buttons")]
    private InputAction rune1Action;
    private InputAction rune2Action;
    private InputAction rune3Action;

    [Header("Rune List Settings")]
    private float inputTimeout = 2f;
    private float lastInputTime;

    public List<string> activeRunes = new List<string>();

    private void Start()
    {
        rune1Action = InputSystem.actions.FindAction("Rune1");
        rune2Action = InputSystem.actions.FindAction("Rune2");
        rune3Action = InputSystem.actions.FindAction("Rune3");
    }

    private void Update()
    {
        if(activeRunes.Count > 0 && Time.time - lastInputTime > inputTimeout)
        {
            activeRunes.Clear();
        }

        if(rune1Action.WasPressedThisFrame())
        {
            AddRune("Z");
        }

        if(rune2Action.WasPressedThisFrame())
        {
            AddRune("X");
        }

        if(rune3Action.WasPressedThisFrame())
        {
            AddRune("V");
        }
    }

    private void AddRune(string rune)
    {
        activeRunes.Add(rune);
        lastInputTime = Time.time;

        CheckForSpells();
    }

    private void CheckForSpells()
    {
        string currentRuneString = string.Join("", activeRunes);
        
        if(currentRuneString == "ZZX")
        {
            Cast("Fireball");
        }
        else if(currentRuneString == "ZXV")
        {
            Cast("Lightning Strike");
        }
        else if(currentRuneString == "ZVV")
        {
            Cast("Ice Shard");
        }
    }

    private void Cast(string spellName)
    {
        Debug.Log($"Casting {spellName}");
        activeRunes.Clear();
    }
}
