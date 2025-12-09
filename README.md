🎓 Student Attendance Management System (SAMS)

A secure, smart, and modern attendance management platform for schools, colleges, and training institutions.

SAMS replaces outdated manual attendance processes with a QR-based, automated, cheat-proof system built on ASP.NET Core Web API + Razor Pages.

🚀 Key Features
🧑‍💼 User Roles
Role	Capabilities
Super Admin	Manages entire system, admins, global settings
Admin	Manages students, teachers, classes, timetable
Teacher	Starts attendance sessions, generates QR, verifies attendance
Student	Scans QR, views history, requests leave
Parent (Portal)	Monitors student attendance & alerts
🔐 Smart Dynamic QR Attendance (Anti-Proxy System)

SAMS introduces dynamic, auto-refreshing QR codes to prevent misuse:

QR regenerates every 15–30 seconds

Cannot be screenshot-shared

Linked with GPS / WiFi location verification

QR tied to specific class + teacher + time

Backend checks:

QR expiration

Student location

Duplicate scans

Suspicious/Fraud attempts (AI-based pattern detection)

This ensures students must be physically present to mark attendance.

📊 Reporting & Analytics

Daily, weekly, monthly attendance analytics

Heatmaps & trend graphs

Student-wise performance

Class/department comparisons

Automated low-attendance alerts

Export reports to PDF / Excel

🗓 Timetable & Scheduling

Official timetable management

Attendance session allowed only for scheduled class times

Teachers cannot trigger attendance at random hours

📩 Leave / Excuse Request Module

Students submit digital leave requests

Teachers review and approve/decline

Leave auto-updates attendance reports

🔔 Smart Alerts & Notifications

Email/SMS notifications for:

Low attendance

Attendance session started/ended

Leave approval status

📱 Parent Portal

Parents can view:

Daily attendance

Monthly report

Alerts & warnings

📝 Exam & Event Attendance

QR-based or manual attendance for:

Examinations

School events

Seminars / Activities

🧠 AI-Based Fraud Detection

Detects suspicious patterns such as:

Frequent borderline attendance

Same device used for multiple students

Remote attendance attempts

Repeated wrong location scans

🔌 Offline Attendance Mode (Optional)

Teachers can take attendance without internet:

Local caching

Auto-sync when connection is restored

🛠 Technology Stack
Layer	Technology
Backend	ASP.NET Core Web API
Frontend	Razor Pages
Database	SQL Server
QR Generation	QRCoder
Auth	JWT / Identity
Location Verification	Browser Geolocation API
Reports	PDF/Excel generators
AI Module	Pattern detection logic (custom rules/ML optional)
🗄 Database Structure
Core Tables

Users (Role-based)

Students

Teachers

Classes

Timetable

Attendance

QRSession (tracks dynamic QR lifecycle)

LeaveRequests

ParentAccounts

Notifications

▶️ Attendance Workflow

Teacher starts an attendance session

Backend generates a dynamic QR code

Students scan using phone

System verifies:

QR validity

Time window

Student location

Unique device

Attendance is securely recorded

Reports update automatically

📦 API Layer

REST API endpoints for:

Attendance

Students

Teachers

Classes

Leave requests

Parent portal

Reports

QR sessions

Used by Razor Pages and can be extended for mobile apps.


