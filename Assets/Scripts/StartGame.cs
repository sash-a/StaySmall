using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //print(GetComponentInChildren<Slider>().value);
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("GameScreen");
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            var comps = GetComponentsInChildren<Text>();

            foreach (var comp in comps)
            {
                if (comp.name.Equals("Difficulty"))
                {
                    comp.text = "Difficulty: 1";
                }
            }

            LevelManager.difficulty = 1;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            var comps = GetComponentsInChildren<Text>();

            foreach (var comp in comps)
            {
                if (comp.name.Equals("Difficulty"))
                {
                    comp.text = "Difficulty: 2";
                }
            }

            LevelManager.difficulty = 2;
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            var comps = GetComponentsInChildren<Text>();

            foreach (var comp in comps)
            {
                if (comp.name.Equals("Difficulty"))
                {
                    comp.text = "Difficulty: 3";
                }
            }

            LevelManager.difficulty = 3;
        }

        if (Input.GetKey(KeyCode.Alpha4))
        {
            var comps = GetComponentsInChildren<Text>();

            foreach (var comp in comps)
            {
                if (comp.name.Equals("Difficulty"))
                {
                    comp.text = "Difficulty: 4";
                }
            }

            LevelManager.difficulty = 4;
        }

        if (Input.GetKey(KeyCode.Alpha5))
        {
            var comps = GetComponentsInChildren<Text>();

            foreach (var comp in comps)
            {
                if (comp.name.Equals("Difficulty"))
                {
                    comp.text = "Difficulty: 5";
                }
            }

            LevelManager.difficulty = 5;
        }
    }
}