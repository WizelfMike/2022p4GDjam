using System.Collections.Generic;

namespace GameJam.Game
{
    public class Room
    {
        public Tile[][] tiles;
        internal List<Enemy> enemys= new List<Enemy>();
        internal int roomx;
        internal int roomy;
    }
}



