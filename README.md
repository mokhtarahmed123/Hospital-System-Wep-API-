 
# 🏥 Hospital Management System API

A complete Hospital Management System built with **ASP.NET Core Web API** and **Entity Framework Core**, designed to manage hospital operations such as patient admission, doctor assignment, billing, and room management — with secure, role-based access.

---

## 🚀 Features

- ✅ Role-based access: Admin, Doctor, Accountant
- 🧑‍⚕️ Patient admission and doctor assignment
- 🛏️ Room allocation and management
- 💵 Automated billing and payment tracking
- 📊 Admin dashboards and reports
- 🔐 Secure authentication with JWT
- 🧼 Clean layered architecture using DTOs, Services, and EF Core

---

## 🧭 System Workflow

1. **Admins** create users (Doctors, Accountants) and manage departments, rooms, and roles.
2. **Patients** are registered and either admitted (inpatients) or handled as outpatients.
3. **Doctors** diagnose, prescribe treatment, and discharge patients.
4. **Accountants** generate and track bills based on room stay and medical services.
5. **Admins** monitor system performance and generate reports.

---

## 🧱 Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- AutoMapper
- LINQ

---

## 📂 Main Modules

| Module               | Description                                    |
|----------------------|------------------------------------------------|
| User Management      | Identity + Roles (Admin, Doctor, Accountant)  |
| Patient Management   | Add, view, and manage patient records         |
| Inpatient Admission  | Assign room, doctor, and track stay duration  |
| Billing System       | Generate bills and track payment status       |
| Room Management      | Assign/release rooms for admitted patients    |

---

## 📌 Future Enhancements

- ✅ Appointment scheduling for outpatients
- ✅ Email/SMS notifications
- ✅ Full audit log tracking
- ✅ Soft delete and historical records
- ✅ Multi-language support (English/Arabic)

---

## 🛡️ Security

- Role-based authorization with ASP.NET Identity
- JWT Bearer token authentication
- Endpoint protection via `[Authorize(Roles = "...")]`

---

## 📸 Screenshots (Optional)

> You can add Swagger UI screenshots or Postman test examples here

---

## 📦 Setup Instructions

1. Clone the repository
2. Configure `appsettings.json` with your DB connection
3. Run `dotnet ef database update` to apply migrations
4. Run the project and browse to `/swagger` to test the API

---

## 🤝 Contributing

Feel free to fork, open issues, or suggest improvements. PRs are welcome!

---

## 📄 License

This project is licensed under the MIT License.
