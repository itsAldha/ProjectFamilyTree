using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FamilySim.Core
{
    public class FamilyTree : MonoBehaviour
    {
        [SerializeField] GameObject personInfo;
        [SerializeField] TextMeshProUGUI feedback;

        Spawner spawner;
        private void Start()
        {
            spawner = FindObjectOfType<Spawner>();
        }

        public class Person
        {
            public string name;
            public bool hasParent1 = false;
            public string parent1Name;
            public bool hasParent2 = false;
            public string parent2Name;
            public bool hasSpouse = false;
            public string spouseName;
            public bool hasSiblings = false;
            public List<string> siblings;
            public bool hasChildren = false;
            public List<string> children;

            public Person(string _name)
            {
                name = _name;
                siblings = new List<string>();
                children = new List<string>();
            }

            public void AddParent(string _parentName)
            {
                // if I have two parents already
                if (hasParent1 && hasParent2)
                    return;

                // if the parent has a spouse
                if (tree[_parentName].hasSpouse)
                {
                    if ((hasParent1 && parent1Name != tree[_parentName].spouseName) || (hasParent2 && parent2Name != tree[_parentName].spouseName))
                    {
                        print(name + " has a parent who's different from " + _parentName + "'s spouse");
                        return;
                    }
                }

                // look for every person who shares the same parent, and become siblings
                foreach (Person person in tree.Values)
                {
                    if (person.name == name || person.name == _parentName) continue;
                    if (siblings.Contains(person.name)) continue;

                    if (person.parent1Name == _parentName || person.parent2Name == _parentName)
                    {
                        siblings.Add(person.name);
                        hasSiblings = true;
                        person.siblings.Add(name);
                        person.hasSiblings = true;
                    }
                }
                // become first parent 
                if (!hasParent1)
                {
                    hasParent1 = true;
                    parent1Name = _parentName;
                    if (tree[_parentName].hasSpouse)
                    {
                        hasParent2 = true;
                        parent2Name = tree[_parentName].spouseName;
                    }
                }

                // become second parent
                else
                {
                    hasParent2 = true;
                    parent2Name = _parentName;
                    tree[parent2Name].hasSpouse = true;
                    tree[parent2Name].spouseName = parent1Name;
                    tree[parent1Name].hasSpouse = true;
                    tree[parent1Name].spouseName = parent2Name;
                }

                tree[_parentName].hasChildren = true;
                if(!tree[_parentName].children.Contains(name))
                    tree[_parentName].children.Add(name);
                if (tree[_parentName].hasSpouse)
                {
                    tree[tree[_parentName].spouseName].hasChildren = true;
                    if(!tree[tree[_parentName].spouseName].children.Contains(name))
                        tree[tree[_parentName].spouseName].children.Add(name);
                }
            }

            public void AddSpouse(string _spouseName)
            {
                // Become each other's spouses
                hasSpouse = true;
                spouseName = _spouseName;
                tree[_spouseName].hasSpouse = true;
                tree[_spouseName].spouseName = name;
                

                // merge children

                foreach (Person person in tree.Values)
                {
                    if (person.name == name || person.name == _spouseName) continue;
                    if(person.parent1Name == name || person.parent2Name == name)
                    {
                        if (person.parent1Name != spouseName && person.parent2Name != spouseName)
                            person.AddParent(spouseName);

                    }
                    else if (person.parent1Name == _spouseName || person.parent2Name == _spouseName)
                    {
                        if (person.parent1Name != name && person.parent2Name != name)
                            person.AddParent(name);
                    }
                }
            }

                public void Print()
            {
                print("=========================");
                print("name: " + name);
                if (!hasParent1 && !hasParent2)
                    print("has no parents.");
                else
                {
                    if (hasParent1)
                        print("parent1: " + parent1Name);
                    if (hasParent2)
                        print("parent2: " + parent2Name);
                }
                if(!hasSiblings)
                    print("has no siblings.");
                else
                {
                    foreach (string sibling in siblings)
                        print("sibling: " + sibling);
                }
                if (!hasSpouse)
                    print("has no spouse.");
                else
                    print("spouse: " + spouseName);
                if (!hasChildren)
                    print("has no children.");
                else
                {
                    foreach (string child in children)
                        print("child: " + child);
                }
                print("=========================");
            }
        }

        public static Dictionary<string,Person> tree = new Dictionary<string, Person>();



        public bool Ancestor(string person, string ancestor)
        {
            if (GetPerson(ancestor).hasParent1 == false && GetPerson(ancestor).hasParent2 == false)
                return false;

            if (GetPerson(ancestor).hasParent1)
            {
                if (GetPerson(ancestor).parent1Name == person)
                    return true;
                if (Ancestor(person, GetPerson(ancestor).parent1Name) == true)
                    return true;
            }

            if (GetPerson(ancestor).hasParent2)
            {
                if (GetPerson(ancestor).parent2Name == person)
                    return true;
                if (Ancestor(person, GetPerson(ancestor).parent2Name) == true)
                    return true;
            }
            return false;
        }

        public bool RelativeCheck(string person1, string person2)
        {
            if (Ancestor(person1, person2))
                return true;

            if( GetPerson(person1).hasParent1 )
            {
                if (Relative(GetPerson(person1).parent1Name, person2))
                    return true;
            }

            if (GetPerson(person1).hasParent2)
            {
                if (Relative(GetPerson(person1).parent2Name, person2))
                    return true;
            }

            return false;
        }

        public bool Relative(string person1, string person2)
        {
            if (RelativeCheck(person1, person2))
                return true;
            else if (RelativeCheck(person2, person1))
                return true;
            else
            {
                if (GetPerson(person1).hasSpouse)
                {
                    if (RelativeCheck(GetPerson(person1).spouseName, person2))
                        return true;
                    else if (RelativeCheck(person2, GetPerson(person1).spouseName))
                        return true;
                }

                if (GetPerson(person2).hasSpouse)
                {
                    if (RelativeCheck(GetPerson(person2).spouseName, person1))
                        return true;
                    else if (RelativeCheck(person1, GetPerson(person2).spouseName))
                        return true;
                }

                return false;
            }
        }

        public void ProcessOP(string function, string value)
        {
            function = function.ToLower();
            value = value.ToLower();

            if (function == "person" || function == "member" || function == "familymember" || function == "man" || function == "woman")
            {
                Person person = GetPerson(value);
                if (person != null)
                {
                    Feedback(value + " found.");
                    return;
                }

                person = new Person(value);
                tree.Add(value, person);
                spawner.Spawn(person);

                Feedback("Person created successfully.");
                return;
            }

            if (function == "print" || function == "details" || function == "info" || function == "information" || function == "get")
            {
                Person person = GetPerson(value);
                if(person == null)
                {
                    Feedback("Person does not exist.",true);
                    return;
                }
                else
                {
                    //person.Print();
                    Feedback("Details found.");
                    GameObject info = Instantiate(personInfo);
                    info.GetComponent<PersonInfo>().Initialize(person);
                    return;
                }

            }

            if (function == "parent" || function == "father" || function == "mother" || function == "dad" || function == "mom")
            {
                Person person = GetPerson(value);
                if (person == null)
                {
                    Feedback(value + " does not exist.", true);
                    return;
                }
                else
                {
                    if(person.hasChildren)
                        Feedback(value + " is a parent, has " + person.children.Count + " children.");
                    else
                        Feedback(value + " is not a parent.",true);
                    return;
                }
            }

            if (function == "sibling" || function == "siblings" || function == "brothers" || function == "sisters")
            {
                Person person = GetPerson(value);
                if (person == null)
                {
                    Feedback(value + " does not exist.", true);
                    return;
                }
                else
                {
                    if (person.hasSiblings)
                        Feedback(value + " has " + person.siblings.Count + " siblings.");
                    else
                        Feedback(value + " has no siblings.", true);
                    return;
                }
            }

            if (function == "spouse" || function == "married" || function == "husband" || function == "wife" || function == "partner")
            {
                Person person = GetPerson(value);
                if (person == null)
                {
                    Feedback(value + " does not exist.", true);
                    return;
                }
                else
                {
                    if (person.hasSpouse)
                        Feedback(value + " has a spouse: " + person.spouseName + ".");
                    else
                        Feedback(value + " has no spouse.", true);
                    return;
                }
            }
        }

        public void ProcessTP(string function, string value1, string value2)
        {
            
            function = function.ToLower();
            value1 = value1.ToLower();
            value2 = value2.ToLower();


            // If the function is "Ancestor"
            if (function == "ancestor" || function == "grandfather")
            {
                Person person = GetPerson(value1);
                if (person == null)
                {
                    Feedback("Person does not exist.", true);
                    return;
                }
                else
                {
                    if (Ancestor(value1,value2))
                        Feedback("is an ancestor.");
                    else
                        Feedback("is not an ancestor.", true);
                }

            }

            // If the function is "Relative"
            if (function == "relative" || function == "relatives" || function == "related")
            {
                Person person = GetPerson(value1);
                if (person == null)
                {
                    Feedback("Person does not exist.", true);
                    return;
                }
                else
                {
                    if (Relative(value1, value2))
                        Feedback("is a relative.");
                    else 
                        Feedback("is not a relative.", true);
                }

            }


            // If the function is "Child" switch to Parent
            if (function == "child" || function == "son" || function == "daughter")
            {
                function = "parent";
                string temp = value1;
                value1 = value2;
                value2 = temp;
            }

            // If the function is "Parent"
            if (function == "parent" || function == "father" || function == "mother" || function == "dad" || function == "mom")
            {
                Person parent = GetPerson(value1);
                if (parent == null)
                {
                    parent = new Person(value1);
                    tree.Add(value1, parent);
                    spawner.Spawn(parent);
                }

                Person child = GetPerson(value2);
                if (child != null)
                {

                    // If they are parent/child already
                    if (parent.parent1Name == value2 || parent.parent2Name == value2)
                    {
                        Feedback(value2 + " is already a parent to " + value1, true);
                        return;
                    }

                    // If they are siblings.. WTF?
                    if (child.siblings.Contains(parent.name))
                    {
                        Feedback("They are siblings..", true);
                        return;
                    }

                    // If they are spouses.. WTF?
                    if (child.spouseName == parent.name)
                    {
                        Feedback("They are spouses..", true);
                        return;
                    }

                    // If they are relatives
                    if (Relative(parent.name, child.name))
                    {
                        Feedback("They are relatives already.");
                        return;
                    }

                    // If the child has two parents
                    if (child.hasParent1 && child.hasParent2)
                    {
                        if (child.parent1Name == value1 || child.parent2Name == value1)
                            Feedback(value1 + " is already a parent of " + value2);
                        else
                            Feedback(value2 + " has different parents already", true);
                        return;
                    }

                    // If the child has one parent
                    else if (child.hasParent1 && child.parent1Name == value1)
                    {
                        Feedback(value1 + " is already a parent of " + value2);
                        return;
                    }

                    else if (child.hasParent2 && child.parent2Name == value1)
                    {
                        Feedback(value1 + " is already a parent of " + value2);
                        return;
                    }

                    child.AddParent(value1);
                }
                else
                {
                    child = new Person(value2);
                    tree.Add(value2, child);
                    child.AddParent(value1);
                    spawner.Spawn(child);
                }

                Feedback("Parent created successfully.");
            }

            // If the function is "Spouse"
            if (function == "spouse" || function == "married" || function == "husband" || function == "wife" || function == "partner")
            {
                Person person1 = GetPerson(value1);
                if (person1 == null)
                {
                    person1 = new Person(value1);
                    tree.Add(value1,person1);
                    spawner.Spawn(person1);
                }
                Person person2 = GetPerson(value2);
                if (person2 == null)
                {
                    person2 = new Person(value2);
                    tree.Add(value2, person2);
                    spawner.Spawn(person2);
                }

                if (person2 != null)
                {

                    // If one of them is a parent to the other ... WTF
                    if (person2.parent1Name == value1 || person2.parent2Name == value1 )
                    {
                        Feedback(value1 + " is a parent to " + value2, true);
                        return;
                    }

                    if (person1.parent1Name == value2 || person1.parent2Name == value2)
                    {
                        Feedback(value2 + " is a parent to " + value1, true);
                        return;
                    }

                    // If they are siblings.. WTF?
                    if (person2.siblings.Contains(person1.name))
                    {
                        Feedback("They are siblings..", true);
                        return;
                    }

                    // If they are spouses already
                    if (person2.spouseName == person1.name)
                    {
                        Feedback("They are spouses.");
                        return;
                    }

                    // If they are relatives
                    if (Relative(person1.name,person2.name))
                    {
                        Feedback("They are relatives.");
                        return;
                    }

                    // If one of them is married to someone else
                    if (person2.hasSpouse && person2.spouseName != value1 ||
                        person1.hasSpouse && person1.spouseName != value2)
                    {
                        Feedback("One of them is married to someone else.", true);
                        return;
                    }


                    person2.AddSpouse(value1);
                }

                Feedback("Spouses created successfully.");
            }
        }

        Person GetPerson(string _name)
        {
            _name = _name.ToLower();
            if (tree.ContainsKey(_name))
                return tree[_name];
            return null;
        }

        void Feedback(string _text, bool error = false)
        {
            feedback.text = _text;
            if (error)
                feedback.color = Color.red;
            else
                feedback.color = Color.green;
        }
    }
}