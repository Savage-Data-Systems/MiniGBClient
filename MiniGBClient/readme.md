# Prerequisists 
- .Net 8 SDK
- IDE (Recommended Visual Studio 2022)

# Setup
1. Configure your config.json file
- The client_id and client_secret are provided when you create an application in the SavageData Onboarding site. Or if you have another Data Custodian you want to connect to.
`{
  "client_id": "<your client id>",
  "client_secret": "<your client secret>"
}`
2. Configure appsettings.json
- The provided appsettings assume you've created an application in an SavageData Onboarding site and had requested all possible data for the scope.
3. Run the application and login, you can use the provided credentials from the onboarding site or use the one time access login with any information.
4. Accept the authorization. 
