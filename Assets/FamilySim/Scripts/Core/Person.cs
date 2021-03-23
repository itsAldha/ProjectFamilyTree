using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FamilySim.Core
{
    public class Person : MonoBehaviour
    {
        [SerializeField] GameObject linePrefab;
        Transform root;

        bool parent1Linked = false;
        bool parent2Linked = false;
        bool spouseLinked = false;

        public FamilyTree.Person data;
        bool active = false;

        private void Awake()
        {
            root = GameObject.Find("Spawner").transform;
        }

        public void Initialize()
        {
            GetComponentInChildren<TextMeshProUGUI>().text = data.name;
            if (data.hasParent1)
            {
                Transform parent = Spawner.paths[data.parent1Name].transform;
                transform.SetParent(parent, false);
                SpringJoint2D joint = gameObject.AddComponent<SpringJoint2D>();
                joint.connectedBody = transform.parent.GetComponent<Rigidbody2D>();
                joint.autoConfigureDistance = false;
                joint.distance = 3;

            }
            active = true;
        }

        public void UpdateData()
        {
            data = FamilyTree.tree[data.name];
            if (data.hasParent1 && transform.parent.name != data.parent1Name )
            {
                transform.SetParent(Spawner.paths[data.parent1Name].transform, false);
                SpringJoint2D joint = transform.GetComponent<SpringJoint2D>();
                if(joint==null)
                    joint = gameObject.AddComponent<SpringJoint2D>();
                joint.connectedBody = transform.parent.GetComponent<Rigidbody2D>();
                joint.autoConfigureDistance = false;
                joint.distance = 3;
            }
            if (!data.hasParent1 && !data.hasParent2)
            {
                GetComponent<Rigidbody2D>().isKinematic = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                GetComponent<Rigidbody2D>().isKinematic = false;
                if (data.hasParent1 && !parent1Linked)
                {
                    parent1Linked = true;
                    GameObject line = Instantiate(linePrefab, transform);
                    line.name = "Parent1Link";
                    line.GetComponent<Line>().child = gameObject;
                    line.GetComponent<Line>().parent = Spawner.paths[data.parent1Name];
                    line.GetComponent<Line>().Initialize(new Color(0.7f, 0.7f, 0.65f), Color.black);
                }
                if (data.hasParent2 && !parent2Linked)
                {
                    parent2Linked = true;
                    GameObject line = Instantiate(linePrefab, transform);
                    line.name = "Parent2Link";
                    line.GetComponent<Line>().child = gameObject;
                    line.GetComponent<Line>().parent = Spawner.paths[data.parent2Name];
                    line.GetComponent<Line>().Initialize(new Color(0.7f, 0.7f, 0.65f), Color.black);
                }
            }

            if (data.hasSpouse && !spouseLinked)
            {
                spouseLinked = true;
                GameObject line = Instantiate(linePrefab, transform);
                line.name = "SpouseLink";
                line.GetComponent<Line>().child = gameObject;
                line.GetComponent<Line>().parent = Spawner.paths[data.spouseName];
                line.GetComponent<Line>().Initialize(Color.red, Color.red);
            }
        }

        private void Update()
        {
            if (!active) return;
            UpdateData();
        }
    }

}