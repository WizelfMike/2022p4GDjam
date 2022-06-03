using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameJam.Tools;
using GameJam.Game;

namespace GameJam.Game
{
    internal class Enemy : RenderObject
    {
        private readonly GameContext gc;

        internal Enemy(GameContext gc)
        {
            frames = gc.spriteMap.GetEnemyFrames();
            rectangle = new Rectangle(2 * gc.tileSize, 2 * gc.tileSize, gc.tileSize, gc.tileSize);
            this.gc = gc;
        }

        private void MoveEnemy(int x, int y)
        {
            float newx = rectangle.X + (x * gc.tileSize);
            float newy = rectangle.Y + (y * gc.tileSize);

            Tile next = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)newx, (int)newy))).FirstOrDefault();

            if (next != null)
            {
                if (next.graphic != '#' || next.graphic != 'D')
                {
                    rectangle.X = newx;
                    rectangle.Y = newy;
                }
            }
        }
    }
}
