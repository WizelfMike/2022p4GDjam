using GameJam.Game;
using GameJam.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GameJam
{

    public partial class RenderForm : Form
    {
        private LevelLoader levelLoader;
        private float frametime;
        private GameRenderer renderer;
        private readonly GameContext gc = new GameContext();

        public RenderForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            ResizeRedraw = true;

            KeyDown += RenderForm_KeyDown;
            FormClosing += Form1_FormClosing;
            Load += RenderForm_Load;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.Dispose();
        }
        private void RenderForm_Load(object sender, EventArgs e)
        {
            levelLoader = new LevelLoader(gc.tileSize, new FileLevelDataSource(), gc);
            levelLoader.LoadRooms(gc.spriteMap.GetMap());

            renderer = new GameRenderer(gc);

            gc.room = levelLoader.GetRoom(1, 1);

            gc.player = new RenderObject()
            {
                frames = gc.spriteMap.GetPlayerFrames(),
                rectangle = new Rectangle(4 * gc.tileSize, 3 * gc.tileSize, gc.tileSize, gc.tileSize),
            };


            ClientSize =
             new Size(

                (gc.tileSize * gc.room.tiles[0].Length) * gc.scaleunit,
                (gc.tileSize * gc.room.tiles.Length) * gc.scaleunit
                );
                gc.clientSize = ClientSize;
        }

        private void RenderForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                MovePlayer(0, -1);
            }
            else if (e.KeyCode == Keys.S)
            {
                MovePlayer(0, 1);
            }
            else if (e.KeyCode == Keys.A)
            {
                MovePlayer(-1, 0);
            }
            else if (e.KeyCode == Keys.D)
            {
                MovePlayer(1, 0);
            }

            Random r = new Random();

            if (e.KeyCode == Keys.A || e.KeyCode == Keys.W || e.KeyCode == Keys.S || e.KeyCode == Keys.D)
            {
                foreach (Enemy enemy in gc.room.enemys)
                {
                    enemy.MoveEnemy(r.Next(-1, 2), r.Next(-1, 2));
                    LoseCondision(enemy);
                }
            }
        }

        private void MovePlayer(int x, int y)
        {
            RenderObject player = gc.player;
            float newx = player.rectangle.X + (x * gc.tileSize);
            float newy = player.rectangle.Y + (y * gc.tileSize);

            Tile next = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)newx, (int)newy))).FirstOrDefault();

            if (next != null)
            {
                if (next.graphic == 'D')
                {
                    gc.room = levelLoader.GetRoom(gc.room.roomx + x, gc.room.roomy + y);

                    if (y != 0)
                    {
                        player.rectangle.Y += -y * ((gc.room.tiles.Length - 2) * gc.tileSize);
                    }
                    else
                    {
                        player.rectangle.X += -x * ((gc.room.tiles[0].Length - 2) * gc.tileSize);
                    }
                }

                else if (next.graphic != '#' && next.graphic != '@')
                {
                    player.rectangle.X = newx;
                    player.rectangle.Y = newy;
                }

                if (next.graphic == '%')
                {
                    gc.states = GameStates.winGame;
                }
            }
        }

        public void Logic(float frametime)
        {
            this.frametime = frametime;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            renderer.Render(e, frametime);
        }

        private void LoseCondision(Enemy enemy)
        {
            RenderObject player = gc.player;

            if (enemy.rectangle == player.rectangle)
            {
                gc.states = GameStates.endGame;
            }
        }

        private void RenderForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}


