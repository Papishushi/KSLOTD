# KSLOTD
Open Source Key stroke logger for Windows Devices. See [license](https://github.com/Papishushi/KSLOTD/blob/main/LICENSE) for more details.

This piece of .NET software is designed with educative intentions on the field of malware coding mechanisms.This is usefull to the community by, for example: creating algorithims capable of picking this patterns and block the malware before an attack is fullfilled. 

I dont accept any responsibility or liability for the use of this licensed work.

## v1.0 Features:
* Keylogger

* Spyware send emails with log and computer data

* Autoruns setup

* Only displayed on Task Manager


## Usage and Installation:

To use this malware first you have to download the source code compossed of the files:

1. _SysMainProcess.cs_
2. _SysMainProcess.Constants.cs_
3. _SysMainProcess.ico_

Then modify _SysMainProcess.Constants.cs_ with the required parameters:

`private const ushort SMTP_PORT = Your SMTP port;`  
`private const uint PODCAST_INTERVAL = Interval time in miliseconds at which a podcast is broadcasted;`  
`private static readonly string[] PODCAST_EMISSOR_CREDENTIALS = { "EMISSOR MAIL", "PASSWORD" };`  
`private const string PODCAST_RECEIVER = "RECEIVER MAIL";`  
`private const ushort THREAD_SLEEP_TIME = Time in miliseconds the thread is sleeping;`  
`private const ushort MIN_ASCII_CODE = Minimun ASCII code used;`  
`private const ushort MAX_ASCII_CODE = Maximun ASCII code used;`  
`private const string REGISTRY_FILE_NAME = "YourRegistryFileName.dll";` 

And to finalize compile the .NET Framework 4.0 project using some IDE like Visual Studio Community, and then run the result exe file on your test destination, it should be located in the \bin folder of your project.

Congratulations now you own your own copy of this malware!

If you decide to modidy, distribute, fork or use commercially this malware you must declare the source code publicly, and license it under the [GNU General Public License v3.0](https://github.com/Papishushi/KSLOTD/blob/main/LICENSE).  

If you use this tool for nefarius purpose as I said "_I dont accept any responsibility or liability for the use of this licensed work._"

Cheers
