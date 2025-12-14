# 🏭 Smart Manufacturing Monitoring System
### Windows Forms SCADA Simulation – C#

**Department of Instrumentation Engineering**  
**Institut Teknologi Sepuluh Nopember (ITS), Surabaya**

---

## 📌 Overview

Smart Manufacturing Monitoring System adalah aplikasi **SCADA (Supervisory Control and Data Acquisition)** berbasis **C# Windows Forms** yang mensimulasikan pemantauan sensor dan aktuator pada sistem manufaktur cerdas.

Aplikasi ini mengimplementasikan **Finite State Machine (FSM)** dan **POPCOUNT logic** untuk menentukan kondisi sistem secara real-time, dilengkapi dengan visualisasi status aktuator serta monitoring sinyal berbasis ASCII waveform.

---

## 🎯 Features

### 🔹 Sensor Monitoring
Terdapat 6 sensor industri yang disimulasikan:
- Strain Gauge (Regangan)
- Load Cell (Gaya Tekan)
- Accelerometer (Getaran)
- Temperature Sensor (Suhu)
- Hall Effect Sensor (Medan Magnet)
- LVDT (Pergeseran)

Input sensor dilakukan menggunakan checkbox sebagai representasi sinyal digital.

---

### 🔹 Finite State Machine (FSM)

Sistem menggunakan FSM 2-bit dengan definisi sebagai berikut:

| State | Binary | Description |
|------|--------|------------|
| NORMAL | 00 | Operasi sistem normal |
| ALERT | 01 | Peringatan awal |
| CRITICAL | 10 | Kondisi kritis |
| EMERGENCY | 11 | Keadaan darurat |

Penentuan state dilakukan berdasarkan **jumlah sensor aktif (POPCOUNT)**.

---

### 🔹 Actuator Control

Sistem mengendalikan 6 aktuator industri:
- Linear Actuator
- Servo Motor
- Voice Coil Actuator
- Heater / Peltier
- Electromagnet
- Solenoid Actuator

Status aktuator akan berubah secara otomatis mengikuti state sistem dan sensor yang aktif.

---

### 🔹 Signal Visualization

- Visualisasi sinyal real-time menggunakan **ASCII waveform**
- Pola sinyal berubah sesuai kondisi sistem:
  - Amplitudo rendah → NORMAL
  - Amplitudo tinggi → EMERGENCY
- Update setiap **500 ms**

---

### 🔹 Emergency Reset

Tombol **EMERGENCY RESET** digunakan untuk:
- Menonaktifkan seluruh sensor
- Mengembalikan sistem ke:
  - POPCOUNT = 0
  - STATE = NORMAL (00)
- Dilengkapi konfirmasi dan notifikasi sistem

---

## 🧠 System Logic


