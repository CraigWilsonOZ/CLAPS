
Connect-AzureAD

$appName = "CLAPS"
$appHomePageUrl = "http://craigwilson.blog/"
$appReplyURLs = @("http://localhost")
$appLogoPath = "F:\Repos\CLAPS\library.claps\Assests\CloudLock.png"

$Guid = New-Guid
$startDate = Get-Date
    
$PasswordCredential = New-Object -TypeName Microsoft.Open.AzureAD.Model.PasswordCredential
$PasswordCredential.StartDate = $startDate
$PasswordCredential.EndDate = $startDate.AddYears(10)
$PasswordCredential.KeyId = $Guid
$PasswordCredential.Value = ([System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes(($Guid))))

if(!($myApp = Get-AzureADApplication -Filter "DisplayName eq '$($appName)'"  -ErrorAction SilentlyContinue))
{
    $myApp = New-AzureADApplication -DisplayName $appName -Homepage $appHomePageUrl -ReplyUrls $appReplyURLs -PasswordCredentials $PasswordCredential -PublicClient $true
    Set-AzureADApplicationLogo -ObjectId $myApp.ObjectId -FilePath $appLogoPath
    $myAppSP = New-AzureADServicePrincipal -AccountEnabled $true -AppId $myApp.AppId -AppRoleAssignmentRequired $true -DisplayName $myApp.DisplayName -Tags {WindowsAzureActiveDirectoryIntegratedApp}
}

Write-Output "Application created, use the following values for configuration."
Write-Output "------------------------------------------------------------------"
Write-Output "Client ID         : $($myApp.AppId)"
Write-Output "Client Secert     : $($PasswordCredential.Value)"
Write-Output "------------------------------------------------------------------"
Write-Output "Service Principal created, use the following values for Key Vault."
Write-Output "------------------------------------------------------------------"
Write-Output "Subscription ID   : $($myAppSP.AppOwnerTenantId)"
Write-Output "Service Principal : $($myAppSP.ObjectId)"

