using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FamilySim.Core
{
    public class Console : MonoBehaviour
    {
        [SerializeField] Command commandPrefab;
        public void AddCommand(string _text)
        {
            // Add new Command
            Command command = Instantiate(commandPrefab, transform);
            command.SetText(_text);
            // Check if full, delete the oldest
            if (transform.childCount > 10)
                Destroy(transform.GetChild(0).gameObject);
        }
    }
}