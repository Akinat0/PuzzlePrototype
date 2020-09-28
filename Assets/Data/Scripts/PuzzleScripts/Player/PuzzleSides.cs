using System;
using System.Data.Common;
using UnityEngine;

namespace Puzzle
{
    [Serializable]
    public struct PuzzleSides : IEquatable<PuzzleSides>
    {
        public PuzzleSides(bool top, bool right, bool bottom, bool left)
        {
            _Top = top;
            _Right = right;
            _Bottom = bottom;
            _Left = left;
        }
        
        [SerializeField] private bool _Top;
        [SerializeField] private bool _Right;
        [SerializeField] private bool _Bottom;
        [SerializeField] private bool _Left;

        public bool Top => _Top;
        public bool Right => _Right;
        public bool Bottom => _Bottom;
        public bool Left => _Left;
        
        public bool[] ToArray()
        {
            return new[] { Right, Bottom, Left, Top };
        }

        public static bool operator ==(PuzzleSides a, PuzzleSides b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PuzzleSides a, PuzzleSides b)
        {
            return !a.Equals(b);
        }

        public bool Equals(PuzzleSides other)
        {
            return other.Top == Top && other.Right == Right 
                                    && other.Bottom == Bottom && other.Left == Left;
        }
    }
}