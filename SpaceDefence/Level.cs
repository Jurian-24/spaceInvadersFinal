using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDefence
{
    public static class Level
    {
        public static int level = 6;

        public static void IncreaseLevel()
        {
            level++;
        }

        public static int GetLevel()
        {
            return level;
        }

        public static void SetLevel(int newLevel)
        {
            level = newLevel;
        }

        public static void ResetLevel()
        {
            level = 1;
        }

        public static void spawnAlien()
        {
            Alien alien = new Alien();
            for (int i = 0; i < level; i++)
            {
                GameManager.GetGameManager().AddGameObject(alien);
            }
        }
    }
}
