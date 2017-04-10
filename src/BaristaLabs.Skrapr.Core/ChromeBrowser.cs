﻿namespace BaristaLabs.Skrapr
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a Chrome Browser instance.
    /// </summary>
    public class ChromeBrowser : IChromeBrowser, IDisposable
    {
        private Process m_chromeProcess;
        private readonly string m_chromeHost;
        private readonly int m_chromeDebuggingPort;
        private DirectoryInfo m_userDirectory;

        private ChromeBrowser(Process chromeProcess, DirectoryInfo userDirectory, string chromeHost, int chromeDebuggingPort)
        {
            m_chromeProcess = chromeProcess ?? throw new ArgumentNullException(nameof(chromeProcess));
            m_userDirectory = userDirectory ?? throw new ArgumentNullException(nameof(userDirectory));
            m_chromeHost = chromeHost;
            m_chromeDebuggingPort = chromeDebuggingPort;
        }

        /// <summary>
        /// Gets the Process object for the Chrome Browser
        /// </summary>
        public Process Process
        {
            get { return m_chromeProcess; }
        }

        /// <summary>
        /// Returns the version information of the chrome browser.
        /// </summary>
        /// <returns></returns>
        public async Task<ChromeVersion> GetChromeVersion()
        {
            ChromeVersion version;
            using (var chromeDebuggerClient = GetDebuggerClient())
            {
                var chromeVersionResponseBody = await chromeDebuggerClient.GetStringAsync("/json/version");
                version = JsonConvert.DeserializeObject<ChromeVersion>(chromeVersionResponseBody);
            }

            return version;
        }

        /// <summary>
        /// Returns a collection of sessions that are currently active on the chrome browser.
        /// </summary>
        /// <param name="chromeUrl"></param>
        /// <returns></returns>
        public async Task<ICollection<ChromeSessionInfo>> GetChromeSessions()
        {
            using (var chromeDebuggerClient = GetDebuggerClient())
            {
                var sessions = await chromeDebuggerClient.GetStringAsync("/json");
                return JsonConvert.DeserializeObject<ICollection<ChromeSessionInfo>>(sessions);
            }
        }

        /// <summary>
        /// Returns a HttpClient that can be used to interact with the current Chrome Browser.
        /// </summary>
        /// <returns></returns>
        private HttpClient GetDebuggerClient()
        {
            UriBuilder builder = new UriBuilder()
            {
                Host = m_chromeHost,
                Port = m_chromeDebuggingPort
            };

            var chromeHttpClient = new HttpClient()
            {
                BaseAddress = builder.Uri
            };

            return chromeHttpClient;
        }

        #region IDisposable Support
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Kill the chrome process.
                if (m_chromeProcess != null)
                {
                    if (m_chromeProcess.HasExited == false)
                        m_chromeProcess.WaitForExit(5000);

                    if (m_chromeProcess.HasExited == false)
                        m_chromeProcess.Kill();

                    m_chromeProcess.Dispose();
                    m_chromeProcess = null;
                }

                //Clean up the target user directory.
                if (m_userDirectory != null)
                {

                    //for (int i = 0; i < 10; i++)
                    //{
                    //    if (m_userDirectory.Exists == false)
                    //        continue;

                    //    try
                    //    {
                    //        Thread.Sleep(500);
                    //        m_userDirectory.Delete(true);
                    //    }
                    //    catch
                    //    {
                    //        //Do Nothing.
                    //    }
                    //}

                    //if (m_userDirectory.Exists)
                    //    throw new InvalidOperationException($"Unable to delete the user directory at {m_userDirectory.FullName} after 10 tries.");

                    m_userDirectory = null;
                }
            }
        }

        /// <summary>
        /// Closes the browser and clears any used resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        /// <summary>
        /// Creates a new chrome browser instance
        /// </summary>
        /// <param name="remoteDebuggingPort"></param>
        /// <returns></returns>
        public static ChromeBrowser LaunchChromeBrowser(string host = "localhost", int remoteDebuggingPort = 9222)
        {
            string path = Path.GetRandomFileName();
            var directoryInfo = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), path));
            var remoteDebuggingArg = $"--remote-debugging-port={remoteDebuggingPort}";
            var userDirectoryArg = $"--user-data-dir=\"{directoryInfo.FullName}\"";
            var chromeProcessArgs = $"{remoteDebuggingArg} {userDirectoryArg} --bwsi --no-first-run";

            Process chromeProcess;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                chromeProcess = Process.Start(new ProcessStartInfo(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", chromeProcessArgs) { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                chromeProcess = Process.Start("google-chrome", chromeProcessArgs);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                chromeProcess = Process.Start("/Applications/Google Chrome.app/Contents/MacOS/Google Chrome", chromeProcessArgs);
            }
            else
            {
                throw new InvalidOperationException("Unknown or unsupported platform.");
            }

            return new ChromeBrowser(chromeProcess, directoryInfo, host, remoteDebuggingPort);
        }
    }
}