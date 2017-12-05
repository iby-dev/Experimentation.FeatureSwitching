using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Experimentation.FeatureSwitches.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Experimentation.FeatureSwitches
{
    public class FeatureSwitch
    {
        private string _url;
        private readonly IConfigurationRoot _config;
        private const string ApiNameKey = "ApiName";
        private const string MachineNameKey = "MachineName";
        private const string PortNumberKey = "PortNumber";
        private List<string> Switches { get; } = new List<string>();

        public FeatureSwitch() 
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _config = builder.Build();

            Init();
        }

        public FeatureSwitch(IConfigurationRoot configuration)
        {
            _config = configuration;
            Init();
        }

        private void Init()
        {
            // Hard coded because these are not likely to change.
            const string resource = "features";
            var apiName = _config[ApiNameKey];
            var machineName = _config[MachineNameKey];
            var portNumber = _config[PortNumberKey];

            _url = $"http://{machineName}:{portNumber}/{apiName}/{resource}";
        }

        public void OverrideSwitch(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name of the feature switch cannot be null, empty or contain whitespace.");
            }

            Switches.Add(name);
        }

        public async Task<FeatureModel> GetSwitchAsync(string name)
        {
            var fullyFormedUrl = $"{_url}/{name}";

            var client = new RestClient(fullyFormedUrl) {Encoding = Encoding.UTF8};

            var request = new RestRequest(Method.GET) {RequestFormat = DataFormat.Json};

            var result = await client.ExecuteTaskAsync<FeatureModel>(request);

            Debug.WriteLine(BuildLogMessage(result));

            return result.Data;
        }

        public FeatureModel GetSwitch(string name)
        {
            var fullyFormedUrl = $"{_url}/{name}";

            var client = new RestClient(fullyFormedUrl) {Encoding = Encoding.UTF8};

            var request = new RestRequest(Method.GET) {RequestFormat = DataFormat.Json};

            var result = client.Execute<FeatureModel>(request);

            Debug.WriteLine(BuildLogMessage(result));

            return result.Data;
        }

        public async Task<bool> IsSwitchEnabledAsync(string name)
        {
            if (Switches.Contains(name))
            {
                return true;
            }

            var result = await GetSwitchAsync(name);
            return result != null;
        }
        
        public bool IsSwitchEnabled(string name)
        {
            if (Switches.Contains(name))
            {
                return true;
            }
            
            var result = GetSwitch(name);
            return result != null;
        }

        public async Task<bool> IsPermittedOnSwitchAsync(string switchId, string idInBucket)
        {
            var fullyFormedUrl = $"{_url}/{switchId}/bucket/{idInBucket}";

            var client = new RestClient(fullyFormedUrl) {Encoding = Encoding.UTF8};

            var request = new RestRequest(Method.GET) {RequestFormat = DataFormat.Json};

            var result = await client.ExecuteTaskAsync<bool>(request);

            Debug.WriteLine(BuildLogMessage(result));

            return result.Data;
        }
        
        public bool IsPermittedOnSwitch(string switchId, string idInBucket)
        {
            var fullyFormedUrl = $"{_url}/{switchId}/bucket/{idInBucket}";

            var client = new RestClient(fullyFormedUrl) {Encoding = Encoding.UTF8};

            var request = new RestRequest(Method.GET) {RequestFormat = DataFormat.Json};

            var response = client.Execute<bool>(request);

            Debug.WriteLine(BuildLogMessage(response));

            return response.Data;
        }

        private string BuildLogMessage<T>(IRestResponse<T> result)
        {
            var msg = new StringBuilder("");
            msg.AppendLine($"Response: Code: {result.StatusCode}, Status: {result.ResponseStatus}");

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                msg.AppendLine($"Error: {result.ErrorMessage}");
                if (result.ErrorException != null)
                {
                    var m = $"{nameof(result.ErrorException)} - {result.ErrorException.Message}";
                    msg.AppendLine(m);
                }
            }

            if (!string.IsNullOrWhiteSpace(result.Content))
            {
                msg.AppendLine($"Data: {result.Data}");
            }
            return msg.ToString();
        }
    }
}
