using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FamilySim.Core
{
    public class Command : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] int index;

        public void SetText(string _text)
        {
            text.text = _text;
        }

        private void Update()
        {
            index = -(transform.GetSiblingIndex() - transform.parent.childCount + 1);
            if(index == 6)
            {
                Color newColor = text.color;
                newColor.a = 0.8f;
                text.color = newColor;
            }
            else if (index == 7)
            {
                Color newColor = text.color;
                newColor.a = 0.6f;
                text.color = newColor;
            }
            else if (index == 8)
            {
                Color newColor = text.color;
                newColor.a = 0.4f;
                text.color = newColor;
            }
            else if (index == 9)
            {
                Color newColor = text.color;
                newColor.a = 0.2f;
                text.color = newColor;
            }


        }
    }
}