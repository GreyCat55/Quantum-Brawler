using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AttackType { lowPunch = 0, highPunch = 1, elbow = 2 };

public class FightingCombo : MonoBehaviour {

    [Header("Inputs")]
    public KeyCode lowPunchKey;
    public KeyCode highPunchKey;
    public KeyCode elbowKey;

    [Header("Attacks")]
    public Attack lowPunch;
    public Attack highPunch;
    public Attack elbow;
    public List<Combo> combos;
    public float comboLeeway = 0.2f;

    [Header("Components")]
    Animator anim;

    Attack currentAtk = null;
    ComboInput lastInput = null;
    List<int> currentCombos = new List<int>();

    float timer = 0;
    float leeway = 0;
    bool skip = false;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        PrimeCombos();
    }

    void PrimeCombos()
    {
        for (int i = 0; i < combos.Count; ++i)
        {
            Combo c = combos[i];
            c.onInput.AddListener(() =>
            {
                //Call Attack function with combo's attack
                skip = true;
                Attack(c.comboAttack);
                ResetCombos();

            });
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (currentAtk != null)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                currentAtk = null;
            }
            return;
        }

        if (currentCombos.Count > 0)
        {
            leeway += Time.deltaTime;
            if(leeway >= comboLeeway)
            {
                if (lastInput != null)
                {
                    Attack(getAttackFromType(lastInput.type));
                    lastInput = null;
                }

                ResetCombos();
            }
        }
        else
        {
            leeway = 0;
        }

            ComboInput input = null;
        if (Input.GetKeyDown(lowPunchKey) || Input.GetButtonDown("Fire1"))
        {
            input = new ComboInput(AttackType.lowPunch);
        }
        if (Input.GetKeyDown(highPunchKey) || Input.GetButtonDown("Fire2"))
        {
            input = new ComboInput(AttackType.highPunch);
        }
        if (Input.GetKeyDown(elbowKey))
        {
            input = new ComboInput(AttackType.elbow);
        }

        if(input == null)
        {
            return;
        }

        lastInput = input;


        List<int> remove = new List<int>();
        for (int i = 0; i < currentCombos.Count; ++i)
        {
            Combo c = combos[currentCombos[i]];
            if (c.continueCombo(input))
            {
                leeway = 0;
            }
            else
            {
                remove.Add(i);
            }
        }

        if (skip)
        {
            skip = false;
            return;
        }

        for (int i = 0; i < combos.Count; ++i)
        {
            if (currentCombos.Contains(i))
            {
                continue;
            }
            if (combos[i].continueCombo(input))
            {
                currentCombos.Add(i);
                leeway = 0;
            }
        }

        foreach(int i in remove)
        {
            currentCombos.RemoveAt(i);
        }

        if (currentCombos.Count <= 0)
        {
            Attack(getAttackFromType(input.type));
        }
    }

    void ResetCombos()
    {
        leeway = 0;
        for (int i = 0; i < currentCombos.Count; ++i)
        {
            Combo c = combos[currentCombos[i]];
            c.resetCombo();
        }

        currentCombos.Clear();
    }

    void Attack(Attack attack)
    {
        currentAtk = attack;
        timer = attack.animLen;
        anim.Play(attack.animName, -1, 0);
    }

    Attack getAttackFromType(AttackType t)
    {
        if (t == AttackType.lowPunch)
        {
            return lowPunch;
        }
        if (t == AttackType.highPunch)
        {
            return highPunch;
        }
        if (t == AttackType.elbow)
        {
            return elbow;
        }
        return null;

    }

}

[System.Serializable]
public class Attack
{
    public string animName;
    public float animLen;
}

[System.Serializable]
public class ComboInput
{
    //Note: Add movement type enum for more precise combos
    public AttackType type;

    public ComboInput(AttackType t)
    {
        type = t;
    }

    public bool isSameAs(ComboInput i)
    {
        return (type == i.type);
    }
}

[System.Serializable]
public class Combo
{
    public string name;
    public List<ComboInput> inputs;
    public Attack comboAttack;
    public UnityEvent onInput;
    int currentIn = 0;

    public bool continueCombo(ComboInput i)
    {
        if (inputs[currentIn].isSameAs(i))
        {
            ++currentIn;
            if (currentIn >= inputs.Count)
            {
                onInput.Invoke();
                currentIn = 0;
            }
            return true;
        }
        else
        {
            currentIn = 0;
            return false;
        }
    }

    public ComboInput currentComboInput()
    {
        if(currentIn >= inputs.Count)
        {
            return null;
        }
        return inputs[currentIn];
    }

    public void resetCombo()
    {
        currentIn = 0;
    }
}
