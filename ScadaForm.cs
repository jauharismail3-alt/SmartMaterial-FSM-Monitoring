// ========================================================================
// SMART MANUFACTURING MONITORING SYSTEM - C# WINDOWS FORMS
// ITS Surabaya - Instrumentation Department
// 
// File: ScadaForm.cs
// Complete Windows Forms implementation
// ========================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace SmartSCADA
{
    public partial class ScadaForm : Form
    {
        // FSM State definitions
        private const string STATE_NORMAL = "00";
        private const string STATE_ALERT = "01";
        private const string STATE_CRITICAL = "10";
        private const string STATE_EMERGENCY = "11";

        private string currentState = STATE_NORMAL;
        private Random random = new Random();
        private Timer signalTimer;

        // UI Components
        private CheckBox[] sensorCheckBoxes;
        private Panel[] actuatorPanels;
        private Label[] actuatorLabels;
        private Label popcountLabel;
        private Label statusLabel;
        private RichTextBox chartTextBox;
        private Button emergencyButton;

        public ScadaForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            StartSignalSimulation();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Smart Manufacturing Monitoring System v2.0";
            this.Size = new Size(1220, 900);
            this.BackColor = ColorTranslator.FromHtml("#0A0E27");
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            this.ResumeLayout(false);
        }

        private void InitializeCustomComponents()
        {
            int yPos = 20;

            // ============================================================
            // HEADER
            // ============================================================
            Label titleLabel = new Label
            {
                Text = "Smart Manufacturing Monitoring System v2.0",
                Location = new Point(20, yPos),
                Size = new Size(900, 35),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#00D9FF"),
                BackColor = Color.Transparent
            };
            this.Controls.Add(titleLabel);
            yPos += 40;

            Label subtitleLabel = new Label
            {
                Text = "ITS SURABAYA - INSTRUMENTATION DEPT.",
                Location = new Point(20, yPos),
                Size = new Size(800, 20),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#8B9DC3"),
                BackColor = Color.Transparent
            };
            this.Controls.Add(subtitleLabel);
            yPos += 40;

            // ============================================================
            // PRODUCTION STATUS PANEL
            // ============================================================
            Panel statusPanel = CreatePanel(20, yPos, 1160, 140, ColorTranslator.FromHtml("#1A1F3A"));

            Label productionLabel = new Label
            {
                Text = "PRODUCTION RUNNING",
                Location = new Point(380, 15),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#00FFB3"),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };
            statusPanel.Controls.Add(productionLabel);

            Panel separator = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(1120, 2),
                BackColor = ColorTranslator.FromHtml("#00D9FF")
            };
            statusPanel.Controls.Add(separator);

            Label pcLabel = new Label
            {
                Text = "Active Sensors Count:",
                Location = new Point(380, 75),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#DFE6F0"),
                BackColor = Color.Transparent
            };
            statusPanel.Controls.Add(pcLabel);

            popcountLabel = new Label
            {
                Text = "0",
                Location = new Point(590, 75),
                Size = new Size(50, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#FFD93D"),
                BackColor = Color.Transparent
            };
            statusPanel.Controls.Add(popcountLabel);

            Label stLabel = new Label
            {
                Text = "System State:",
                Location = new Point(420, 105),
                Size = new Size(130, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#DFE6F0"),
                BackColor = Color.Transparent
            };
            statusPanel.Controls.Add(stLabel);

            statusLabel = new Label
            {
                Text = "NORMAL (00)",
                Location = new Point(560, 105),
                Size = new Size(180, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#00FFB3"),
                BackColor = Color.Transparent
            };
            statusPanel.Controls.Add(statusLabel);

            this.Controls.Add(statusPanel);
            yPos += 160;

            // ============================================================
            // EMERGENCY RESET BUTTON
            // ============================================================
            emergencyButton = new Button
            {
                Text = "EMERGENCY RESET",
                Location = new Point(20, yPos),
                Size = new Size(1160, 50),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#E63946"),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            emergencyButton.FlatAppearance.BorderSize = 0;
            emergencyButton.Click += EmergencyButton_Click;
            this.Controls.Add(emergencyButton);
            yPos += 70;

            // ============================================================
            // SENSOR INPUTS PANEL
            // ============================================================
            Panel sensorPanel = CreatePanel(20, yPos, 565, 270, ColorTranslator.FromHtml("#1A1F3A"));

            Label sensorTitle = new Label
            {
                Text = "SENSOR INPUTS",
                Location = new Point(15, 15),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#00D9FF"),
                BackColor = Color.Transparent
            };
            sensorPanel.Controls.Add(sensorTitle);

            string[] sensorNames = {
                "S1: Strain Gauge (Regangan)",
                "S2: Load Cell (Gaya Tekan)",
                "S3: Accelerometer (Getaran)",
                "S4: Temperature Sensor (Suhu)",
                "S5: Hall Effect (Medan Magnet)",
                "S6: LVDT (Pergeseran)"
            };

            sensorCheckBoxes = new CheckBox[6];
            for (int i = 0; i < 6; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Text = sensorNames[i],
                    Location = new Point(25, 55 + i * 35),
                    Size = new Size(520, 25),
                    Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                    ForeColor = ColorTranslator.FromHtml("#DFE6F0"),
                    BackColor = Color.Transparent
                };
                cb.CheckedChanged += SensorCheckBox_CheckedChanged;
                sensorCheckBoxes[i] = cb;
                sensorPanel.Controls.Add(cb);
            }

            this.Controls.Add(sensorPanel);

            // ============================================================
            // ACTUATOR STATUS PANEL
            // ============================================================
            Panel actuatorPanel = CreatePanel(615, yPos, 565, 270, ColorTranslator.FromHtml("#1A1F3A"));

            Label actuatorTitle = new Label
            {
                Text = "ACTUATOR STATUS",
                Location = new Point(15, 15),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#00D9FF"),
                BackColor = Color.Transparent
            };
            actuatorPanel.Controls.Add(actuatorTitle);

            string[] actuatorNames = {
                "A1: Linear Actuator (Regangan)",
                "A2: Servo Motor (Gaya/Rotasi)",
                "A3: Voice Coil (Getaran)",
                "A4: Peltier/Heater (Suhu)",
                "A5: Electromagnet (Medan)",
                "A6: Solenoid Actuator (Displacement)"
            };

            actuatorPanels = new Panel[6];
            actuatorLabels = new Label[6];

            for (int i = 0; i < 6; i++)
            {
                Panel actPanel = new Panel
                {
                    Location = new Point(15, 50 + i * 35),
                    Size = new Size(535, 30),
                    BackColor = ColorTranslator.FromHtml("#2A2F4A"),
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label actLabel = new Label
                {
                    Text = actuatorNames[i],
                    Location = new Point(10, 5),
                    Size = new Size(515, 20),
                    Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                    ForeColor = ColorTranslator.FromHtml("#6B7280"),
                    BackColor = Color.Transparent
                };

                actPanel.Controls.Add(actLabel);
                actuatorPanels[i] = actPanel;
                actuatorLabels[i] = actLabel;
                actuatorPanel.Controls.Add(actPanel);
            }

            this.Controls.Add(actuatorPanel);
            yPos += 290;

            // ============================================================
            // SIGNAL MONITORING CHART
            // ============================================================
            Panel chartPanel = CreatePanel(20, yPos, 1160, 290, ColorTranslator.FromHtml("#1A1F3A"));

            Label chartTitle = new Label
            {
                Text = "SIGNAL MONITORING",
                Location = new Point(470, 15),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#00D9FF"),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter
            };
            chartPanel.Controls.Add(chartTitle);

            // Legend
            int legendX = 300;
            CreateLegendItem(chartPanel, legendX, 50, ColorTranslator.FromHtml("#00FFB3"), "Linear Actuator");
            CreateLegendItem(chartPanel, legendX + 200, 50, ColorTranslator.FromHtml("#FFD93D"), "Servo Motor");
            CreateLegendItem(chartPanel, legendX + 400, 50, ColorTranslator.FromHtml("#FF6B9D"), "Voice Coil");

            // Chart area
            chartTextBox = new RichTextBox
            {
                Location = new Point(20, 90),
                Size = new Size(1120, 180),
                Font = new Font("Consolas", 10F, FontStyle.Regular),
                BackColor = ColorTranslator.FromHtml("#0F1419"),
                ForeColor = ColorTranslator.FromHtml("#8B9DC3"),
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.None
            };
            chartPanel.Controls.Add(chartTextBox);

            this.Controls.Add(chartPanel);

            // Initialize system
            UpdateSystemState();
        }

        private Panel CreatePanel(int x, int y, int width, int height, Color bgColor)
        {
            return new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = bgColor,
                BorderStyle = BorderStyle.None
            };
        }

        private void CreateLegendItem(Panel parent, int x, int y, Color color, string text)
        {
            Panel colorBox = new Panel
            {
                Location = new Point(x, y + 7),
                Size = new Size(25, 3),
                BackColor = color
            };
            parent.Controls.Add(colorBox);

            Label label = new Label
            {
                Text = text,
                Location = new Point(x + 30, y),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = color,
                BackColor = Color.Transparent
            };
            parent.Controls.Add(label);
        }

        private void StartSignalSimulation()
        {
            signalTimer = new Timer { Interval = 500 };
            signalTimer.Tick += SignalTimer_Tick;
            signalTimer.Start();
        }

        private void SignalTimer_Tick(object sender, EventArgs e)
        {
            UpdateSignalVisualization();
        }

        private void UpdateSignalVisualization()
        {
            string linearSignal, servoSignal, voiceCoilSignal;

            switch (currentState)
            {
                case STATE_EMERGENCY:
                    linearSignal = GenerateSignalPattern(8, 10);
                    servoSignal = GenerateSignalPattern(9, 10);
                    voiceCoilSignal = GenerateSignalPattern(8, 10);
                    break;
                case STATE_CRITICAL:
                    linearSignal = GenerateSignalPattern(5, 8);
                    servoSignal = GenerateSignalPattern(6, 9);
                    voiceCoilSignal = GenerateSignalPattern(4, 7);
                    break;
                case STATE_ALERT:
                    linearSignal = GenerateSignalPattern(3, 5);
                    servoSignal = GenerateSignalPattern(3, 6);
                    voiceCoilSignal = GenerateSignalPattern(1, 3);
                    break;
                default:
                    linearSignal = GenerateSignalPattern(1, 3);
                    servoSignal = GenerateSignalPattern(1, 3);
                    voiceCoilSignal = GenerateSignalPattern(0, 1);
                    break;
            }

            chartTextBox.Text = $"Linear Actuator: {linearSignal}\n" +
                               $"Servo Motor:     {servoSignal}\n" +
                               $"Voice Coil:      {voiceCoilSignal}";
        }

        private string GenerateSignalPattern(int minLevel, int maxLevel)
        {
            char[] blocks = { '▁', '▂', '▃', '▄', '▅', '▆', '▇', '█' };
            string pattern = "";

            for (int i = 0; i < 40; i++)
            {
                int level = random.Next(minLevel, maxLevel + 1);
                if (level == 0)
                    pattern += " ";
                else if (level >= blocks.Length)
                    pattern += blocks[blocks.Length - 1];
                else
                    pattern += blocks[level];
            }
            return pattern;
        }

        private void SensorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSystemState();
        }

        private void UpdateSystemState()
        {
            int popcount = sensorCheckBoxes.Count(cb => cb.Checked);
            popcountLabel.Text = popcount.ToString();

            string newState, stateText;
            Color stateColor;

            if (popcount == 6)
            {
                newState = STATE_EMERGENCY;
                stateText = "EMERGENCY (11)";
                stateColor = ColorTranslator.FromHtml("#FF6B9D");
            }
            else if (popcount >= 4)
            {
                newState = STATE_CRITICAL;
                stateText = "CRITICAL (10)";
                stateColor = ColorTranslator.FromHtml("#FF9E64");
            }
            else if (popcount >= 2)
            {
                newState = STATE_ALERT;
                stateText = "ALERT (01)";
                stateColor = ColorTranslator.FromHtml("#FFD93D");
            }
            else
            {
                newState = STATE_NORMAL;
                stateText = "NORMAL (00)";
                stateColor = ColorTranslator.FromHtml("#00FFB3");
            }

            currentState = newState;
            statusLabel.Text = stateText;
            statusLabel.ForeColor = stateColor;

            UpdateActuators();
        }

        private void UpdateActuators()
        {
            // Reset all actuators
            for (int i = 0; i < 6; i++)
            {
                actuatorPanels[i].BackColor = ColorTranslator.FromHtml("#2A2F4A");
                actuatorLabels[i].ForeColor = ColorTranslator.FromHtml("#6B7280");
            }

            // Set actuators based on state
            switch (currentState)
            {
                case STATE_EMERGENCY:
                    SetActuatorState(0, "#5A1A2C", "#FF6B9D");
                    SetActuatorState(3, "#5A1A2C", "#FF6B9D");
                    SetActuatorState(5, "#5A1A2C", "#FF6B9D");
                    break;
                case STATE_CRITICAL:
                    SetActuatorState(1, "#5A3A1A", "#FF9E64");
                    SetActuatorState(2, "#5A3A1A", "#FF9E64");
                    SetActuatorState(5, "#5A3A1A", "#FF9E64");
                    break;
                case STATE_ALERT:
                    SetActuatorState(4, "#5A4F1A", "#FFD93D");
                    SetActuatorState(5, "#5A4F1A", "#FFD93D");
                    break;
                case STATE_NORMAL:
                    SetActuatorState(0, "#1A4A3A", "#00FFB3");
                    SetActuatorState(4, "#1A3F5A", "#00D9FF");
                    break;
            }

            // Individual sensor mapping
            for (int i = 0; i < sensorCheckBoxes.Length && i < actuatorPanels.Length; i++)
            {
                if (sensorCheckBoxes[i].Checked)
                {
                    if (actuatorLabels[i].ForeColor == ColorTranslator.FromHtml("#6B7280"))
                    {
                        SetActuatorState(i, "#1A3F5A", "#00D9FF");
                    }
                }
            }
        }

        private void SetActuatorState(int index, string bgColor, string textColor)
        {
            actuatorPanels[index].BackColor = ColorTranslator.FromHtml(bgColor);
            actuatorLabels[index].ForeColor = ColorTranslator.FromHtml(textColor);
        }

        private void EmergencyButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to reset all sensors and return to NORMAL state?",
                "Emergency Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                foreach (var cb in sensorCheckBoxes)
                    cb.Checked = false;

                UpdateSystemState();

                MessageBox.Show(
                    "System has been successfully reset to NORMAL state.\n\nPOPCOUNT: 0\nSTATE: NORMAL (00)",
                    "Reset Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

    }
}