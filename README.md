# Student Attendance Management System (SAMS)

## 📌 Overview

SAMS replaces traditional attendance methods with a **fast, secure, and automated web solution** designed for schools, colleges, and training centers.

This system prevents proxy (fake) attendance by using **Smart Dynamic QR Codes** that expire and regenerate automatically.
Students must be physically present to mark attendance, making the system far more reliable than typical classroom attendance apps.

---

## 🚀 Key Features

### 🔐 User Roles

* **Admin**

  * Manage students, teachers, classes
  * Assign teachers to classes
  * View global reports

* **Teacher**

  * Start attendance sessions
  * Generate dynamic QR codes
  * Validate attendance (QR + location)
  * View reports

* **Student**

  * Scan QR to mark attendance
  * View attendance history
  * Receive alerts

---

## 🆕 Innovative Feature: Smart Dynamic QR Attendance

Unlike typical apps, SAMS introduces **time-sensitive QR codes**:

* QR regenerates every 15–30 seconds
* Prevents screenshot sharing
* Location verification ensures the student is present
* System detects:

  * Fake scans
  * Remote attempts
  * Duplicate submissions

This makes attendance **secure and cheat-proof**.

---

## 📊 Reporting & Analytics

* Daily summaries
* Monthly attendance reports
* Student attendance history
* Low attendance alerts

---

## 🛠 Technology Stack

* **Backend:** Web Api
* **Frontend:** Razor Pages
* **Database:** SQL Server
* **QR Code Generation:** QRCoder
* **Location Verification:** Browser Geolocation API / WiFi-based checks

---

## 📂 Database Structure

### Tables:

* **Users** (Admin/Teacher/Student)
* **Students**
* **Classes**
* **Attendance**
* **QRSession** *(For dynamic QR tracking)*

---

## ▶️ How Attendance Works

1. Teacher clicks **Start Attendance Session**
2. System generates a **dynamic QR code**
3. Students scan using their phone
4. Backend verifies:

   * QR validity
   * Time of scan
   * Student location
5. Attendance is recorded securely

---



