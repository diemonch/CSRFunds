# CSRFundDetails

## Overview
CSRFundDetails is a .NET web application designed to manage CSR (Corporate Social Responsibility) funds for organizations. The application provides functionalities for users to log in and manage CSR funds, view, edit, and save information, and logout. It also has administrator capabilities to manage users and funds. This document provides the necessary steps to set up the application, install dependencies, and configure it for deployment to an on-premises IIS server.

## Prerequisites
Before starting the installation process, ensure the following are installed and set up:

### 1. .NET 7 SDK
- Download and install the .NET 7 SDK from [Microsoft .NET SDK Download](https://dotnet.microsoft.com/en-us/download/dotnet/7.0).
- Verify the installation by running the following command in a terminal:
  ```bash
  dotnet --version

  2. MongoDB

The application uses MongoDB as its database. Follow the instructions to install and configure MongoDB.

Installing MongoDB

	•	Download the MongoDB Community Edition from the MongoDB Download Center.
	•	Follow the official MongoDB installation documentation for your operating system.

Starting MongoDB

Once installed, start the MongoDB service:
mongod --config /usr/local/etc/mongod.conf --fork

3. IIS (Internet Information Services)

IIS is the web server used to host the application on Windows machines. Ensure that IIS is installed and configured with the necessary components for .NET applications.

Installing IIS

	•	Open Control Panel > Programs > Turn Windows features on or off.
	•	Check the Internet Information Services checkbox.
	•	Ensure the following sub-components are selected:
	•	Web Management Tools
	•	World Wide Web Services
	•	Application Development Features (ASP.NET and .NET Extensibility)

Getting the Code

	1.	Clone the repository from GitHub:
git clone https://github.com/diemonch/CSRFunds.git

Project Structure

	•	CSRFundDetails.sln - The main solution file for the project.
	•	CSRFundDetails/ - The main project directory containing source code.
	•	.gitignore - The ignore file for Git.

Configuration

1. MongoDB Connection

Update the MongoDB connection string in the appsettings.json file under the CSRFundDetails project directory:

{
  "ConnectionStrings": {
    "MongoDB": "mongodb://localhost:27017/CSRFundDetails"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}

 Configure the Application for IIS

Step 1: Publish the Application

	1.	In Visual Studio, right-click on the CSRFundDetails project and select Publish.
	2.	Choose Folder as the publish target.
	3.	Set the destination folder (e.g., C:\inetpub\wwwroot\CSRFundDetails).
	4.	Click Publish.

Step 2: Configure IIS

	1.	Open the IIS Manager on your Windows machine.
	2.	Right-click on Sites > Add Website.
	3.	Set the following:
	•	Site Name: CSRFundDetails
	•	Physical Path: The path to the folder where you published the app (e.g., C:\inetpub\wwwroot\CSRFundDetails).
	•	Binding: Ensure you bind the site to port 80 or another port as needed.
	4.	Click OK.

Step 3: Configure Application Pool

	1.	Open IIS Manager.
	2.	In the Connections pane, expand the server node and click Application Pools.
	3.	Right-click the DefaultAppPool (or your custom app pool), and choose Basic Settings.
	4.	Set the .NET CLR Version to No Managed Code.
	5.	Set the Managed Pipeline Mode to Integrated.

Step 4: Set Up Permissions

	1.	Ensure that the IIS user (e.g., IIS_IUSRS) has Read & Execute permissions on the application folder (e.g., C:\inetpub\wwwroot\CSRFundDetails).

3. MongoDB Collections Setup

You will need to ensure the necessary collections are created in MongoDB. Once MongoDB is running, follow the steps to create necessary collections like Users, CSRFunds, etc. You can use the following command to create a collection:


use CSRFundDetails
db.createCollection("Users")

You can manually insert initial test users into the Users collection:

db.Users.insert({
  "UserId": "admin",
  "Password": "admin123",
  "Role": "csr-admin",
  "OrganizationId": "org001"
})


Running the Application

1. Run Locally (Optional)

Before deploying to IIS, you can run the application locally using Visual Studio:

	1.	Open the project in Visual Studio.
	2.	Press F5 to build and run the application locally on http://localhost:5000.

2. Running on IIS

	1.	Open your browser and navigate to http://localhost/CSRFundDetails (or the domain name where it’s hosted).
	2.	Log in using one of the test user credentials.

Troubleshooting

	1.	MongoDB Connection Issues
Ensure that the MongoDB service is running. Check your connection string in appsettings.json and ensure that it matches your MongoDB instance details.
	2.	IIS Deployment Errors
If the application doesn’t load on IIS, ensure the .NET Core Hosting Bundle is installed on the server. Download it from here.
	3.	Logs
You can check the application logs in the Logs directory to see more details on any errors.
 
