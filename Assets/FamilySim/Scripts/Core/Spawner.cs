using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FamilySim.Core
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] GameObject personPrefab;
        public static Dictionary<string, GameObject> paths = new Dictionary<string, GameObject>();

        public void Spawn(FamilyTree.Person person)
        {
            if (paths.ContainsKey(person.name)) return;
            GameObject personGameObject = Instantiate(personPrefab, transform);
            personGameObject.name = person.name;
            paths.Add(personGameObject.name, personGameObject);

            personGameObject.GetComponent<Person>().data = person;
            personGameObject.GetComponent<Person>().Initialize();
        }
        /*
        public void SpouseMerge(string first, string second)
        {
            GameObject personGameObject = Instantiate(personPrefab, transform);
            personGameObject.name = first + " + " + second;
            paths[first] = personGameObject;

        }
        */
    }

}