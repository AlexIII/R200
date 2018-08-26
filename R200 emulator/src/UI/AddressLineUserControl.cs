using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace remu
{
    /// <summary>
    /// Base class for controls that represent one address element of any kind of memory.
    /// It is virtual instead of abstract only to let the desinger work correctly.
    /// </summary>
    public class AddressLineUserControl : UserControl
    {
        private UInt32 _value;
        private byte _resolution = 8;

        protected virtual Label AddressLabel { get; }
        protected virtual Label ValueLabel { get; }
        protected virtual ToolTip ValueToolTip { get; }

        protected int DrawingLeft => ((ValueLabel?.Left + ValueLabel?.Width) ?? 0) + 20;
        protected const int ElementMargin = 2;
        protected const int RightPadding = 32;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.Resize += AddressLineUserControl_Resize;
            base.Paint += AddressLineUserControl_Paint;
        }

        /// <summary>
        /// Number of significant bits in Value
        /// </summary>
        public byte Resolution
        {
            get { return _resolution; }
            set
            {
                _resolution = value;

                // Reserve needed amount of screen space for value label
                ValueLabel.Text = Value.ToString("X" + Math.Ceiling(Resolution / 4M));
            }
        }

        public UInt32 Value
        {
            get { return _value; }
            set
            {
                var needInvalidate = _value != value;
                _value = value;
                ValueLabel.Text = value.ToString("X" + Math.Ceiling(Resolution / 4M));
                ValueToolTip.SetToolTip(ValueLabel, String.Format("DEC: {0} | BIN: {1}", value, Convert.ToString(value, 2)));

                if (needInvalidate)
                    Invalidate();
            }
        }

        /// <summary>
        /// Description of the line (usually, the address)
        /// </summary>
        public string Address
        {
            get { return AddressLabel.Text; }
            set
            {
                AddressLabel.Text = value;
                ValueLabel.Left = AddressLabel.Width + 20;
                Invalidate();
            }
        }

        private void AddressLineUserControl_Resize(object sender, EventArgs e)
        {
            if (AddressLabel != null)
            {
                var fontSize = Height - 6;
                var newFont = new Font(AddressLabel.Font.FontFamily, fontSize, AddressLabel.Font.Style);
                AddressLabel.Font = newFont;
                ValueLabel.Font = newFont;

                Invalidate();
            }
        }

        private void AddressLineUserControl_Paint(object sender, PaintEventArgs e)
        {
            using (var g = e.Graphics)
            {
                g.FillRectangle(new SolidBrush(BackColor), 0, 0, Width - 1, Height - 1);
                Repaint(g);
            }

            Width = DrawingLeft + (Resolution - 1) * ElementMargin + Resolution * Height + RightPadding;
        }

        protected virtual void Repaint(Graphics graphics) { }
    }
}
