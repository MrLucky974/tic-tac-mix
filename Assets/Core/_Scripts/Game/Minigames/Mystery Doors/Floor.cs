using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class Floor : Stage
    {
        public Stage previousStage;

        [SerializeField] private SpriteRenderer m_wall;
        [SerializeField] private Door[] m_doors;

        public void Initialize(Color color)
        {
            m_wall.color = color;

            var doors = new List<Door>(m_doors);

            // This code only executes if the number of doors provided is at least
            // one.
            if (doors.Count == 0)
                return;

            // Pick a random door to mark it as safe to enter,
            // every other door will be trapped
            var safeDoorIndex = Random.Range(0, doors.Count);
            var safeDoor = doors[safeDoorIndex];
            safeDoor.Initialize(false);
            doors.RemoveAt(safeDoorIndex);

            foreach (var door in doors)
            {
                door.Initialize(true);
            }
        }
    }
}
