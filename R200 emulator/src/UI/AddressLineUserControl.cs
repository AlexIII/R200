using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace remu
{
    /// <summary>
    /// Base class for controls that represent one address element of any kind of memory 
    /// </summary>
    public class AddressLineUserControl : UserControl
    {
        private UInt32 _value;
        private byte _resolution = 8;

        protected virtual Label AddressLabel { get; }
        protected virtual Label ValueLabel { get; }

        protected int DrawingLeft => ValueLabel.Left + ValueLabel.Width + 20;
        protected const int ElementMargin = 2;

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
    }
}
