using System;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Security.Cryptography;
using AzureStorageHandler;

namespace QueueOperations
{
    class Program
    {
        static void Main()
        {
            //CreateDemoData();
            GetNextMessage();
            EnCryptDecrypt.CryptorEngine.Decrypt();

            JobHost host = new JobHost();
            host.Start();

            // Stop the host if Ctrl + C/Ctrl + Break is pressed
            Console.CancelKeyPress += (sender, args) =>
            {
                host.Stop();
            };

            while (true)
            {
                Thread.Sleep(500);
            }


            

        }

        private static void CreateDemoData()
        {

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=inovafiles;AccountKey=DIAf5tmIX05NzqK8kFEjbr1eU18Fk1AmzTmvolCu5iUnhdT73amRelFeNjUH+yp4z4Y60G1YA8B8wV6gHbl2bg=="; // AmbientConnectionStringProvider.Instance.GetConnectionString(ConnectionStringNames.Storage);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("workerroletest");
            queue.CreateIfNotExists();

            InovaQueue person = new InovaQueue()
            {
                Name = "sujith",
                OrderId = Guid.NewGuid().ToString("N").ToLower()
            };

            queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(person)));

        }

        private static void GetNextMessage()
        {

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=inovafiles;AccountKey=DIAf5tmIX05NzqK8kFEjbr1eU18Fk1AmzTmvolCu5iUnhdT73amRelFeNjUH+yp4z4Y60G1YA8B8wV6gHbl2bg=="; // AmbientConnectionStringProvider.Instance.GetConnectionString(ConnectionStringNames.Storage);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("thisqueue");
            queue.CreateIfNotExists();

            // Get the next message
            CloudQueueMessage retrievedMessage = queue.GetMessage();

            Console.WriteLine("Retrieved message with content '{0}'", retrievedMessage.AsString);

            //Process the message in less than 30 seconds, and then delete the message
            queue.DeleteMessage(retrievedMessage);



        }


    }
}
