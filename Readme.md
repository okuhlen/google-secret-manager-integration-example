# Google Secret Manager Integration Example

#### The purpose of this repository is to provide a working example of how Google's Secret Manager offering can be integrated into existing .NET backend applications. For more information on this cloud offering, please visit the [Google Secret Manager documentation](https://cloud.google.com/security/products/secret-manager).

## Introduction

Google Secret Manager is a cloud service offering that allows developers to store sensative application secrets and keys in a centralized and secure manner. The cloud offering includes advanced features such as secret versioning, logging of secret interractions, and much more. If you would like to read more on the offering, please visit their [documentation](https://cloud.google.com/security/products/secret-manager).

After much thought, I thought it would be a good idea to create a working example of how this cloud offering can be integrated into existing .NET backend applications. This code was pulled from my previous projects that made use of the offering. It is my hope that this example does have value to someone looking to integrate a similar solution. As my 2nd year information systems lecturer famously said, "there are many ways to skin a cat." - this means that there are many ways to implement and improve the solution to your needs.

## Solution Setup

This solution contains 3 projects:

- Application: This is a class library that contains common extensions, utilities, constants and other application related model classes.
- SettingsLoader: This is a console application that loads secrets into your Google secret manager instance. The idea here is, you could have a powershell script or similar which runs this app when neccessary (in development, or when deploying code to production).
- WebApiExample: This is a simple ASP.NET Core Web API application that uses the secrets loaded into your secret manager instance. More on this below.

## Setting everything up

- Create a Google Cloud project and enable the Secret Manager API.
- Create a service account with the Secret Manager Admin role.
- Update the settings.csv file in the Application project with the secrets you wish to load into google secret manager. Have a look at how the example entry is formatted.
