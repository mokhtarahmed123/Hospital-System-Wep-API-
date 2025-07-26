 
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
![API5 png](https://github.com/user-attachments/assets/92787bf7-751a-4276-a4b6-29e8b1599605)

This project is licensed under the MIT License.
![API png](https://github.com/user-attachments/assets/9fa82436-ed1f-4919-8376-03e24b5a7aa4)
![API2 PNG](https://github.com/user-attachments/assets/a408050c-c786-4464-8f00-87a815f23180)
![API3 png](https://github.com/user-attachments/assets/4ab31cbb-0660-4732-844e-989fb5c6fd2c)
![API6 png](https://github.com/user-attachments/assets/ebe0dd60-818d-4a8e-87f2-f4ba43ef33ba)

![API4 png](https://github.com/user-attachments/assets/2582d0e9-af8c-4799-8791-fd7e1fcbe9d6)

