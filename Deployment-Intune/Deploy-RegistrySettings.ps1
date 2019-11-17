# PowerShell Script for deployment in Intune

New-ItemProperty -Name BaseDirectory -PropertyType String -Value "C:\\Program Files\\CraigWilson.Blog\\CLAPS" -Path HKLM:\SOFTWARE\CraigWilson.Blog\CLAPS -force
New-ItemProperty -Name KeyVaultURL -PropertyType String -Value "https://yoururl.vault.azure.net" -Path HKLM:\SOFTWARE\CraigWilson.Blog\CLAPS -force
New-ItemProperty -Name HourOfDayForReset -PropertyType dword -Value "00000014" -Path HKLM:\SOFTWARE\CraigWilson.Blog\CLAPS -force
New-ItemProperty -Name LocalAdministratorGroupName -PropertyType String -Value "Administrators" -Path HKLM:\SOFTWARE\CraigWilson.Blog\CLAPS -force
New-ItemProperty -Name HoursBeforeUpdate -PropertyType String -Value "1" -Path HKLM:\SOFTWARE\CraigWilson.Blog\CLAPS -force
New-ItemProperty -Name KeyVaultClientID -PropertyType String -Value "5f6ff3b8-ec6f-437d-9e9e-201fad92ff35" -Path HKLM:\SOFTWARE\CraigWilson.Blog\CLAPS -force
New-ItemProperty -Name KeyVaultSecret -PropertyType String -Value "OTY1YjE4YmUtMWQ0Yy00ZmNhLTlmOWItYTliYWI0MDE4NTQy" -Path HKLM:\SOFTWARE\CraigWilson.Blog\CLAPS -force
New-ItemProperty -Name LocalAccountName -PropertyType String -Value "MyLocalAdmin" -Path HKLM:\SOFTWARE\CraigWilson.Blog\CLAPS -force
New-ItemProperty -Name LastUpdateTime -PropertyType String -Value "20191116-200004" -Path HKLM:\SOFTWARE\CraigWilson.Blog\CLAPS -force
