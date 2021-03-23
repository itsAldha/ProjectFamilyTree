using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FamilySim.Core
{
    public class Line : MonoBehaviour
    {
        [SerializeField] Material material;
        public GameObject parent = null;
        public GameObject child = null;       

        private LineRenderer line;

        // Use this for initialization
        public void Initialize(Color startcolor, Color endColor)
        {
            // Add a Line Renderer to the GameObject
            line = this.gameObject.AddComponent<LineRenderer>();
            // Set the width of the Line Renderer
            line.startWidth = 0.05f;
            line.endWidth = 0.05f;
            // Set the number of vertex fo the Line Renderer
            line.positionCount = 2;
            line.material = material;
            line.sortingOrder = -1;
            line.startColor = startcolor;
            line.endColor = endColor;
        }

        // Update is called once per frame
        void Update()
        {
            // Check if the GameObjects are not null
            if (parent != null && child != null)
            {
                // Update position of the two vertex of the Line Renderer
                line.SetPosition(0, parent.transform.position);
                line.SetPosition(1, child.transform.position);
            }
        }
    }
}