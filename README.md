# LogFusionX

LogFusionX is a powerful logging library designed to handle different log levels, formats, and logging scenarios. It provides synchronous logging capabilities, including the ability to log to files, handle exceptions, and log warning and error messages in a straightforward manner.

## Features

- **Multiple Log Levels**: Support for `Info`, `Warning`, `Error`, and custom log levels.
- **File Logging**: Logs can be written to files with flexible file and directory options.
- **Exception Handling**: Log errors with detailed exception stack traces.
- **Synchronous Logging**: Log entries are written synchronously, ensuring that messages are recorded in the correct order.

## Installation

To install `LogFusionX`, use the following command:
```bash
dotnet add package LogFusionX --version 0.1.0
````
This will add the latest version of `LogFusionX` to your project.

## Usage

To use `LogFusionX` for logging, you can follow the example below:

### Example Code

```csharp
using System;
using System.IO;
using LogFusionX.Core.Configurations;
using LogFusionX.Core.Loggers;

class Program
{
    static void Main(string[] args)
    {
        // Set up the logger configuration
        var options = new XFileLoggerConfigurationOptions
        {
            LogDirectory = Directory.GetCurrentDirectory() + "/AppData", // Log file directory
            LogFileName = "FusionLogs" // Log file name
        };

        // Initialize the logger with the configuration options
        var logger = new FusionXLogger(options);

        try
        {
            for (int i = 0; i < 10; i++)
            {
                // Log info messages
                if (i == 0)
                {
                    logger.Log($"Hello, this is a sync log message {i} - {DateTime.Now}, by LogFusionX");
                }
                // Log warning messages
                else if (i == 1)
                {
                    logger.LogWarning("Hi, this is a warning message", new Exception("Warning exception"));
                }
                // Simulate an error
                else
                {
                    throw new Exception("Simulated error");
                }
            }
        }
        catch (Exception ex)
        {
            // Log error messages with exception details
            logger.LogError("An error occurred while processing the loop. Please try again later.", ex);
        }

        // Synchronous Logging
        //logger.WriteLog("This is a synchronous log entry.");

        Console.WriteLine("Logs written. Press any key to exit...");
        Console.ReadKey();
    }
}
````

## Explanation:

### Logger Configuration
The `XFileLoggerConfigurationOptions` class is used to configure the logger. You can set the log file directory (`LogDirectory`) and the log file name (`LogFileName`).

### Logging Levels:
- **Info**: Logs general information.
- **Warning**: Logs warning messages.
- **Error**: Logs error messages along with exception details.

### Exception Handling
If an exception occurs during logging, it is caught and logged using `LogError`.

### Synchronous Logging
LogFusionX allows synchronous logging, ensuring that messages are written in the correct sequence.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.

## Contributing
Feel free to fork the repository, submit issues, or make pull requests to contribute to the development of LogFusionX.

## Repository
You can find the project repository and source code here:  
[LogFusionX GitHub Repository](https://github.com/goldi0002/LogFusionX)

---

### Thank you for using LogFusionX! 🎉
We hope this library makes logging in your .NET projects more powerful and easier. If you have any feedback, questions, or suggestions, feel free to reach out. Stay tuned for more features and updates in the future!

Happy Coding! 🚀
