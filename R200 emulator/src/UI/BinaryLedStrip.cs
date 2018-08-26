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
    public partial class BinaryLedStrip : AddressLineUserControl
    {
        protected override Label AddressLabel => lblAddress;
        protected override Label ValueLabel => lblValue;

        public Brush Color = Brushes.Red;

        public BinaryLedStrip() : base()
        {
            InitializeComponent();
        }

        protected override void Repaint(Graphics g)
        {
            var drawingLeft = DrawingLeft;

            UInt32 mask = (UInt32)1 << (Resolution - 1);
            for (int i = 0; i < Resolution; i++)
            {
                var left = drawingLeft + i * ElementMargin + i * Height;
                var brush = (Value & mask) > 0 ? Color : Brushes.LightGray;
                g.FillEllipse(brush, left, 0, Height - 1, Height - 1);
                g.DrawEllipse(Pens.Black, left, 0, Height - 1, Height - 1);

                mask >>= 1;
            }
        }
    }
}
