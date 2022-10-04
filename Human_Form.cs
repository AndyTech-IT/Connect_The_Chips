using Connect_The_Chips.Game;
using Connect_The_Chips.Game.Chips;
using Connect_The_Chips.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect_The_Chips.Players
{
    public partial class Human_Form : Form
    {
        public Round_Result Result => Result;
        private Round_Result _result;

        private int _cell_size;
        private int _map_border;
        private Size _image_size;
        private int _image_height => _image_size.Height;
        private int _image_width => _image_size.Width;

        private Size _mesh_size;
        private int _mesh_height => _mesh_size.Height;
        private int _mesh_width => _mesh_size.Width;

        public Image Resource { get; private set; }

        private Image _map;
        private Connection_Chip[] _placement_chips;
        private Connection_Chip _hover_chip;

        private Button[] _buttons;
        public Human_Form()
        {
            InitializeComponent();
            _buttons = new Button[] { button1, button2, button3 };
            _map_border = 2;
            _hover_chip = null;
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
            _image_size = new Size(_cell_size * _mesh_size.Width + _map_border * 2, _cell_size * _mesh_size.Height + _map_border * 2);

            _map = new Bitmap(_image_width, _image_width);
            Map_PB.Image = new Bitmap(_image_width, _image_width);

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
                foreach(var chip in _placement_chips)
                {
                    if (chip.X == -1 && chip.Y == -1)
                        continue;

                    g.DrawImage(Chip_Image(chip), _map_border + chip.X * _cell_size, _map_border + chip.Y * _cell_size, _cell_size + 5, _cell_size + 5);
                }
            }
            Map_PB.Refresh();
        }

        private Image Chip_Image(Connection_Chip chip)
        {
            Image image;
            switch (chip.Rotation)
            {
                case Rotation.Degree_0:
                    image = new Bitmap(Resources.L_Chip_0);
                    break;
                case Rotation.Degree_90:
                    image = new Bitmap(Resources.L_Chip_90);
                    break;
                case Rotation.Degree_180:
                    image = new Bitmap(Resources.L_Chip_180);
                    break;
                case Rotation.Degree_270:
                    image = new Bitmap(Resources.L_Chip_270);
                    break;
                default:
                    return null;
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
                    g.DrawLine(p, _map_border, _map_border + y * _cell_size, _image_width - _map_border, y * _cell_size + _map_border);
                for (int x = 1; x < _mesh_width; x++)
                    g.DrawLine(p, _map_border + x * _cell_size, _map_border, _map_border + x * _cell_size, _image_height - _map_border);

                p = new Pen(Color.LightGray, _map_border * 2);
                g.DrawLine(p, _image_width, _image_height, _image_width, 0);
                g.DrawLine(p, _image_width, _image_height, 0, _image_height);

                p = new Pen(Color.Gray, _map_border*2);
                g.DrawLine(p, 0, 0, _image_width, 0);
                g.DrawLine(p, 0, 0, 0, _image_height);

            }
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

            _hover_chip.Position = new Point(mouse_x, mouse_y);

            Invoke((MethodInvoker) Update_PB);
        }
    }
}
