using BattleCity.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCity
{
    class PackmanController
    {
        public static int _spriteSize = 48;

        public static void PlayerWall_Collide(Kolobok pl, Wall wall)
        {
            if (boxCollides(pl, wall))
            {
                if (pl.direction == GameObject.Direction.left)
                {
                    pl.posX = wall.rightBorder;
                }
                else if (pl.direction == GameObject.Direction.right)
                {
                    pl.posX = wall.leftBorder - pl.image.Width;
                }
                if (pl.direction == GameObject.Direction.up)
                {
                    pl.posY = wall.bottomBorder;
                }
                else if (pl.direction == GameObject.Direction.down)
                {
                    pl.posY = wall.topBorder - pl.image.Height;
                }
                pl.TurnAround();
            }
        }
        public static void PlayerWater_Collide(Kolobok pl, Water water)
        {
            if (boxCollides(pl, water))
            {
                if (pl.direction == GameObject.Direction.left)
                {
                    pl.posX = water.rightBorder;
                }
                else if (pl.direction == GameObject.Direction.right)
                {
                    pl.posX = water.leftBorder - pl.image.Width;
                }
                if (pl.direction == GameObject.Direction.up)
                {
                    pl.posY = water.bottomBorder;
                }
                else if (pl.direction == GameObject.Direction.down)
                {
                    pl.posY = water.topBorder - pl.image.Height;
                }
                pl.TurnAround();
            }
        }
        public static bool PlayerApple_Collide(Kolobok pl, Apple apple)
        {
            if (boxCollides(pl, apple))
            {
                GameObjectsStorage.apples.Remove(apple);
                return true;
            }
            else return false;
        }
        public static bool PlayerEnemy_Collide(Kolobok pl, Tank en)
        {
            if (boxCollides(pl, en))
            {
                return true;
            }
            else return false;
        }

        public static void EnemyRandomDirection(Tank en)
        {
            if (en.directionStep >= en.image.Width)
            {
                en.directionStep = 0;

                Random rnd = new Random();
                int newDirection = rnd.Next(1, 7);
                switch (newDirection)
                {
                    case 1:
                        en.direction = GameObject.Direction.down;
                        break;

                    case 2:
                        en.direction = GameObject.Direction.up;
                        break;

                    case 3:
                        en.direction = GameObject.Direction.left;
                        break;

                    case 4:
                        en.direction = GameObject.Direction.right;
                        break;
                    default:
                        break;
                }
            }
        }
        public static void EnemyWall_Collide(Tank en, Wall wall)
        {
            if (boxCollides(en, wall))
            {
                if (en.direction == GameObject.Direction.left)
                {
                    en.posX = wall.rightBorder;
                }
                else if (en.direction == GameObject.Direction.right)
                {
                    en.posX = wall.leftBorder - en.image.Width;
                }
                if (en.direction == GameObject.Direction.up)
                {
                    en.posY = wall.bottomBorder;
                }
                else if (en.direction == GameObject.Direction.down)
                {
                    en.posY = wall.topBorder - en.image.Height;
                }
                en.TurnAround();
            }
        }
        public static void EnemyWater_Collide(Tank en, Water water)
        {
            if (boxCollides(en, water))
            {
                if (en.direction == GameObject.Direction.left)
                {
                    en.posX = water.rightBorder;
                }
                else if (en.direction == GameObject.Direction.right)
                {
                    en.posX = water.leftBorder - en.image.Width;
                }
                if (en.direction == GameObject.Direction.up)
                {
                    en.posY = water.bottomBorder;
                }
                else if (en.direction == GameObject.Direction.down)
                {
                    en.posY = water.topBorder - en.image.Height;
                }
                en.TurnAround();
            }
        }
        public static void EnemyEnemy_Collide(Tank en, Tank en2)
        {
            if (boxCollides(en, en2) && en != en2)
            {
                en.TurnAround();
            }
        }

        public static void BulletEnemy_Collide(Kolobok pl, int bulletIndex, Tank en)
        {
            if (boxCollides(pl.bullets[bulletIndex], en))
            {
                GameObjectsStorage.enemies.Remove(en);
                pl.bullets[bulletIndex].posX = -50;
                pl.bullets[bulletIndex].posY = -50;
                GameObjectsStorage.explosions.Add(new Explosion(en.posX, en.posY));
            }
        }
        public static void BulletWall_Collide(Kolobok pl, int bulletIndex, Wall wall)
        {
            if (boxCollides(pl.bullets[bulletIndex], wall))
            {
                pl.bullets[bulletIndex].posX = -50;
                pl.bullets[bulletIndex].posY = -50;
                GameObjectsStorage.explosions.Add(new Explosion(wall.posX, wall.posY));

                if (wall.breakable)
                {
                    wall.hitsTaken++;
                }

                if (wall.hitsTaken < 3)
                {
                    wall.GetWallImage();
                }
                else GameObjectsStorage.walls.Remove(wall);
            }
        }
        public static void BulletWall_Collide(Tank en, int bulletIndex, Wall wall)
        {
            if (boxCollides(en.bullets[bulletIndex], wall))
            {
                en.bullets[bulletIndex].posX = -50;
                en.bullets[bulletIndex].posY = -50;
                GameObjectsStorage.explosions.Add(new Explosion(wall.posX, wall.posY));

                if (wall.breakable)
                {
                    wall.hitsTaken++;
                }

                if (wall.hitsTaken < 3)
                {
                    wall.GetWallImage();
                }
                else GameObjectsStorage.walls.Remove(wall);
            }
        }
        public static bool BulletPlayer_Collide(Tank en, int bulletIndex, Kolobok pl)
        {
            if (boxCollides(en.bullets[bulletIndex], pl))
            {
                en.bullets[bulletIndex].posX = -50;
                en.bullets[bulletIndex].posY = -50;
                GameObjectsStorage.explosions.Add(new Explosion(en.posX, en.posY));
                return true;
            }
            else return false;
        }
        public static void AppleWall_Collide(Apple apple, Wall wall)
        {
            if (boxCollides(apple, wall))
            {
                apple.posY = wall.bottomBorder;
            }
        }
        public static void AppleWater_Collide(Apple apple, Water water)
        {
            if (boxCollides(apple, water))
            {
                apple.posY = water.bottomBorder;
            }
        }

        //GameObjects collider
        public static bool boxCollides(GameObject obj1, GameObject obj2)
        {
            if (obj1.collider.IntersectsWith(obj2.collider))
            {
                return true;
            }
            else if (obj1.posX == obj2.posX && obj1.posY == obj2.posY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //collider for new objects
        public static bool boxCollides(int x, int y, GameObject obj2)
        {
            Rectangle newObjectPlace = new Rectangle(x, y, _spriteSize, _spriteSize);
            if (newObjectPlace.IntersectsWith(obj2.collider))
            {
                return true;
            }
            else if (x == obj2.posX && y == obj2.posY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //collider for new obj and rect (using for player)
        public static bool boxCollides(int x, int y, Rectangle rect)
        {
            Rectangle newObjectPlace = new Rectangle(x, y, _spriteSize, _spriteSize);
            if (newObjectPlace.IntersectsWith(rect))
            {
                return true;
            }
            else if (x == rect.Left && y == rect.Top)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //collider for new obj and rect (using for player)
        public static bool boxCollides(GameObject obj, Rectangle rect)
        {
            if (obj.collider.IntersectsWith(rect))
            {
                return true;
            }
            else if (obj.posX == rect.Left && obj.posY == rect.Top)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
