Project Summary
===============

The traditional tool LAPS
(<https://www.microsoft.com/en-us/download/details.aspx?id=46899>) is
used by IT administrators to manage local administrator accounts for
Active Directory joined workstations and servers. This project has been
created to replace that tool, LAPS. This tool will perform a similar
function for Azure AD joined devices that are not connected to a local
domain.

Windows workstations and servers that are joined only to an AzureAD have
no access to Active Directory or Group Policy as such tools like LAPS do
not work. This tool has been designed to fit this gap.

Cloud Local Administrator Password Service or CLAPS has been designed to
run locally on a workstation or server. It will create a new local
administrator account, reset the password at a set time and finally
store the password in Azure Key Vault. This allows auditing on who has
access to the accounts, permits offline access to the workstation and
removes the need for standard users to be given local administrator
rights to their daily account.

Microsoft does provide a solution for device management. Users who have
the Global Administrator or Device Administrator roles are added as
local administrators. The setting is tenant wide and allows any device
administrator to login to any device. See the link
<https://docs.microsoft.com/en-us/azure/active-directory/devices/assign-local-admin>

Requirements
------------

The solution has been built to address the following requirements

1.  The solution must generate a complex password

2.  The solution must reset the password within a given time period

3.  The solution must provide a record of the actions taken

4.  All passwords must be transmitted securely

5.  All passwords must be stored securely

6.  The solution must support unattended installation

7.  Solution configuration can be updated without reinstallation

8.  The solution should use the minimum-security permissions required to
    perform actions

Solution architecture
---------------------

The core functions of the solution use a windows service. This service
runs in the .NET Core framework. It uses either a json configuration
file or local registry for configuration. The services on start-up will
check the registry for the last update to a password, if the password
record is outside the number of hours for a change, then the service
will reset the password. The service then goes to sleep for up to 24
hours. Every time the service is restarted, or the 24-hour window has
passed, the service checks again for a required password changed.

The password is created in code and uses the following values;

  Category|Characters
  --------------------|----------------------------
  Capital letters|ABCDEFGHIJKLMNOPQRSTUVWXYZ
  Small letters|abcdefghijklmnopqrstuvwxyz
  Numbers|0123456789
  Special characters|!@\$?\_-
  Length|24
  Unique characters|4
  

An Azure Key Vault is set up to store the passwords. This is required to
be configured before deployment. The Service gets access to the Key
Vault via an AzureAD Application. This Application is given set
permissions inside the Azure Key Vault, so it can only create a secret,
not read them.

Logging has been built into the application. All actions taken locally
are logged into the Windows Event Log service. In addition, Azure Key
Vault can also perform auditing logs if required.

The service will require internet access. Once a local account password
has been set, it will attempt to store the password in the Azure Key
Vault. It will require the Key Vault URL to be whitelisted in proxy
services.

Deployment
==========

The service will need to be deployed in the following order.

1.  Deployment of AzureAD Application

2.  Creation of Azure Key Vault and assign AzureAD Application with set
    permissions

3.  Update of configuration, using one of two methods

    a.  MSI, Appconfig.json with Key Vault URL, AzureAD ClientID and
        Secret

    b.  Update registry settings

4.  Deployment of MSI

5.  Deployment of registry configuration, if the MSI has not been
    updated

Deployment of AzureAD Application
---------------------------------

An AzureAD Application and Service Principal is required to access the
Azure Key Vault. The following script can be used to create an
Application and Principal.

<https://github.com/CraigWilsonOZ/CLAPS/blob/master/Deployment-Azure/Create-AzureADAppwithServicePrincipal.ps1>

The script outputs follow the following values; these will be required
to complete the configuration.

  Output|Value Usage
  -------------------|-----------------------------------------------------------
  ClientID|Azure AD Application Client ID
  Client Secret|Azure AD Application Client Secret
  Subscription ID|AzureAD Tenant ID
  Service Principal|Service Principal ID used to give access to the Key Vault
  

Deployment of the Azure Key Vault
---------------------------------

The Azure Key Vault is used to store the machine name and credential.
Each time the password is reset, a new record is created under the
machine name. The following ARM templates can be used to create the Key
Vault. The Service Principal and Tenant ID will need to be updated in
the parameter file.

<https://github.com/CraigWilsonOZ/CLAPS/blob/master/Deployment-Azure/>

Deployment of Service
---------------------

The service can be installed using the MSI package. It will create all
the registry keys and install the service. The registry keys will need
to be updated to include your settings created during the deployment of
the AzureAD Application and Key Vault. A PowerShell script has been
included to help with the deployment of the keys. To deploy to an
AzureAD connected device, use Intune. First deploy the MSI, once the MSI
has been deployed. Push out the PowerShell script to update the registry
keys. Another option would be to update the MSI with your own values.
The MSI is created with Visual Studio. Once installed, the application
will run as a service.
