using System;
using UnityEngine;

namespace Game
{
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public static class DirectionExtensions
    {
        public static Direction Rotated(this Direction direction, Quaternion rotation)
        {
            int rotationZ = (int)MathF.Floor(rotation.eulerAngles.z);
            while (rotationZ < 0)
            {
                rotationZ += 360;
            }

            while (rotationZ >= 360)
            {
                rotationZ -= 360;
            }
            
            if (rotationZ % 90 != 0)
            {
                throw new Exception("Don't be silly.");
            }

            switch (rotationZ)
            {
                case 0:
                    return direction;
                case 90:
                    return direction switch
                    {
                        Direction.North => Direction.West,
                        Direction.East => Direction.North,
                        Direction.South => Direction.East,
                        Direction.West => Direction.South,
                    };
                case 180:
                    return direction switch
                    {
                        Direction.North => Direction.South,
                        Direction.East => Direction.West,
                        Direction.South => Direction.North,
                        Direction.West => Direction.East,
                    };
                case 270:
                    return direction switch
                    {
                        Direction.North => Direction.East,
                        Direction.East => Direction.South,
                        Direction.South => Direction.West,
                        Direction.West => Direction.North,
                    };
            }

            throw new ArgumentOutOfRangeException();
        }

        public static Direction Opposite(this Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.South,
                Direction.East => Direction.West,
                Direction.South => Direction.North,
                Direction.West => Direction.East,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        
        public static int GetXOffset(this Direction direction)
        {
            return direction switch
            {
                Direction.East => 1,
                Direction.West => -1,
                _ => 0
            };
        }
        
        public static int GetYOffset(this Direction direction)
        {
            return direction switch
            {
                Direction.North => 1,
                Direction.South => -1,
                _ => 0
            };
        }

        public static Vector3 GetOffset(this Direction direction)
        {
            return new Vector3(direction.GetXOffset(), direction.GetYOffset(), 0);
        }
    }
}