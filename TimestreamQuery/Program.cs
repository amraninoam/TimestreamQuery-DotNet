using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.TimestreamQuery;
using Amazon.TimestreamQuery.Model;
using Amazon.Runtime.CredentialManagement;

namespace TimestreamQueryExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                AmazonTimestreamQueryClient client;

                var dbName = Environment.GetEnvironmentVariable("dbName") ?? "replaceme"; 
                var tableName = Environment.GetEnvironmentVariable("tableName") ?? "replaceme";
                var profileName = Environment.GetEnvironmentVariable("profile");
                var rows = Environment.GetEnvironmentVariable("rows") ?? "1";
                var serviceURL = Environment.GetEnvironmentVariable("serviceURL");
                 
                var queryString = $"SELECT * FROM \"{dbName}\".\"{tableName}\" LIMIT {rows}";

                AmazonTimestreamQueryConfig config;

                if (!string.IsNullOrEmpty(serviceURL))
                {
                    config = new AmazonTimestreamQueryConfig
                    {
                        EndpointDiscoveryEnabled = false,
                        ServiceURL = serviceURL
                    };
                }
                else
                {
                    config = new AmazonTimestreamQueryConfig
                    {
                        EndpointDiscoveryEnabled = true,
                        RegionEndpoint = RegionEndpoint.EUWest1
                    };
                }

                if (!string.IsNullOrEmpty(profileName))
                {
                    // Use credentials from the specified profile
                    var chain = new CredentialProfileStoreChain();
                    if (!chain.TryGetAWSCredentials(profileName, out AWSCredentials credentials))
                    {
                        throw new Exception($"Failed to get AWS credentials from the profile: {profileName}");
                    }

                    client = new AmazonTimestreamQueryClient(credentials, config);
                }
                else
                {
                    client = new AmazonTimestreamQueryClient(config);
                }

                var queryRequest = new QueryRequest
                {
                    QueryString = queryString
                };

                Amazon.AWSConfigs.LoggingConfig.LogResponses = Amazon.ResponseLoggingOption.Always;
                Amazon.AWSConfigs.LoggingConfig.LogTo = Amazon.LoggingOptions.SystemDiagnostics;
                Amazon.AWSConfigs.AddTraceListener("Amazon", new System.Diagnostics.ConsoleTraceListener());

                var queryResponse = await client.QueryAsync(queryRequest);

                if (queryResponse.Rows.Count > 0)
                {
                    foreach (var row in queryResponse.Rows)
                    {
                        foreach (var datum in row.Data)
                        {
                            Console.Write($"{datum.ScalarValue} ");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("No rows returned.");
                }
            }
            catch (AmazonTimestreamQueryException e)
            {
                Console.WriteLine($"AWS Timestream Query Error: {e.Message}");
                Console.WriteLine($"Error Code: {e.ErrorCode}");
                Console.WriteLine($"Error Type: {e.ErrorType}");
                Console.WriteLine($"Request ID: {e.RequestId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error querying Timestream: {ex.Message}");
            }
        }
    }
}
