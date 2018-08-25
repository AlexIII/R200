using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace remu
{
    public partial class MainForm : Form
    {
        private Remulator _remu;
        private MainForm()
        {
            InitializeComponent();
        }

        public MainForm(Remulator remu)
        {
            _remu = remu;
            _remu.StepCompleted += Remulator_StepCompleted;

            InitializeComponent();
        }

        private void InitializeEmulatorControls()
        {
            flpRamPanel.Controls.Add(
                    new BinaryLedStrip()
                    {
                        Address = "RA",
                        Value = 0,
                        Resolution = 12,
                    }
            );
            flpRamPanel.Controls.Add(
                    new BinaryLedStrip()
                    {
                        Address = "RB",
                        Value = 0,
                        Resolution = 12,
                    }
            );

            for (int i = 0; i < Remulator.ramSize; i++)
            {
                flpRamPanel.Controls.Add(
                    new BinaryLedStrip() {
                        Address = i.ToString("X2"),
                        Value = 0,
                        Resolution = 12,
                        color = Brushes.Green
                    }
                );
            }

            var cmem = _remu.ConstMemory;
            for (int i = 0; i < Remulator.constMemSize; i++)
            {
                
                flpConst.Controls.Add(
                    new DipSwitch() {
                        Address = i.ToString("X2"),
                        Value = cmem[i],
                        Resolution = 12,
                    }
                );
            }

            // TODO: initialize rom in flpROM
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lock (this)
            {
                if (flpRamPanel.Controls.Count == 0)
                    InitializeEmulatorControls();
            }
        }

        private void Remulator_StepCompleted(object sender, EventArgs e)
        {
            // This handler is called from another thread, so we must invoke action instead of simply calling it
            Invoke(new Action(() =>
            {
                // Checking it synchronized since we have race condition here
                lock (this)
                {
                    if (flpRamPanel.Controls.Count == 0)
                        InitializeEmulatorControls();
                }

                (flpRamPanel.Controls[0] as BinaryLedStrip).Value = _remu.state.RA;
                (flpRamPanel.Controls[1] as BinaryLedStrip).Value = _remu.state.RB;

                for (int i = 0; i < Remulator.ramSize; i++)
                    (flpRamPanel.Controls[i+2] as BinaryLedStrip).Value = _remu.state.RAM[i];
            }));
        }
    }
}
