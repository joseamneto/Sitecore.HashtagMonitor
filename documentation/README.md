# Documentation
Module developed by the GO HORSE TEAM for the Sitecore Hackathon 2018

## Category 
xConnect

## Purpose 
Identify personas and fire goals based on a twitter HashTag

## Detailed description
The Module automatically tracks all tweets for a specified #hashtag and identify accounts that interact with it and import them to xDB. 

This module helps marketers to identify the profile of each user along with their engagement with the #hashtag, 
e.g. if someone tweets on #SCHackthon, his informations are imported into xDB. Later, when he visits the website and fills in a Visit Us Form, then we can match the user to a given persona and show personalized content.

With this module marketers will be able to automate the interation with #hashtags on Twitter and use it in their engagement plans.

## Pre-requisites
- Sitecore 9 Update-1
- xConnect must be properly installed and configured (Sitecore 9 post build steps)

## Installation
1. Install Sitecore 9
2. Use the Sitecore Installation wizard to install the Sitecore HashTagMonitor module [package](#https://github.com/Sitecore-Hackathon/2018-Go-Horse/blob/master/sc.package/HashTagMonitor-1.0.zip) - In case of item conflicts choose Merge/Merge. In case of file conflicts, please overwrite them
3. Use the Sitecore Update Installation Wizard Page using the following url: http://[your Sitecore website here]/sitecore/admin/UpdateInstallationWizard.aspx to install the Example Web Sitecore [package](https://github.com/Sitecore-Hackathon/2018-Go-Horse/blob/master/sc.package/GoHorse_Sample_Site.update)
3.1 The Update has some steps 
3.2 Anaylze
![analyze step](images/analyze.png?raw=true "analyze step")
3.3 Install
![Install Package](images/installpackage.png?raw=true "Install Package")
3.4 Processing instalation
![Install Package](images/processing.png?raw=true "Install Package")
4. Configure one or multiple HashTags under /sitecore/system/Modules/HashTagMonitor. The module comes with a test #HashTag configured as #SCHackathon, triggering a sample goal and registering a sample Profile Card
5. Publish the website 
6. Rebuild all indexes (Control Panel ->  Index Manager -> Rebuild all indexes)
7. Rebuild Link Database (Control Panel ->  Database -> Rebuild link Database)
8. Deploy Marketing Definitions (Control Panel ->  Analytics  -> Deploy Marketing Definitions)

## Configuration
The module comes with a config file called "Sitecore.HashTagMonitor.config" where two configurations can be changes:
- HashTagMonitor.RepositoryPath - path to the repository of HashTags - Default: /sitecore/system/Modules/HashTagMonitor
- HashTagMonitor.Database - database to use during the process - Default: master

## Usage
The user has the ability to create multiple hashtags under /sitecore/system/Modules/HashTagMonitor

The image below show the configured HashTag
/sitecore/system/Modules/HashTagMonitor/Test/SCHackathon
![HashTag](images/configurehashtag.png?raw=true "Configure HashTag")

After the HashTag is defined, the user can run the Task Scheduler in order to import all the tweets into the xDB.
The image below the scheduler configured to be executed every 5 minutes.

![Task Scheduler](images/TaskScheduler.png?raw=true "Task Scheduler")

If you can't wait, Force the importation procedure by Firing the following url - but notice that at this first version, this should NOT be executed in publish mode. 
http://myur/api/sitecore/hash/Process

After you run the Process, go to "Experience Profile" and check the List of Contacts. The image below, shows all the Contacts created after the Task was run.

![Contacts](images/contacts.png?raw=true "Contacts")

Acessing the Website:
User should access the home at e.g. "gohorse.local", click on visit us call to action button, fill the form with his personal data and click on Submit. 

![Visit Us](images/VisitUs.png?raw=true "Visit Us")

He will be redirected to the home page and get a personalized message.

![Thanks for tweeting](images/ThanksForTweeting.jpg?raw=true "Thanks for tweeting")

## Video
[Click here to watch the module presentation on YouTube](https://youtu.be/2lEAazVlHUQ) 


