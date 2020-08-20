# LifeInsuranceWebApi
VS Code Web API. This web api is build in C# and based on the customer detail, the coverage plan is applied and saved in MS SQL server

To run this project following is required 

## Dependencies

- npm
- .NET CORE 3.1 
- .NET Code

- MS SQL SERVER 2017 ONWARDS

This Repo as 2 parts 
1) SQL Server Script  [Present in: SQLScripts/Script.sql]
2) VS CODE PROJECT

SQL Server Script: 
 Download the SQL/Script.sql file and open in "Microsort SQL Server Management Studio" 
 Run the script against any Databases
 After executing you should see 3 table Contracts, Coverage Plan and Rate Chart 

VS CODE PROJECT
 Open the folder in VS Code 
 After the project is opened 
  a) Goto appsettings.json -> replace the value for "dbConnection" to your local MS SQL SERVER
  b) Run the following command 
  
  'dotnet restore'
  
  'npm install'
  
  'dotnet build'
  
Execute the program using CTRL + F5. 

In case of certificate issue come while running the program, use the below command 
1.dotnet dev-certs https --clean
2.dotnet dev-certs https -t
3.Restart VS
  
Use Postman to test following contract

GET Contracts: https://localhost:5001/api/LI
GET Contract By Id: https://localhost:5001/api/LI/1
POST: https://localhost:5001/api/LI
Example Payload:
{
  "customerName":"Jolie",
  "customerAddress":"Grande",
  "customerGender":"F",
  "customerCountry":"CAN",
  "customerDateofBirth":"1980-01-01T00:00:40",
  "saleDate":"2007-01-01T00:00:40"
}
PUT: https://localhost:5001/api/LI/2
Example Payload:
{
    "id": 2,
    "customerName": "Jolie A",
    "customerAddress": "TEXAS",
    "customerGender": "F",
    "customerCountry": "USA",
    "customerDateofBirth": "1980-01-01T00:00:40",
    "saleDate": "2010-01-01T00:00:40"
}

DELETE: https://localhost:5001/api/LI/4