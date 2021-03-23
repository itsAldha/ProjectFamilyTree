using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FamilySim.Core
{
    public class PersonInfo : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI personName;
        [SerializeField] TextMeshProUGUI parentsData;
        [SerializeField] RectTransform siblingsData;
        [SerializeField] TextMeshProUGUI spouseData;
        [SerializeField] RectTransform childrenData;
        [SerializeField] GameObject textPrefab;

        public void Initialize(FamilyTree.Person person)
        {
            siblingsData.anchoredPosition = new Vector3(0, -177.712f, 0);
            childrenData.anchoredPosition = new Vector3(0, -177.712f, 0);
            personName.text = person.name;
            if (person.hasSiblings)
            {
                foreach(string sibling in person.siblings)
                {
                    GameObject text = Instantiate(textPrefab, siblingsData);
                    text.GetComponent<TextMeshProUGUI>().text = sibling;
                }
            }
            else
            {
                GameObject text = Instantiate(textPrefab, siblingsData);
                text.GetComponent<TextMeshProUGUI>().text = "None";
            }

            if (!person.hasParent1 && !person.hasParent2)
                parentsData.text = "None";
            else if (person.hasParent1 && !person.hasParent2)
                parentsData.text = "(1) " + person.parent1Name;
            else if (person.hasParent1 && person.hasParent2)
                parentsData.text = "(1) " + person.parent1Name + Environment.NewLine +
                    "(2) " + person.parent2Name;
            if (person.hasSpouse)
                spouseData.text = person.spouseName;
            else
                spouseData.text = "None";

            if (person.hasChildren)
            {
                foreach (string child in person.children)
                {
                    GameObject text = Instantiate(textPrefab, childrenData);
                    text.GetComponent<TextMeshProUGUI>().text = child;
                }
            }
            else
            {
                GameObject text = Instantiate(textPrefab, childrenData);
                text.GetComponent<TextMeshProUGUI>().text = "None";
            }


        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

    }
}