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
            }
        }
        public static void BulletWall_Collide(Kolobok pl, int bulletIndex, Wall wall)
        {
            if (boxCollides(pl.bullets[bulletIndex], wall))
            {
                pl.bullets[bulletIndex].posX = -50;
                pl.bullets[bulletIndex].posY = -50;
            }
        }
        public static void BulletWall_Collide(Tank en, int bulletIndex, Wall wall)
        {
            if (boxCollides(en.bullets[bulletIndex], wall))
            {
                en.bullets[bulletIndex].posX = -50;
                en.bullets[bulletIndex].posY = -50;
            }
        }
        public static bool BulletPlayer_Collide(Tank en, int bulletIndex, Kolobok pl)
        {
            if (boxCollides(en.bullets[bulletIndex], pl))
            {
                en.bullets[bulletIndex].posX = -50;
                en.bullets[bulletIndex].posY = -50;
                return true;
            }
            else return false;
        }

        //GameObjects collider
        public static bool boxCollides(GameObject obj1, GameObject obj2)
        {
            if (obj1.collider.IntersectsWith(obj2.collider))
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
            else
            {
                return false;
            }
        }
    }
}

//USELESS EN CHANGE DIRECTION METHOD
//Random rnd = new Random();
//int newDirection = rnd.Next(1, 4);
//                switch (en.direction)
//                {
//                    case GameObject.Direction.down:
//                        if (newDirection == 1) { en.direction = GameObject.Direction.up; }
//                        else if (newDirection == 2) { en.direction = GameObject.Direction.left; }
//                        else if (newDirection == 3) { en.direction = GameObject.Direction.right; };
//                        break;

//                    case GameObject.Direction.up:
//                        if (newDirection == 1) { en.direction = GameObject.Direction.down; }
//                        else if (newDirection == 2) { en.direction = GameObject.Direction.left; }
//                        else if (newDirection == 3) { en.direction = GameObject.Direction.right; };
//                        break;

//                    case GameObject.Direction.left:
//                        if (newDirection == 1) { en.direction = GameObject.Direction.up; }
//                        else if (newDirection == 2) { en.direction = GameObject.Direction.down; }
//                        else if (newDirection == 3) { en.direction = GameObject.Direction.right; };
//                        break;

//                    case GameObject.Direction.right:
//                        if (newDirection == 1) { en.direction = GameObject.Direction.up; }
//                        else if (newDirection == 2) { en.direction = GameObject.Direction.left; }
//                        else if (newDirection == 3) { en.direction = GameObject.Direction.down; };
//                        break;
//                }
