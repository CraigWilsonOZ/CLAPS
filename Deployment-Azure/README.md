# Deployment-Azure </br>
This template allows you to deploy an Azure Key Vault. A script has been included for the creation of an AzureAD Application and Serivce Principal.

The Template builds the following:
 * Creates a Azure Key Vault
 
## Parameters </br>
- kvname 
  - The Key Vault name for this deployment
  - Type: String

- tenantId  
  - The Tenant ID for the Azure AD Application.
  - Type: Strng

- DefaultCLAPSRoleId  
  - The Service Principal ID for the Azure AD Application.
  - Type: Strng
  
- location 
  - The Azure location for this deployment
  - Type: String

## Prerequisites </br>
AzureAD Application with Serivce Principal

## Script </br>
The script Create-AzureADAppwithServicePrincipal.ps1 can be used to create the AzureAD Application and Serivce Principal account. The output from the script will contain the required values for the deployment parameters and registry configuration changes.
