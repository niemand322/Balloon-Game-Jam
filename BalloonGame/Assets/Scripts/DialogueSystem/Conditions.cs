using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Conditions : MonoBehaviour
{
    IDictionary<string, bool> dict = new Dictionary<string, bool>(); // The dictionary stores all bool values used to make the conditions, with their keys.
    public UIManager uIManager;

    public void SetValues(string message) // An action node passes the information in the form: "key1,true,key2,false". // The transmitted values are stored in the dictionary.
    {
        var tuple = KeysValues(message);
        string[] keys = tuple.Item1;
        bool[] values = tuple.Item2;

        for (int i = 0; i < values.Length; i++) // Fills the Dictionary
        {
            SaveValueInDictionary(keys[i], values[i]);
        }
    }

    public void PushValue(string message) // An action node passes the information in the form: "key,text_index"
    {
        // The answer option with the index text_index is activated if and only if the value of the key is true.
        var keyIndex = IndexKey(message);

        if (dict.ContainsKey(keyIndex.Item2))
        {
            uIManager.FeedBools((keyIndex.Item1, dict[keyIndex.Item2]));
        }
        else
        {
            uIManager.FeedBools((keyIndex.Item1, false));
        }
        
    }

    public void ConditionAND(string message) // An action node passes the information in the form: "newkey,key1,true,key2,false"
    {
        // This function takes keys and the expected bool values and calculates true only if all keys are included in the dictionary and have the expected bool value. Otherwise it calculates false.
        // The calculated value will be saved in the dictionary with the newkey.

        var tuple = NewKeyKeysValues(message);
        string newKey = tuple.Item1;
        string[] keys = tuple.Item2;
        bool[] values = tuple.Item3;

        bool condition = true;

        for (int i = 0; i < keys.Length; i++)
        {
            if (dict.ContainsKey(keys[i]))
            {
                if (dict[keys[i]] != values[i])
                {
                    condition = false;
                    break;
                }
            }
            else
            {
                // Maybe one should not output "false" if the value of a key that does not exist should have had the default value "false" anyway?
                condition = false;
                break;
            }
            
        }

        SaveValueInDictionary(newKey, condition);
    }

    public void ConditionOR(string message) // An action node passes the information in the form: "newkey,key1,true,key2,false"
    {
        // This function takes keys and the expected bool values and calculates true if any key of the keys is included in the dictionary and has the expected bool value. Otherwise it calculates false.
        // The calculated value will be saved in the dictionary with the newkey.

        var tuple = NewKeyKeysValues(message);
        string newKey = tuple.Item1;
        string[] keys = tuple.Item2;
        bool[] values = tuple.Item3;

        bool condition = false;

        for (int i = 0; i < keys.Length; i++)
        {
            if (dict.ContainsKey(keys[i]))
            {
                if (dict[keys[i]] == values[i])
                {
                    condition = true;
                    break;
                }
            }
        }

        SaveValueInDictionary(newKey, condition);

    }

    public void ConditionNOT(string message) // This function takes a key, inverts its value, and stores it back in the dictionary.
    {
        if (dict.ContainsKey(message))
        {
            dict[message] = !dict[message];
        }
    }

    private void SaveValueInDictionary(string key, bool value) // Saves a value with a key in the Dictionary
    {
        if (dict.ContainsKey(key)) // If the key already exists, replace the value, otherwise add it
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }
    }


    (string[], bool[]) KeysValues(string message) // This function splits the string and stores the elements in an array.
    {
        string[] keysAndValues = message.Split(',');
        return SplittingKeysValues(keysAndValues); // Splits the array of keys and values into two unmixed arrays.
    }

    (int, string) IndexKey(string message)
    {
        string[] keyAndIndex = message.Split(',');
        int.TryParse(keyAndIndex[1], out int var);
        return (var, keyAndIndex[0]);
    }

    (string,string[],bool[]) NewKeyKeysValues(string message) // This function splits the string and stores the first entry as a single string and the rest in an array.
    {

        string[] splitmessage = message.Split(',');
        string[] keysAndValues = new string[splitmessage.Length - 1];
        Array.Copy(splitmessage, 1, keysAndValues, 0, splitmessage.Length - 1);

        var tuple = SplittingKeysValues(keysAndValues); // Splits the array of keys and values into two unmixed arrays.
        
        return (splitmessage[0], tuple.Item1,tuple.Item2);
    }

    (string[], bool[]) SplittingKeysValues(string[] keysAndValues) // This function takes an array of alternating keys and values and returns two unmixed arrays
    {
        int length = keysAndValues.Length / 2;

        string[] keys = new string[length];
        bool[] values = new bool[length];

        for (int i = 0; i < keysAndValues.Length; i++) //Fills the keys and value arrays with the entries from the keysAndValue array.
        {
            if (i % 2 == 0)
            {
                keys[i / 2] = keysAndValues[i];
            }
            else
            {
                bool.TryParse(keysAndValues[i], out bool var);
                values[(i - 1) / 2] = var;
            }
        }

        return (keys, values);
    }
}
