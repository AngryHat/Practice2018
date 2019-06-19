using BattleCity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCity
{
    class GameObjectsStorage
    {
        public static List<GameObject> ObjectViewFormList
        {
            get
            {
                List<GameObject> result = new List<GameObject>();
                result.Add(pl);
                result.AddRange(enemies);
                result.AddRange(apples);
                return result;
            }
        }

        public static Kolobok pl;
        public static List<GameObject> enemies;
        public static List<GameObject> walls;
        public static List<GameObject> apples;
    }
}
