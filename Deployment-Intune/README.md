# Deployment-Intune </br>
This section holds the scripts and registry files needed for the configuration of the services.

## Prerequisites </br>
AzureAD Application with Serivce Principal created

## Script </br>
The script Deploy-RegistrySettings.ps1 can be deployed via Intune. In the device configuration section, create a PowerShell script deployment task and assign to devices. Make sure the registry keys are update with your values and the script is deployed post MSI deployment. The MSI deployment if used will replace the registry keys.

## Reg File </br>
The registry file has been include for manual deployment if required.

