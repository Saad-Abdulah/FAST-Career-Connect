# FAST Career Connect

🎯 **Experience the Future of Career Management!** 

Check out our live demo:
[Visit Project Demo Here](https://www.linkedin.com/posts/saadabdullah786_%F0%9D%90%85%F0%9D%90%80%F0%9D%90%92%F0%9D%90%93-%F0%9D%90%82%F0%9D%90%9A%F0%9D%90%AB%F0%9D%90%9E%F0%9D%90%9E%F0%9D%90%AB-%F0%9D%90%82%F0%9D%90%A8%F0%9D%90%A7%F0%9D%90%A7%F0%9D%90%9E%F0%9D%90%9C%F0%9D%90%AD-activity-7329773721219395584-VPds)

![FAST Career Connect](FAST_Career_Connect.png)

A comprehensive career portal system for managing job fairs, interviews, and career opportunities at FAST University.

## 🚀 Features

- Student Portal
  - Create and manage profiles
  - Search and apply for jobs
  - Schedule interviews
  - Submit company reviews

- Company Portal
  - Post job opportunities
  - Shortlist applications
  - Manage interview slots
  - Hire candidates

- TPO (Training & Placement Office) Portal
  - User approval management
  - Job fair scheduling
  - Booth allocation
  - Generate reports

- Booth Coordinator Portal
  - Student check-in management
  - Monitor booth traffic

## 📋 Prerequisites

- Visual Studio 2019 or later
- SQL Server 2019 or later
- .NET Framework 4.7.2 or later
- Reports(RDLC) Setup in VS

## 🛠️ Setup Instructions

### 1. Database Setup

1. Open SQL Server Management Studio
2. Create a new database named `FASTCareerConnect`
3. Download the SQL scripts from the repository's `database` folder
4. Execute the scripts in the following order:
   - `Data_base_Creation .sql`
   - `Data_base_Insertion.sql`
   - `Function.sql`

### 2. Configuration Setup

1. Create `App.config` file in the root directory:
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
       <connectionStrings>
           <add name="UserDB" 
                connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=FASTCareerConnect;Integrated Security=True;TrustServerCertificate=True"
                providerName="System.Data.SqlClient" />
       </connectionStrings>
   </configuration>
   ```
   Replace `YOUR_SERVER_NAME` with your SQL Server instance name.

2. Create `DatabaseConfig.cs` file:
   ```csharp
   using System.Configuration;

   namespace Fast_Connect_DB_Final_project
   {
       public static class DatabaseConfig
       {
           private static string _connectionString;

           public static string ConnectionString
           {
               get
               {
                   if (string.IsNullOrEmpty(_connectionString))
                   {
                       LoadConnectionString();
                   }
                   return _connectionString;
               }
           }

           private static void LoadConnectionString()
           {
               try{
               _connectionString = ConfigurationManager.ConnectionStrings["UserDB"].ConnectionString;
                }
                catch (Exception)
                {
                    // Fallback to default connection string if App.config entry is not found
                    _connectionString = "Data Source=YOUR_SERVER_NAME;Initial Catalog=FASTCareerConnect;Integrated Security=True;TrustServerCertificate=True"
                providerName="System.Data.SqlClient";
                }
           }
       }
   }
   ```
    Replace `YOUR_SERVER_NAME` with your SQL Server instance name.

### 3. Build and Run

1. Open the solution in Visual Studio
2. Restore NuGet packages:
   - Right-click on the solution
   - Select "Restore NuGet Packages"
3. Build the solution (Ctrl + Shift + B)
4. Run the application (F5)

The application will start from `Program.cs` with the login form.

## 👥 Default Login Credentials

- We have role for all Users
- Execute 
  - Select * from Users
  - Input Emial & Password of user with Desriered role 


## 🔒 Security Note

1. Never commit actual connection strings
2. Keep your database credentials secure
3. Use Windows Authentication when possible

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📝 License

This project is totally Open-Source developed Personally by Us.

## 👨‍💻 Authors

- [Saad Abdullah](https://github.com/Saad-Abdulah)
- [Muneeb Ur Rehman](https://github.com/MUNEEBAZAM96)