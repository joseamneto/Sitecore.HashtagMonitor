#define parameters 

# Installation prefix - all devs use "th"
$prefix = "gohorse"

# This is for when multiple devs must have the same URL (Eg: th.local) but different SQL prefixes
# For Local SQL Server $developer should be empty, so DBs will have $prefix
# For centralized SQL Server should use different DB names, so $instance must be filled (initials - jh, nc, rd) PLEASE FILL OUT!!!!
$instance = ""
$sqlPrefix = "$prefix$instance" # Don't touch this

# Needs to point to a folder worked out as 
$PSScriptRoot = "C:\SitecoreInstaller\Packages\Sitecore 9.0.1 rev. 171219" 

# SOLR Setup - your SOLR should be running (check your services window)
$SolrUrl = "https://localhost:8983/solr" 
$SolrRoot = "C:\solr\solr-6.6.2" 
$SolrService = "solr-6.6.2" 

# SQL Setup - Must correctly point to an Admin user (the admin user is only used for the installation and can be removed later)
$SqlServer = "localhost" 
$SqlAdminUser = "sa" 
$SqlAdminPassword= "sa" 

# Instance Names 
$XConnectCollectionService = "$prefix.xconnect" 
$sitecoreSiteName = "$prefix.local" 
 
#install client certificate for xconnect 
$certParams = @{     
	Path = "$PSScriptRoot\xconnect-createcert.json"     
	CertificateName = "$prefix.xconnect_client" 
} 
Install-SitecoreConfiguration @certParams -Verbose 
 
#install solr cores for xdb 
$solrParams = @{     
	Path = "$PSScriptRoot\xconnect-solr.json"     
	SolrUrl = $SolrUrl     
	SolrRoot = $SolrRoot     
	SolrService = $SolrService     
	CorePrefix = $prefix 
} 
Install-SitecoreConfiguration @solrParams 
 
#deploy xconnect instance 
$xconnectParams = @{     
	Path = "$PSScriptRoot\xconnect-xp0.json"     
	Package = "$PSScriptRoot\Sitecore 9.0.1 rev. 171219 (OnPrem)_xp0xconnect.scwdp.zip"     
	LicenseFile = "$PSScriptRoot\license.xml"     
	Sitename = $XConnectCollectionService     
	XConnectCert = $certParams.CertificateName     
	SqlDbPrefix = $sqlPrefix
	SqlServer = $SqlServer  
	SqlAdminUser = $SqlAdminUser     
	SqlAdminPassword = $SqlAdminPassword     
	SolrCorePrefix = $prefix     
	SolrURL = $SolrUrl      
} 
Install-SitecoreConfiguration @xconnectParams 
 
#install solr cores for sitecore 
$solrParams = @{     
	Path = "$PSScriptRoot\sitecore-solr.json"     
	SolrUrl = $SolrUrl     
	SolrRoot = $SolrRoot     
	SolrService = $SolrService     
	CorePrefix = $prefix 
} 
Install-SitecoreConfiguration @solrParams 
 
#install sitecore instance 
$xconnectHostName = "$prefix.xconnect" 
$sitecoreParams = @{     
	Path = "$PSScriptRoot\sitecore-XP0.json"     
	Package = "$PSScriptRoot\Sitecore 9.0.1 rev. 171219 (OnPrem)_single.scwdp.zip"     
	LicenseFile = "$PSScriptRoot\license.xml"     
	SqlDbPrefix = $sqlPrefix  
	SqlServer = $SqlServer  
	SqlAdminUser = $SqlAdminUser     
	SqlAdminPassword = $SqlAdminPassword     
	SolrCorePrefix = $prefix  
	SolrUrl = $SolrUrl     
	XConnectCert = $certParams.CertificateName     
	Sitename = $sitecoreSiteName         
	XConnectCollectionService = "https://$XConnectCollectionService"    
} 
Install-SitecoreConfiguration @sitecoreParams