# ToDoApp Using DOTNET


A simple **To-Do List** web application built using **ASP.NET Core MVC**. This app allows users to create, edit, delete, and mark tasks as completed. It also features email notifications for task reminders.

---

## Features  
 Add, Edit, and Delete To-Do tasks  
 Mark tasks as Completed  
 Set Due Dates for tasks  
 Email Notifications for task reminders  
 Bootstrap-based Responsive UI  

---

## Tech Stack  
- **Frontend**: HTML, CSS, Bootstrap  
- **Backend**: ASP.NET Core MVC  
- **Database**: Entity Framework Core (SQL Server)  
- **Email Service**: SMTP (MailKit)  

---

## Installation  

### Clone the Repository  
```bash
git clone https://github.com/JanviBh249/ToDoApp_DOTNET.git
cd ToDoApp_DOTNET
```

### Configure the Database  
Update the `appsettings.json` file with your database connection string.

### Run Migrations  
```bash
dotnet ef database update
```

### Run the Application  
```bash
dotnet run
```

---

## Email Configuration  
To enable email notifications, add your **SMTP settings** in `appsettings.json`:  
```json
"Smtp": {
  "Host": "smtp.your-email.com",
  "Port": "587",
  "Username": "your-email@example.com",
  "Password": "your-email-password"
}
```

## Author  
👩‍💻 **Janvi Bhanushali**  
🔗 [GitHub](https://github.com/JanviBh249) | ✉️ [Email](mailto:janvibhanushali249@gmail.com)  

---

Feel free to contribute and improve this project! 
```
