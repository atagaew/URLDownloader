# URL Downloader

## Overview

This console application downloads files from URLs listed in a text file in parallel threads. The downloaded files are stored in a timestamped directory.

## Usage

### Prerequisites

- [.NET 6.0 or later](https://dotnet.microsoft.com/download)

### Steps to Use

1. **Clone the Repository**

   Clone this repository using the following command:

   ```bash
   git clone https://github.com/atagaew/URLDownloader.git

2. **Navigate to the Project Directory**

   ```bash
   cd URLDownloader

3. **Build the Application**

   Execute the following command to build the application:

   ```bash
   dotnet build -c Release

4. **Run the Application**

   To use the program, execute the built executable file along with the file name containing the URLs:

   ```bash
   dotnet run -- -f <file_name_with_urls>
   
   Replace <file_name_with_urls> with the path to your file containing URLs.
   Example:

   ```bash
   dotnet run -- -f links.txt

   Ensure the file contains one URL per line.

5. **Check Downloads**

   After execution, the downloaded files will be saved in a newly created directory named with a timestamp in the format Download_yyyy-MM-dd-HH-mm-ss.






