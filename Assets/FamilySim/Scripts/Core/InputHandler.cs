using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace FamilySim.Core
{

    public class InputHandler : MonoBehaviour
    {
        [SerializeField] TMP_InputField input;
        [SerializeField] Console console;
        [SerializeField] TextMeshProUGUI feedback;

        FamilyTree familyTree;

        private void Start()
        {
            familyTree = GameObject.Find("FamilyTree").GetComponent<FamilyTree>();
        }

        void Update()
        {
            if ( !string.IsNullOrEmpty(input.text) && ( Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) ) )
            {
                bool successful = Process(input.text);
                if (!successful) return;
                console.AddCommand(input.text);
                ResetInput();
            }
        }


        bool Process(string _text)
        {
            bool oneParameterCommand = Regex.IsMatch( _text, @"^[a-z-A-Z]+\([a-z-A-Z]+\)[.]*$");
            bool twoParameterCommand = Regex.IsMatch(_text, @"^[a-z-A-Z]+\([a-z-A-Z]+\,[a-z-A-Z]+\)[.]*$");

            if (!oneParameterCommand && !twoParameterCommand)
            {
                Feedback("Invalid command", true);
                ResetInput();
                return false;
            }
            else
                Feedback("");

            // Prolog
            string function;
            string value1 = "";
            string value2 = "";

            // Get The Function
            string[] temp1 = _text.Split('(');
            function = temp1[0];

            // If it's a One Parameter Function
            if(oneParameterCommand)
            {
                string[] temp2 = temp1[1].Split(')');
                value1 = temp2[0];
            }

            // If it's a Two Parameter Function
            else if (twoParameterCommand)
            {
                string[] temp2 = temp1[1].Split(',');
                value1 = temp2[0];
                string[] temp3 = temp2[1].Split(')');
                value2 = temp3[0];
            }

            print("Function is (" + function + ") and value1: (" + value1 + ") value2: (" + value2 + ")");

            if (oneParameterCommand) familyTree.ProcessOP(function, value1);
            else if (twoParameterCommand) familyTree.ProcessTP(function, value1, value2);

            return true;
        }

        

        void Feedback(string _text, bool error = false)
        {
            feedback.text = _text;
            if (error)
                feedback.color = Color.red;
            else
                feedback.color = Color.green;
        }

        private void ResetInput()
        {
            input.text = "";
            input.Select();
            input.ActivateInputField();
        }

    }

}