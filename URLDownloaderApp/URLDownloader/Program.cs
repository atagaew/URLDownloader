namespace URLDownloader
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                Console.WriteLine("Provide a file name containing URLs. Example: URLDownloader.exe links.txt");
                return;
            }

            var fileName = args[0];

            if (!File.Exists(fileName))
            {
                Console.WriteLine("File not found.");
                return;
            }

            var urls = await File.ReadAllLinesAsync(fileName);
            if (!urls.Any())
            {
                Console.WriteLine("File doesn't contain any URL.");
                return;
            }

            await ProcessURLsAsync(urls);
        }

        private static async Task ProcessURLsAsync(string[] urls)
        {
            try
            {
                var directoryName = CreateDirectory();

                int maxParallelDownloads = 10;

                using SemaphoreSlim semaphore = new(maxParallelDownloads);
                using HttpClient httpClient = new();
                List<Task> downloadTasks = new();

                foreach (string url in urls)
                {
                    await semaphore.WaitAsync();
                    downloadTasks.Add(DownloadFileAsync(url, directoryName, httpClient, semaphore));
                }

                await Task.WhenAll(downloadTasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("All files downloaded successfully.");
        }

        private static async Task DownloadFileAsync(string url, string directoryName, HttpClient httpClient, SemaphoreSlim semaphore)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string fileNameFromUrl = Path.GetFileName(url);
                    string filePath = Path.Combine(directoryName, fileNameFromUrl);

                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }

                    Console.WriteLine($"Downloaded {fileNameFromUrl}");
                }
                else
                {
                    Console.WriteLine($"Failed to download {url}. Status code: {response.StatusCode}");
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private static string CreateDirectory()
        {
            var timeStamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var directoryName = $"Download_{timeStamp}";
            Directory.CreateDirectory(directoryName);

            return directoryName;
        }
    }

}
