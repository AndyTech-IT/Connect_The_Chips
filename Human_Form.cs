using Connect_The_Chips.Game;
using Connect_The_Chips.Game.Chips;
using Connect_The_Chips.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect_The_Chips.Players
{
    public partial class Human_Form : Form
    {
        private const int CHIP_SIZE = 256;
        private const int CHIP_BORDER = 5;

        private Bitmap[] L_Chips;
        private Bitmap[] T_Chips;
        private Bitmap[] I_Chips;

        private Bitmap[] Nodes;
        private Bitmap Obstruction;

        public Round_Result Result => _result;
        private Round_Result _result;

        private int _cell_size;
        private Size _image_size;
        private int _image_height => _image_size.Height;
        private int _image_width => _image_size.Width;

        private Size _mesh_size;
        private int _mesh_height => _mesh_size.Height;
        private int _mesh_width => _mesh_size.Width;

        private Human_Player _player;
        private Image _map;
        private Obstruction[] _obstructions;
        private Connection_Node[] _nodes;
        private Connection_Chip[] _placement_chips;
        private Connection_Chip[] _placed_chips;
        private Connection_Chip _hover_chip;

        private GameObject[] _all => 
            _nodes.Cast<GameObject>()
            .Union(_placement_chips.Cast<GameObject>())
            .Union(_placed_chips.Cast<GameObject>())
            .Union(_obstructions.Cast<GameObject>())
            .OrderBy(o=>-o.Y).ThenBy(o=>-o.X).ToArray();

        private Button[] _buttons;
        public Human_Form(Human_Player player)
        {
            InitializeComponent();
            _buttons = new Button[] { button1, button2, button3 };
            _hover_chip = null;
            _player = player;
        }

        private void Init_ChipsImages()
        {
            Pen pen = new Pen(Color.Black, CHIP_SIZE / 6);
            Point p = new Point(CHIP_SIZE / 2, CHIP_SIZE / 2);
            int center = CHIP_SIZE / 2;

            L_Chips = new Bitmap[4];
            T_Chips = new Bitmap[4];
            I_Chips = new Bitmap[4];
            Nodes = new Bitmap[4];
            for (int i = 0; i < 4; i++)
            {
                L_Chips[i] = new Bitmap(CHIP_SIZE, CHIP_SIZE);
                T_Chips[i] = new Bitmap(CHIP_SIZE, CHIP_SIZE);
                Nodes[i] = new Bitmap(CHIP_SIZE, CHIP_SIZE);
            }

            I_Chips[0] = I_Chips[2] = new Bitmap(CHIP_SIZE, CHIP_SIZE);
            I_Chips[1] = I_Chips[3] = new Bitmap(CHIP_SIZE, CHIP_SIZE);

            using (var g = Graphics.FromImage(L_Chips[0]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, p, new Point(center, CHIP_SIZE));
                g.DrawLine(pen, p, new Point(CHIP_SIZE, center));
            }

            using (var g = Graphics.FromImage(L_Chips[1]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, p, new Point(center, 0));
                g.DrawLine(pen, p, new Point(CHIP_SIZE, center));
            }

            using (var g = Graphics.FromImage(L_Chips[2]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, p, new Point(center, 0));
                g.DrawLine(pen, p, new Point(0, center));
            }

            using (var g = Graphics.FromImage(L_Chips[3]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, p, new Point(center, CHIP_SIZE));
                g.DrawLine(pen, p, new Point(0, center));
            }


            using (var g = Graphics.FromImage(T_Chips[0]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, p, new Point(center, CHIP_SIZE));
                g.DrawLine(pen, new Point(0, center), new Point(CHIP_SIZE, center));
            }

            using (var g = Graphics.FromImage(T_Chips[1]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, p, new Point(CHIP_SIZE, center));
                g.DrawLine(pen, new Point(center, 0), new Point(center, CHIP_SIZE));
            }

            using (var g = Graphics.FromImage(T_Chips[2]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, p, new Point(center, 0));
                g.DrawLine(pen, new Point(0, center), new Point(CHIP_SIZE, center));
            }

            using (var g = Graphics.FromImage(T_Chips[3]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, p, new Point(0, center));
                g.DrawLine(pen, new Point(center, 0), new Point(center, CHIP_SIZE));
            }


            using (var g = Graphics.FromImage(I_Chips[0]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, new Point(0, center), new Point(CHIP_SIZE, center));
            }

            using (var g = Graphics.FromImage(I_Chips[1]))
            {
                g.Clear(Color.White);
                g.DrawLine(pen, new Point(center, 0), new Point(center, CHIP_SIZE));
            }

            using (var g = Graphics.FromImage(Nodes[0]))
            {
                g.Clear(Color.Gray);
                g.DrawLine(pen, p, new Point(CHIP_SIZE, center));
            }

            using (var g = Graphics.FromImage(Nodes[1]))
            {
                g.Clear(Color.Gray);
                g.DrawLine(pen, p, new Point(center, 0));
            }

            using (var g = Graphics.FromImage(Nodes[2]))
            {
                g.Clear(Color.Gray);
                g.DrawLine(pen, p, new Point(0, center));
            }

            using (var g = Graphics.FromImage(Nodes[3]))
            {
                g.Clear(Color.Gray);
                g.DrawLine(pen, p, new Point(center, CHIP_SIZE));
            }

            pen = new Pen(Color.DarkGray, CHIP_BORDER * 2);
            Pen pen2 = new Pen(Color.LightGray, CHIP_BORDER * 2);

            Obstruction = new Bitmap(CHIP_SIZE, CHIP_SIZE);
            using (var g = Graphics.FromImage(Obstruction))
            {
                g.Clear(Color.Gray);
                g.DrawLine(pen2, 0, 0, 0, CHIP_SIZE);
                g.DrawLine(pen2, 0, 0, CHIP_SIZE, 0);
                g.DrawLine(pen, CHIP_SIZE, CHIP_SIZE, 0, CHIP_SIZE);
                g.DrawLine(pen, CHIP_SIZE, CHIP_SIZE, CHIP_SIZE, 0);
            }
            foreach (Bitmap[] arr in new Bitmap[][] { L_Chips, T_Chips, I_Chips, Nodes })
                foreach (var image in arr)
                    using (var g = Graphics.FromImage(image))
                    {
                        g.DrawLine(pen2, 0, 0, 0, CHIP_SIZE);
                        g.DrawLine(pen2, 0, 0, CHIP_SIZE, 0);
                        g.DrawLine(pen, CHIP_SIZE, CHIP_SIZE, 0, CHIP_SIZE);
                        g.DrawLine(pen, CHIP_SIZE, CHIP_SIZE, CHIP_SIZE, 0);
                    }
        }

        public void Get_Pack(Chips_Pack pack)
        {
            for (int i = 0; i < Game_Controller.CHIPS_PACK_SIZE; i++)
            {
                if (pack.Chips[i] is T_Chip)
                    _buttons[i].Text = "T";
                else if (pack.Chips[i] is L_Chip)
                    _buttons[i].Text = "L";
                else if (pack.Chips[i] is I_Chip)
                    _buttons[i].Text = "I";
            }
            _placement_chips = pack.Chips;
            Update_PB();
        }

        public void Init_Map(Map_Data map)
        {
            int margin = 20;
            _mesh_size = Game_Controller.MAP_SIZE;
            Size pb_size = new Size(Map_PB.Width - margin, Map_PB.Height - margin);
            double max_width = pb_size.Width / _mesh_size.Width;
            double max_height = pb_size.Height / _mesh_size.Height;
            _cell_size = max_width < max_height ? (int)max_width : (int)max_height;
            _image_size = new Size(_cell_size * _mesh_size.Width, _cell_size * _mesh_size.Height);

            _map = new Bitmap(_image_width, _image_width);
            Map_PB.Image = new Bitmap(_image_width, _image_width);

            _obstructions = map.Obstructions;
            _nodes = map.Nodes;

            _placed_chips = new Connection_Chip[0];

            Init_ChipsImages();
            Draw_Map();
        }

        private void Human_Form_MouseUp(object sender, MouseEventArgs e)
        {
            _hover_chip = null;
        }

        private void Update_PB()
        {
            using (var g = Graphics.FromImage(Map_PB.Image))
            {
                g.DrawImage(_map, Point.Empty);
                SolidBrush b = new SolidBrush(Color.White);
                SolidBrush b2 = new SolidBrush(Color.Black);

                int chip_border = (int)(CHIP_BORDER * ((double)_cell_size / CHIP_SIZE)) + 1;

                foreach (var gameoOject in _all)
                {
                    if (gameoOject.X == -1 && gameoOject.Y == -1)
                        continue;
                    Point start = new Point(gameoOject.X * _cell_size - chip_border, gameoOject.Y * _cell_size - chip_border);
                    g.DrawImage(Get_Image(gameoOject), start.X, start.Y, _cell_size + chip_border*2, _cell_size + chip_border*2);
                }
            }
            Map_PB.Refresh();
        }

        private Image Get_Image(GameObject gameObject)
        {
            Bitmap image;
            if (gameObject is L_Chip)
                image = new Bitmap(L_Chips[(int)gameObject.Rotation]);
            else if (gameObject is T_Chip)
                image = new Bitmap(T_Chips[(int)gameObject.Rotation]);
            else if (gameObject is I_Chip)
                image = new Bitmap(I_Chips[(int)gameObject.Rotation]);
            else if (gameObject is Connection_Node)
                image = new Bitmap(Nodes[(int)gameObject.Rotation]);
            else if (gameObject is Obstruction)
                image = new Bitmap(Obstruction);
            else
                throw new IndexOutOfRangeException($"Type {gameObject.GetType()} dont supports!");

            Pen eracer = new Pen(Color.Transparent, CHIP_BORDER*2);
            using (var g = Graphics.FromImage(image))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                if (gameObject.Y == Game_Controller.MAP_HEIGHT - 1 || _all.Any(o => o.X == gameObject.X && o.Y == gameObject.Y + 1))
                    g.DrawLine(eracer, 0, CHIP_SIZE, CHIP_SIZE, CHIP_SIZE);
                if (gameObject.Y == 0 || _all.Any(o => o.X == gameObject.X && o.Y == gameObject.Y - 1))
                    g.DrawLine(eracer, 0, 0, CHIP_SIZE, 0);
                
                if (gameObject.X == Game_Controller.MAP_WIDTH - 1 || _all.Any(o => o.Y == gameObject.Y && o.X == gameObject.X + 1))
                    g.DrawLine(eracer, CHIP_SIZE, 0, CHIP_SIZE, CHIP_SIZE);
                if (gameObject.X == 0 || _all.Any(o => o.Y == gameObject.Y && o.X == gameObject.X - 1))
                    g.DrawLine(eracer, 0, 0, 0, CHIP_SIZE);
            }

            return image;
        }

        private void Draw_Map()
        {
            using (var g = Graphics.FromImage(_map))
            {
                g.Clear(Color.White);

                Pen p = new Pen(Color.Black, 2);
                for (int y = 1; y < _mesh_height; y++)
                    g.DrawLine(p, 0, y * _cell_size, _image_width, y * _cell_size);
                for (int x = 1; x < _mesh_width; x++)
                    g.DrawLine(p, x * _cell_size, 0, x * _cell_size, _image_height);
            }
            Map_PB.Size = _image_size;
        }

        private void Map_PB_MouseUp(object sender, MouseEventArgs e)
        {
            if (_hover_chip != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    _hover_chip.Rotation = _hover_chip.Rotation == Rotation.Degree_270 ? 0 : _hover_chip.Rotation + 1;
                    Update_PB();
                }
                if (e.Button == MouseButtons.Left)
                    _hover_chip = null;
            }
        }

        private void Map_PB_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                _hover_chip = _placement_chips[int.Parse(b.Tag.ToString())];
            }
        }

        private void Map_PB_MouseMove(object sender, MouseEventArgs e)
        {
            if (_hover_chip is null)
                return;

            int mouse_x = e.X;
            int mouse_y = e.Y;
            Text = $"{mouse_x}, {mouse_y}";
            mouse_x = mouse_x > 0 ? mouse_x / _cell_size : 0;
            mouse_y = mouse_y > 0 ? mouse_y / _cell_size : 0;

            if (mouse_x == _hover_chip.Position.X && mouse_y == _hover_chip.Position.Y)
                return;

            foreach (var gameObject in _all)
                if (gameObject.X == mouse_x && gameObject.Y == mouse_y)
                    return;

            _hover_chip.Position = new Point(mouse_x, mouse_y);

            Invoke((MethodInvoker) Update_PB);
        }

        private void Accept_TSMI_Click(object sender, EventArgs e)
        {
            _result = new Round_Result() { Placed_Chips = _placement_chips };
            if (_player.Try_Play(Result))
            {
                _placed_chips = _placed_chips.Union(_placement_chips).ToArray();
            }
            else
                Reset_TSMI_Click(sender, e);
        }

        private void Reset_TSMI_Click(object sender, EventArgs e)
        {
            foreach (var c in _placement_chips)
                c.Position = new Point(-1);
            Update_PB();
        }
    }
}
