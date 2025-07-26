# 🏫 School Management System API

Sistem manajemen sekolah berbasis **ASP.NET Core Web API**, menyediakan fitur lengkap untuk mengelola:

- 👨‍🎓 **Students** (Siswa)
- 👩‍🏫 **Teachers** (Guru)
- 🏫 **Classes** (Kelas)
- 📝 **Enrollments** (Pendaftaran siswa ke kelas)

---

## 🚀 Features

- CRUD untuk Student, Teacher, dan Class
- Assign / Unassign Teacher ke Class
- Enroll Student ke Class tertentu
- Prevent duplicate enrollments
- Pencarian (search), sorting, dan pagination
- Dokumentasi API menggunakan Swagger

---

## 🧰 Tech Stack

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core
- AutoMapper
- Swagger
- PostgreSQL

---

## 📦 Setup & Instalasi

### 1. Clone Repository

```bash
git clone https://github.com/DwiBactiar12/School_Management_System.git
cd SchoolManagementSystem
```

### 2. Clone Repository
Salin file .env.example menjadi .env:
```bash
cp .env.example .env
```
Edit isi .env sesuai kebutuhan:
```bash
DB_HOST=localhost
DB_PORT=5432
DB_NAME=school_management
DB_USER=your_db_user
DB_PASS=your_db_password
```

### 3. Restore Dependency
```bash
dotnet restore
```

### 4. Jalankan Migrasi Database
Install EF CLI jika belum:
```bash
dotnet tool install --global dotnet-ef
```
Lalu jalankan migrasi:
```bash
dotnet ef database update
```
### 5. Jalankan Aplikasi
```bash
dotnet run
```
Dokumentasi API(SWAGGER)
```bash
https://localhost:7056/swagger/index.html
```

### 5. Contoh payload Data
Api /api/Class:
```bash
[
  {
    "classCode": "CLS-2025-01",
    "name": "Kelas 10 IPA 1",
    "description": "Kelas unggulan untuk jurusan IPA tingkat 10"
  },
  {
    "classCode": "CLS-2025-02",
    "name": "Kelas 11 IPS 2",
    "description": "Fokus pada pengembangan ilmu sosial dan ekonomi"
  },
  {
    "classCode": "CLS-2025-03",
    "name": "Kelas 12 Bahasa",
    "description": "Program khusus untuk studi bahasa dan sastra"
  },
  {
    "classCode": "CLS-2025-04",
    "name": "Kelas 10 Teknik Komputer",
    "description": "Dasar-dasar teknik komputer dan jaringan"
  },
  {
    "classCode": "CLS-2025-05",
    "name": "Kelas 11 Seni Musik",
    "description": "Pembelajaran teori dan praktik musik klasik & modern"
  }
]

```
Api /api/Student:
```bash
[
  {
    "studentId": "STD-2025-001",
    "firstName": "Alya",
    "lastName": "Putri",
    "email": "alya.putri@example.com",
    "phone": "081234567891",
    "dateOfBirth": "2008-05-12T00:00:00Z"
  },
  {
    "studentId": "STD-2025-002",
    "firstName": "Rizky",
    "lastName": "Ramadhan",
    "email": "rizky.ramadhan@example.com",
    "phone": "081298765432",
    "dateOfBirth": "2007-11-25T00:00:00Z"
  },
  {
    "studentId": "STD-2025-003",
    "firstName": "Nadia",
    "lastName": "Salsabila",
    "email": "nadia.salsa@example.com",
    "phone": "082123456789",
    "dateOfBirth": "2009-02-03T00:00:00Z"
  },
  {
    "studentId": "STD-2025-004",
    "firstName": "Dimas",
    "lastName": "Saputra",
    "email": "dimas.saputra@example.com",
    "phone": "085612345678",
    "dateOfBirth": "2008-08-18T00:00:00Z"
  },
  {
    "studentId": "STD-2025-005",
    "firstName": "Siti",
    "lastName": "Nurhaliza",
    "email": "siti.nur@example.com",
    "phone": "081356789012",
    "dateOfBirth": "2007-12-30T00:00:00Z"
  }
]

```
Api /api/Teacher:
```bash
[
  {
    "teacherId": "TCH-2025-001",
    "firstName": "Intan",
    "lastName": "Lestari",
    "email": "intan.lestari@example.com",
    "phone": "081223344556",
    "subject": "Matematika"
  },
  {
    "teacherId": "TCH-2025-002",
    "firstName": "Budi",
    "lastName": "Santoso",
    "email": "budi.santoso@example.com",
    "phone": "082112223334",
    "subject": "Fisika"
  },
  {
    "teacherId": "TCH-2025-003",
    "firstName": "Rina",
    "lastName": "Wahyuni",
    "email": "rina.wahyuni@example.com",
    "phone": "083344556677",
    "subject": "Bahasa Indonesia"
  },
  {
    "teacherId": "TCH-2025-004",
    "firstName": "Agus",
    "lastName": "Hidayat",
    "email": "agus.hidayat@example.com",
    "phone": "081355566677",
    "subject": "Sejarah"
  },
  {
    "teacherId": "TCH-2025-005",
    "firstName": "Dewi",
    "lastName": "Amalia",
    "email": "dewi.amalia@example.com",
    "phone": "085677889900",
    "subject": "Biologi"
  }
]

```
