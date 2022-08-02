using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;

namespace ExpandoFeedTransformer
{
    public class PohodaMServer
{
         #region Variables
        private readonly string serverName;
        private readonly string pathToServer;
        private readonly string serverUrl;
        private readonly string username;
        private readonly string password;
        private readonly short retryDelay;
        private short amountOfRetries;
        private HttpClient httpClient;
        #endregion

        #region Constructor
        public PohodaMServer(string serverName, string pathToServer, string serverUrl, string username, string password, short retryDelay, short amountOfRetries)
        {
            this.serverName = serverName;
            this.pathToServer = pathToServer;
            this.serverUrl = serverUrl;
            this.username = username;
            this.password = password;
            this.retryDelay = retryDelay;
            this.amountOfRetries = amountOfRetries;
        }
        #endregion

        #region PublicMethods
        public void StartServer()
        {
            // IDEA: maybe add check if connection is available in case the server is still running
            var mServerStartCommand = $"cd \"{pathToServer}\" & pohoda.exe /http start {serverName}";

            CreateCommand(mServerStartCommand);
        }

        public async Task<bool> IsConnectionAvailable()
        {
            // TODO: load httpClient from factory 
            httpClient = new HttpClient { BaseAddress = new Uri(serverUrl) };
            httpClient.DefaultRequestHeaders.Add("STW-Authorization", CreateAuthHeader());
            httpClient.DefaultRequestHeaders.Add("Accept", "text/xml");

            var responseCode = HttpStatusCode.BadRequest;

            while (responseCode != HttpStatusCode.OK && amountOfRetries >= 0)
            {
                amountOfRetries--;

                try
                {
                    using var response = await httpClient.GetAsync("/status");
                    responseCode = response.StatusCode;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("Error: Couldn't connect to mServer \nResponseCode" + response.StatusCode);
                        continue;
                    }

                    var xmlDoc = new XmlDocument();
                    var responseString = await response.Content.ReadAsStringAsync();
                    xmlDoc.LoadXml(responseString);
                    var actualServerName = xmlDoc.GetElementsByTagName("name")[0]?.InnerText;

                    if (actualServerName != null && !actualServerName.Equals(serverName))
                    {
                        return false;
                    }

                    Console.WriteLine("mServer is running on " + serverName);
                    return true;

                }
                catch (HttpRequestException)
                {
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Couldn't connect to server.");
                    return false;
                }

                await Task.Delay(retryDelay);
            }

            Console.WriteLine("Couldn't connect to the server");

            return false;
        }

        public async Task<string> SendRequest(string body)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, "/xml");
            message.Content = new ByteArrayContent(Encoding.ASCII.GetBytes(body));
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            message.Headers.Add("Content-Encoding", "utf-8");
            var response =  await httpClient.SendAsync(message);
            return await response.Content.ReadAsStringAsync();
        }

        public void StopServer()
        {
            httpClient.Dispose();
            var mServerStopCommand = $"cd \"{pathToServer}\" & pohoda.exe /http stop {serverName}";
            CreateCommand(mServerStopCommand);
        }

        #endregion

        #region PrivateMethods
        private void CreateCommand(string command)
        {
            var cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = false;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
        }

        private string CreateAuthHeader()
        {
            return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        }
        #endregion
}
}