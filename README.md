# SOA CA2 - Library Management 

## Introduction
Library management API with Books, Members, and Borrowings data available for CRUD.  
Use ORM Entity Framework Core to interact with SQL Server database.  
Hosted MySQL database on Azure.  
Used Python to filter books.csv data from Kaggle to populate Books table.  
Kept 1000 records only for easier processing & light on database.  
Seeded Members & Borrowings table ourselves.  
Used AutoMapping for DTOs.  
Hosted API on Railway with CI/CD pipeline on master branch on GitHub.  
Using Swaggle & POSTMAN for testing API.  

## Data 
Tables Books and Members are linked many-to-many with the connecting table Borrowings that takes foreign key of each.  

### Books
ISBN (varchar) (Book Unique ID primary key)  
Title (varchar)  
Author (varchar)  
CopiesAvailable (int) (Default to 1)  

### Members
Id (int) (Primary Key)  
Name (varchar)  
Email (varchar)  
PhoneNumber (varchar)  

### Borrowings
Id (int) (Primary Key)  
MemberId (int) (Foreign Key)   
BookISBN (varchar) (Foreign Key)  
BorrowedDate (date)  
ReturnedDate (date)  

## Reference
https://www.kaggle.com/datasets/saurabhbagchi/books-dataset/data  
https://server78135.medium.com/automapper-a604f9e2afbb#:~:text=AutoMapper%20is%20a%20DTO(Data%20Transfer%20Object)%20library%20in%20Asp,data%20as%20we%20need%20them.  
