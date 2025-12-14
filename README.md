🏭 Smart Manufacturing Monitoring System
Windows Forms SCADA Simulation – C#

Department of Instrumentation Engineering
Institut Teknologi Sepuluh Nopember (ITS), Surabaya

📌 Overview

Smart Manufacturing Monitoring System adalah aplikasi SCADA (Supervisory Control and Data Acquisition) berbasis C# Windows Forms yang mensimulasikan pemantauan sensor dan aktuator pada sistem manufaktur cerdas.

Aplikasi ini mengimplementasikan:

Finite State Machine (FSM)

POPCOUNT logic untuk evaluasi kondisi sistem

Visual monitoring sensor–aktuator secara real-time

Signal visualization berbasis ASCII waveform

Proyek ini dirancang untuk kebutuhan pembelajaran Sistem Digital, Instrumentasi, dan Smart Manufacturing.

🎯 Features
🔹 Sensor Monitoring

6 jenis sensor industri:

Strain Gauge

Load Cell

Accelerometer

Temperature Sensor

Hall Effect Sensor

LVDT

Input sensor menggunakan checkbox (simulasi digital)

🔹 FSM-Based System State

Sistem menggunakan 2-bit FSM:

State	Binary	Description
NORMAL	00	Operasi stabil
ALERT	01	Peringatan awal
CRITICAL	10	Kondisi kritis
EMERGENCY	11	Keadaan darurat

Penentuan state berdasarkan jumlah sensor aktif (POPCOUNT).

🔹 Actuator Control Logic

6 aktuator industri:

Linear Actuator

Servo Motor

Voice Coil

Heater / Peltier

Electromagnet

Solenoid

Status aktuator berubah otomatis berdasarkan:

FSM state

Sensor yang aktif

🔹 Signal Visualization

Real-time signal monitoring menggunakan ASCII waveform

Pola sinyal berubah sesuai kondisi sistem:

Low noise → NORMAL

High amplitude → EMERGENCY

Update setiap 500 ms

🔹 Emergency Reset

Tombol EMERGENCY RESET

Mengembalikan sistem ke:

POPCOUNT = 0

STATE = NORMAL (00)

Dilengkapi dialog konfirmasi dan notifikasi

🧠 System Logic Summary
Sensor Inputs → POPCOUNT → FSM State → Actuator Response → Signal Pattern


FSM Transition:

0–1 sensor → NORMAL

2–3 sensor → ALERT

4–5 sensor → CRITICAL

6 sensor → EMERGENCY

🖥️ User Interface

Modern SCADA-style dark theme

Color-coded states:

🟢 Green → NORMAL

🟡 Yellow → ALERT

🟠 Orange → CRITICAL

🔴 Red → EMERGENCY

Responsive layout (Fixed window)

🛠️ Technologies Used

Language: C#

Framework: .NET Windows Forms

Concepts:

Finite State Machine (FSM)

Digital Logic (POPCOUNT)

Event-driven programming

Industrial instrumentation simulation

📂 Project Structure
SmartSCADA/
│
├── ScadaForm.cs        # Main SCADA UI & logic
├── Program.cs         # Application entry point
├── SmartSCADA.csproj  # Project configuration
└── README.md          # Project documentation

🚀 How to Run

Clone repository:

git clone https://github.com/your-username/SmartSCADA.git


Open project using Visual Studio

Ensure target framework supports Windows Forms

Run:

Ctrl + F5

📚 Academic Context

Project ini relevan untuk mata kuliah:

Sistem Digital

Instrumentasi Industri

SCADA & Automation

Smart Manufacturing Systems

👨‍🎓 Author

Mohammad Eka Jauhar Ismail
Department of Instrumentation Engineering
Institut Teknologi Sepuluh Nopember (ITS) – Surabaya

📜 License

This project is intended for educational and academic use.
Feel free to modify and extend for learning purposes.
