﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PlayerInput : MonoBehaviour
    {

        private Player _player;

        void Start()
        {
            _player = GetComponent<Player>();
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _player.ChangeSides();
            }
        }
    }
}