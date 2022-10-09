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
        private const int CHIP_BG_SIZE = 256;
        private const int CHIP_BG_BORDER = 6;
        private const int CHIP_SIZE = 256;
        private const int CHIP_BORDER = 3;

        private Bitmap[] L_Chips;
        private Bitmap[] T_Chips;
        private Bitmap[] I_Chips;
        private Bitmap[] Nodes;
        private Bitmap Obstruction;
        private Bitmap Chip_BG;

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


        private Color _placed_color = Color.GreenYellow;
        private Color _not_placed_color = Color.Yellow;
        private int _hover_chip_id;
        private Connection_Chip _hover_chip;

        private GameObject[] _all => 
            _nodes.Cast<GameObject>()
            .Union(_placement_chips.Cast<GameObject>())
            .Union(_placed_chips.Cast<GameObject>())
            .Union(_obstructions.Cast<GameObject>())
            .OrderBy(o=>-o.Y).ThenBy(o=>-o.X).ToArray();

        private Connection_Chip[] _all_chips =>
            _nodes.Cast<Connection_Chip>()
            .Union(_placement_chips.Cast<Connection_Chip>())
            .Union(_placed_chips.Cast<Connection_Chip>())
            .OrderBy(o => -o.Y).ThenBy(o => -o.X).ToArray();

        private Button[] _buttons;
        public Human_Form(Human_Player player)
        {
            InitializeComponent();
            _buttons = new Button[] { button1, button2, button3 };
            _hover_chip = null;
            _hover_chip_id = -1;
            _player = player;
        }

        private void Init_ChipsImages()
        {
            Chip_BG = new Bitmap(Resources.Chip_BG);
            Obstruction = new Bitmap(Resources.Chip_BG);

            L_Chips = new Bitmap[4] { new Bitmap(Resources.L_0), new Bitmap(Resources.L_90), new Bitmap(Resources.L_180), new Bitmap(Resources.L_270)};
            T_Chips = new Bitmap[4] { new Bitmap(Resources.T_0), new Bitmap(Resources.T_90), new Bitmap(Resources.T_180), new Bitmap(Resources.T_270) };
            I_Chips = new Bitmap[4] { new Bitmap(Resources.I_0), new Bitmap(Resources.I_90), new Bitmap(Resources.I_0), new Bitmap(Resources.I_90), };
            Nodes = new Bitmap[4] { new Bitmap(Resources.Node_0), new Bitmap(Resources.Node_90), new Bitmap(Resources.Node_180), new Bitmap(Resources.Node_270), };
        }

        public void Get_Pack(Chips_Pack pack)
        {
            _placement_chips = new Connection_Chip[Game_Controller.CHIPS_PACK_SIZE];
            for (int i = 0; i < Game_Controller.CHIPS_PACK_SIZE; i++)
            {
                switch (pack.Chips[i])
                {
                    case Chips_Type.I_Chip:

                        _placement_chips[i] = new I_Chip();
                        _buttons[i].BackgroundImage = I_Chips[0];
                        break;
                    case Chips_Type.L_Chip:
                        _placement_chips[i] = new L_Chip();
                        _buttons[i].BackgroundImage = L_Chips[0];
                        break;
                    case Chips_Type.T_Chip:
                        _placement_chips[i] = new T_Chip();
                        _buttons[i].BackgroundImage = T_Chips[0];
                        break;
                }
            }
            Reset_TSMI_Click(this, EventArgs.Empty);

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

        private void Update_PB()
        {
            using (var g = Graphics.FromImage(Map_PB.Image))
            {
                g.DrawImage(_map, Point.Empty);
                SolidBrush b = new SolidBrush(Color.White);
                SolidBrush b2 = new SolidBrush(Color.Black);

                int chip_bg_border = (int)Math.Ceiling(CHIP_BG_BORDER * ((double)_cell_size / CHIP_BG_SIZE));
                int chip_border = (int)Math.Ceiling((CHIP_BORDER * ((double)_cell_size / CHIP_SIZE)));

                // Draw chips bg
                foreach (var gameoOject in _all)
                {
                    if (gameoOject.X == -1 && gameoOject.Y == -1)
                        continue;
                    Point start = new Point(gameoOject.X * _cell_size - chip_bg_border, gameoOject.Y * _cell_size - chip_bg_border);
                    g.DrawImage(Get_BG_Image(gameoOject), start.X, start.Y, _cell_size + chip_bg_border * 2, _cell_size + chip_bg_border * 2);
                }

                // Draw chip
                foreach (var chip in _all_chips)
                {
                    if (chip.X == -1 && chip.Y == -1)
                        continue;
                    Point start = new Point(chip.X * _cell_size - chip_border, chip.Y * _cell_size - chip_border);
                    g.DrawImage(Get_Chip_Image(chip), start.X, start.Y, _cell_size + chip_border * 2, _cell_size + chip_border * 2);
                }
            }
            Map_PB.Refresh();
        }

        private Bitmap Get_BG_Image(GameObject gameObject)
        {
            Bitmap result = new Bitmap(Chip_BG);

            GameObject up = _all.FirstOrDefault(o => o.X == gameObject.X && o.Y == gameObject.Y - 1);
            GameObject down = _all.FirstOrDefault(o => o.X == gameObject.X && o.Y == gameObject.Y + 1);
            GameObject right = _all.FirstOrDefault(o => o.Y == gameObject.Y && o.X == gameObject.X + 1);
            GameObject left = _all.FirstOrDefault(o => o.Y == gameObject.Y && o.X == gameObject.X - 1);

            GameObject left_top = _all.FirstOrDefault(o => o.X == gameObject.X - 1 && o.Y == gameObject.Y - 1);
            GameObject right_top = _all.FirstOrDefault(o => o.X == gameObject.X + 1 && o.Y == gameObject.Y - 1);
            GameObject left_down = _all.FirstOrDefault(o => o.X == gameObject.X - 1 && o.Y == gameObject.Y + 1);
            GameObject right_down = _all.FirstOrDefault(o => o.X == gameObject.X + 1 && o.Y == gameObject.Y + 1);

            // Cut bg
            Pen eracer = new Pen(Color.Transparent, CHIP_BG_BORDER * 2);
            SolidBrush eracer_b = new SolidBrush(Color.Transparent);
            using (var bg_graphics = Graphics.FromImage(result))
            {
                // Cut sides
                bg_graphics.CompositingMode = CompositingMode.SourceCopy;
                if (up != null)
                    bg_graphics.DrawLine(eracer, 0, 0, CHIP_BG_SIZE, 0);
                if (down != null)
                    bg_graphics.DrawLine(eracer, 0, CHIP_BG_SIZE, CHIP_BG_SIZE, CHIP_BG_SIZE);

                if (right != null)
                    bg_graphics.DrawLine(eracer, CHIP_BG_SIZE, 0, CHIP_BG_SIZE, CHIP_BG_SIZE);
                if (left != null)
                    bg_graphics.DrawLine(eracer, 0, 0, 0, CHIP_BG_SIZE);

                // Cut corners
                if (left_top != null)
                    bg_graphics.FillRectangle(eracer_b, 0, 0, CHIP_BG_BORDER, CHIP_BG_BORDER);
                if (right_top != null)
                    bg_graphics.FillRectangle(eracer_b, CHIP_BG_SIZE - CHIP_BG_BORDER, 0, CHIP_BG_BORDER, CHIP_BG_BORDER);

                if (left_down != null)
                    bg_graphics.FillRectangle(eracer_b, 0, CHIP_BG_SIZE - CHIP_BG_BORDER, CHIP_BG_BORDER, CHIP_BG_BORDER);
                if (right_down != null)
                    bg_graphics.FillRectangle(eracer_b, CHIP_BG_SIZE - CHIP_BG_BORDER, CHIP_BG_SIZE - CHIP_BG_BORDER, CHIP_BG_BORDER, CHIP_BG_BORDER);
            }

            return result;
        }

        private Bitmap Get_Chip_Image(Connection_Chip chip)
        {
            Bitmap chip_image;
            if (chip is L_Chip)
                chip_image = new Bitmap(L_Chips[(int)chip.Rotation]);
            else if (chip is T_Chip)
                chip_image = new Bitmap(T_Chips[(int)chip.Rotation]);
            else if (chip is I_Chip)
                chip_image = new Bitmap(I_Chips[(int)chip.Rotation]);
            else if (chip is Connection_Node)
                chip_image = new Bitmap(Nodes[(int)chip.Rotation]);
            else
                throw new IndexOutOfRangeException($"Type {chip.GetType()} dont support chip image!");

            GameObject up = _all.FirstOrDefault(o => o.X == chip.X && o.Y == chip.Y - 1);
            GameObject down = _all.FirstOrDefault(o => o.X == chip.X && o.Y == chip.Y + 1);
            GameObject right = _all.FirstOrDefault(o => o.Y == chip.Y && o.X == chip.X + 1);
            GameObject left = _all.FirstOrDefault(o => o.Y == chip.Y && o.X == chip.X - 1);

            // Cut chip
            Pen eracer = new Pen(Color.Transparent, CHIP_BORDER * 2);
            using (var chip_graphics = Graphics.FromImage(chip_image))
            {
                chip_graphics.CompositingMode = CompositingMode.SourceCopy;
                if (up is Connection_Chip u_chip && u_chip.Connections.Contains(Direction.Bottom))
                    chip_graphics.DrawLine(eracer, 0, 0, CHIP_SIZE, 0);
                if (down is Connection_Chip d_chip && d_chip.Connections.Contains(Direction.Top))
                    chip_graphics.DrawLine(eracer, 0, CHIP_SIZE, CHIP_SIZE, CHIP_SIZE);

                if (left is Connection_Chip l_chip && l_chip.Connections.Contains(Direction.Right))
                    chip_graphics.DrawLine(eracer, 0, 0, 0, CHIP_SIZE);
                if (right is Connection_Chip r_chip && r_chip.Connections.Contains(Direction.Left))
                    chip_graphics.DrawLine(eracer, CHIP_SIZE, 0, CHIP_SIZE, CHIP_SIZE);
            }

            return chip_image;
        }

        private void Draw_Map()
        {
            using (var g = Graphics.FromImage(_map))
            {
                g.Clear(Color.White);

                Pen p = new Pen(Color.White, 2);
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
                {
                    _buttons[_hover_chip_id].BackColor = _placed_color;
                    _hover_chip = null;
                    _hover_chip_id = -1;
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                if (_hover_chip != null)
                    _hover_chip.Position = new Point(-1);
                _hover_chip_id = int.Parse(b.Tag.ToString());
                _hover_chip = _placement_chips[_hover_chip_id];
                b.BackColor = _not_placed_color;
            }
        }

        private void Map_PB_MouseMove(object sender, MouseEventArgs e)
        {
            if (_hover_chip is null)
                return;

            int mouse_x = e.X;
            int mouse_y = e.Y;
            mouse_x = mouse_x > 0 ? mouse_x / _cell_size : 0;
            mouse_y = mouse_y > 0 ? mouse_y / _cell_size : 0;
            Point mouse_pos = new Point(mouse_x, mouse_y);

            if (_hover_chip.Position == mouse_pos)
                return;

            foreach (var gameObject in _all)
                if (gameObject.Position == mouse_pos)
                    return;

            _hover_chip.Position = mouse_pos;

            Invoke((MethodInvoker) Update_PB);
        }

        private void Accept_TSMI_Click(object sender, EventArgs e)
        {
            Round_Result result = new Round_Result(_player, _placement_chips);
            Connection_Chip[] temp = _placement_chips;
            if (_player.Try_Play(result))
            {
                _placed_chips = _placed_chips.Union(temp).ToArray();
                Update_PB();
            }
            else
                Reset_TSMI_Click(sender, e);
        }

        private void Reset_TSMI_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Game_Controller.CHIPS_PACK_SIZE; i++)
            {
                _buttons[i].BackColor = _not_placed_color;
                _placement_chips[i].Position = new Point(-1);
            }
            Update_PB();
        }
    }
}
