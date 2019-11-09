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
            if (dict.ContainsKey(keys[i])) // If the key already exists, replace the value, otherwise add it
            {
                dict[keys[i]] = values[i];
            }
            else
            {
                dict.Add(keys[i], values[i]);
            }
        }
    }

    public void Condition(string message) // An action node passes the information in the form: "text_index,key1,true,key2,false"
    {
        // This function takes keys and the expected bool values and returns true only if all keys are included in the dictionary and have the expected bool value. Otherwise it returns false.
        // ToDo: The calculated logical value is sent directly to the UIManager, maybe one should first store it in the Dictionary and create an extra function for sending a variable.
        // ToDo: Exchange the text_index with a key in which the result of the evaluation should be stored.
        // ToDo: Change the name to "ConditionAND", because this function only connects bool values with AND.
        // ToDo: Add a "ConditionOR", a "ConditionNOT" and a "Push"(Which passes a bool and a text_index to the UIManager.) function.

        var tuple = IntegerKeysValue(message);
        string[] keys = tuple.Item1;
        bool[] values = tuple.Item2;
        int text_index = tuple.Item3;

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

        uIManager.FeedBools((text_index, condition));
    }

    (string[], bool[]) KeysValues(string message) // This function splits the string and stores the elements in an array.
    {
        string[] keysAndValues = message.Split(',');
        return SplittingKeysValues(keysAndValues); // Splits the array of keys and values into two unmixed arrays.
    }

    (string[],bool[],int) IntegerKeysValue(string message) // This function splits the string and stores the first entry as a single int and the rest in an array.
    {
        // ToDo: Matching the changes above, the single int here would have to be made into a single string for the key

        string[] splitmessage = message.Split(',');
        string[] keysAndValues = new string[splitmessage.Length - 1];
        Array.Copy(splitmessage, 1, keysAndValues, 0, splitmessage.Length - 1);

        var tuple = SplittingKeysValues(keysAndValues); // Splits the array of keys and values into two unmixed arrays.

        int.TryParse(splitmessage[0], out int var);

        return (tuple.Item1,tuple.Item2, var);
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
