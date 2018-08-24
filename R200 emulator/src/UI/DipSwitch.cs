using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace remu
{
    public partial class DipSwitch : AddressLineUserControl
    {
        protected override Label AddressLabel => lblAddress;
        protected override Label ValueLabel => lblValue;

        public DipSwitch()
        {
            InitializeComponent();
        }

        private void DipSwitch_Paint(object sender, PaintEventArgs e)
        {
            var padding = 2;
            var width = Height / 2;
            var height = (Height - 4) / 3;
            var drawingLeft = DrawingLeft;

            using (var g = e.Graphics)
            {
                UInt32 mask = (UInt32)1 << (Resolution - 1);
                for (int i = 0; i < Resolution; i++)
                {
                    var left = drawingLeft + i * padding + i * Height;
                    g.FillRectangle(Brushes.DarkRed, left, 0, Height - 1, Height - 1);
                    g.FillRectangle(Brushes.White, left + width / 2, (Value & mask) > 0 ? 2 : Height - 3 - height, width, height);
      
                    mask >>= 1;
                }
            }
      
            Width = drawingLeft + (Resolution - 1) * padding + Resolution * Height;
        }

        private void DipSwitch_MouseDown(object sender, MouseEventArgs e)
        {
            var padding = 2;
      
            var p = (e.X - DrawingLeft) / (Height + padding);
            if (p >= 0 && p < Resolution)
            {
                p = Resolution - p - 1;
                var mask = (UInt32)1 << p;
                Value ^= mask;
                Invalidate();
            }
        }
    }
}
