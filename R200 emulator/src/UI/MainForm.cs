using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace remu
{
    public partial class MainForm : Form
    {
        public enum EmulatorState { Execution, StepByStep, Pause, Stop };

        private Remulator _remu;
        Preprocessor.ProcProg _preprocessedProgram;
        bool _needToShowCurrentLineOfSourceCode;

        private EmulatorState _emulatorState = EmulatorState.Stop;
        private int _executionSpeed = 0;
        private BackgroundWorker _emulatorThread = new BackgroundWorker();

        private MainForm()
        {
            InitializeComponent();
        }

        public MainForm(Remulator remu, Preprocessor.ProcProg preprocessedProgram, bool run = false)
        {
            _remu = remu;
            _preprocessedProgram = preprocessedProgram;

            _emulatorThread.DoWork += EmulatorThread_DoWork;
            _emulatorThread.RunWorkerAsync(); // starting it now. However, real operation will start only on Run button press (see DoWork handler)
            _remu.StepCompleted += Remulator_StepCompleted;

            InitializeComponent();

            if (run)
                btnRun_Click(btnRun10x, EventArgs.Empty);
        }

        private void EmulatorThread_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!_remu.halt && (_emulatorState == EmulatorState.Execution || _emulatorState == EmulatorState.StepByStep))
                    _remu.step();
                else
                    Thread.Sleep(100); // wait for Run button press
            }
        }

        private void InitializeEmulatorControls()
        {
            for (int i = 0; i < Remulator.ramSize; i++)
            {
                flpRamPanel.Controls.Add(
                    new BinaryLedStrip() {
                        Address = i.ToString("X2"),
                        Value = 0,
                        Resolution = 12,
                        Color = Brushes.Green,
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
                        Height= 17,
                    }
                );
            }

            flpRegisters.Controls.Add(
                new BinaryLedStrip() {
                    Address = "RA",
                    Value = _remu.state.RA,
                    Resolution = 12,
                }
            );

            flpRegisters.Controls.Add(
                new BinaryLedStrip()
                {
                    Address = "PC  ", // spaces are needed for alignment with "LEAF"
                    Value = _remu.state.PC,
                    Resolution = 6,
                }
            );

            flpRegisters.Controls.Add(
                new BinaryLedStrip()
                {
                    Address = "RB",
                    Value = _remu.state.RB,
                    Resolution = 12,
                }
            );

            flpRegisters.Controls.Add(
                new BinaryLedStrip()
                {
                    Address = "LEAF",
                    Value = _remu.state.LEAF,
                    Resolution = 6,
                }
            );

            // TODO: initialize rom in flpROM
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lock (this)
            {
                if (flpRamPanel.Controls.Count == 0)
                    InitializeEmulatorControls();
            }

            txtSourceCode.Text = _preprocessedProgram.initialSourceCode;
        }

        private void Remulator_StepCompleted(object sender, EventArgs e)
        {
            // Simulate pause button press after each step if we are in step-by-step mode
            if (_emulatorState == EmulatorState.StepByStep)
            {
                _emulatorState = EmulatorState.Pause;
                _needToShowCurrentLineOfSourceCode = true;
                return;
            }

            if (_executionSpeed == 0)
                return;

            _needToShowCurrentLineOfSourceCode = _executionSpeed > 0;

            Thread.Sleep(_executionSpeed);
        }

        private void repaintTimer_Tick(object sender, EventArgs e)
        {
            // Checking it synchronized since we have race condition here
            lock (this)
            {
                if (flpRamPanel.Controls.Count == 0)
                    InitializeEmulatorControls();
            }

            // Updating values. This indirectly executes a control repaint procedure.

            for (int i = 0; i < Remulator.ramSize; i++)
            {
                (flpRamPanel.Controls[i] as BinaryLedStrip).Value = _remu.state.RAM[i];
            }

            // Access items in same order we added them
            (flpRegisters.Controls[0] as BinaryLedStrip).Value = _remu.state.RA;
            (flpRegisters.Controls[1] as BinaryLedStrip).Value = _remu.state.PC;
            (flpRegisters.Controls[2] as BinaryLedStrip).Value = _remu.state.RB;
            (flpRegisters.Controls[3] as BinaryLedStrip).Value = _remu.state.LEAF;

            // Monitoring current execution
            if (_remu.halt && _emulatorState != EmulatorState.Stop)
            {
                // if we should stop because of Halt, theb simulate Stop button click
                btnStop_Click(sender, e);
                _needToShowCurrentLineOfSourceCode = true;
            }

            if (_needToShowCurrentLineOfSourceCode)
            {
                ShowCurrentSourceCodeLine();
                _needToShowCurrentLineOfSourceCode = false;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var speed = int.Parse((sender as ToolStripButton).Tag.ToString());
            _executionSpeed = speed;

            // If this is restart, then repoint PC to first ROM cell
            if (_emulatorState == EmulatorState.Stop)
            {
                _remu.state.PC = 0;
                _remu.halt = false;
            }

            _emulatorState = EmulatorState.Execution;

            var runButtons = new[] { btnRun, btnRun10x, btnRunOnMaximumSpeed };
            foreach (var btn in runButtons)
                btn.Checked = btn == sender;

            btnPause.Checked = false;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (_emulatorState == EmulatorState.Execution || _emulatorState == EmulatorState.StepByStep)
            {
                _emulatorState = EmulatorState.Pause;
                btnPause.Checked = true;
                ShowCurrentSourceCodeLine();
            }
            else if (_emulatorState == EmulatorState.Pause)
            {
                _emulatorState = EmulatorState.Execution;
                btnPause.Checked = false;
            }
        }

        private void ShowCurrentSourceCodeLine()
        {
            // Highlight current source code line
            var lineNumber = _preprocessedProgram.LineNumberRelation[(int)_remu.state.PC];
            var position = txtSourceCode.GetFirstCharIndexFromLine(lineNumber);
            if (position < 0)
            {
                // lineNumber is too big
                txtSourceCode.Select(txtSourceCode.Text.Length, 0);
            }
            else
            {
                int lineEnd = txtSourceCode.Text.IndexOf(Environment.NewLine, position);
                if (lineEnd < 0)
                {
                    lineEnd = txtSourceCode.Text.Length;
                }

                txtSourceCode.Select(position, lineEnd - position);
            }

            txtSourceCode.ScrollToCaret();
            txtSourceCode.Focus();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _emulatorState = EmulatorState.Stop;
            var buttons = new[] { btnRun, btnRun10x, btnRunOnMaximumSpeed, btnPause };
            foreach (var btn in buttons)
                btn.Checked = false;
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            // If this is restart, then repoint PC to first ROM cell
            if (_emulatorState == EmulatorState.Stop)
            {
                _remu.state.PC = 0;
                _remu.halt = false;
            }

            _emulatorState = EmulatorState.StepByStep;
            
            var runButtons = new[] { btnRun, btnRun10x, btnRunOnMaximumSpeed, btnPause };
            foreach (var btn in runButtons)
                btn.Checked = false;

            // We will stop after a moment so we can check Pause button here
            // This is easier that to invoke it in OnStepCompleted callback from another thread.
            btnPause.Checked = true;
        }

        private void txtSourceCode_ScrollPositionChanged(object sender, ScrollEventArgs e)
        {
            // TODO: draw current line arrow and breakpoint circles
            // Text = e.NewValue.ToString();
        }
    }
}
