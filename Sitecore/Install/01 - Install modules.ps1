# Add the Sitecore MyGet repository to PowerShell
Register-PSRepository -Name SitecoreGallery -SourceLocation https://sitecore.myget.org/F/sc-powershell/api/v2

# Install the Sitecore Install Framwork module
Install-Module SitecoreInstallFramework -Force

# Install the Sitecore Fundamentals module (provides additional functionality for local installations like creating self-signed certificates)
Install-Module SitecoreFundamentals -Force

# Import the modules into your current PowerShell context (if necessary)
Import-Module SitecoreFundamentals
Import-Module SitecoreInstallFramework