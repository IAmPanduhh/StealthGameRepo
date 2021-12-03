using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool can_pause = true;
    [Header("PLAYER STATS")]
    public float sprint_level;
    public float max_sprint_level;
    public bool can_sprint;
    public bool sprinting;

    void Awake()
    {
        if (instance == null)
        {
            instance = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        }
    }

    void Start()
    {
        instance = this;
        sprint_level = max_sprint_level;
        can_sprint = true;
    }

    void Update()
    {

    }
}
