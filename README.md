# LogFusionX

LogFusionX is a powerful logging library designed to handle different log levels, formats, and logging scenarios. It provides synchronous logging capabilities, including the ability to log to files, handle exceptions, and log warning and error messages in a straightforward manner.


### Features of LogFusionX
- **Structured Logging**: Supports JSON formatting for structured log data.
- **Security Focus**: Highlights suspicious or unauthorized actions in the system.
- **Customizability**: Allows defining custom log levels and extending functionality.

## Installation

To install `LogFusionX`, use the following command:
```bash
dotnet add package LogFusionX --version 0.1.1
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
The `LogFusionXConfigurationOptions` class is used to configure the logger in LogFusionX. Key properties include:
- **LogDirectory**: Specifies the directory where log files will be stored.
- **LogFileName**: Allows you to set a custom name for the log file. Default names can follow a date-based pattern for easy organization.
- **EnableConsoleLogging**: Enables or disables logging to the console for debugging or local development.
- **MaxLogFileSize**: Defines the maximum size of a log file before it rolls over to a new file.

Example configuration:
```csharp
var config = new LogFusionXConfigurationOptions
{
    LogDirectory = "C:\\Logs",
    LogFileName = "AppLogs.log",
    EnableConsoleLogging = true,
    MaxLogFileSize = 5 * 1024 * 1024 // 5 MB
};
```
---
## Logging Levels in LogFusionX

LogFusionX supports a wide range of logging levels to handle different types of application scenarios effectively:

### **1. Info**
Logs general information, such as application startup, process completion, or routine activities.

Example:
```plaintext
"Application started successfully at 12:30 PM"
```

### **2. Warning**
Logs potential issues or unexpected behavior that does not stop the application but needs attention.

Example:
```plaintext
"Memory usage exceeded 80%, consider optimizing"
```

### **3. Error**
Logs application errors, such as unhandled exceptions or invalid operations. Includes details about the exception (e.g., stack trace).

Example:
```plaintext
"Database connection failed: TimeoutException at ..."
```

### **4. Trace**
Logs detailed, step-by-step execution for debugging purposes, useful during development or troubleshooting.

Example:
```plaintext
"Entering method ProcessOrder at 12:31:05 PM"
```

### **5. Fatal**
Logs critical failures that cause the application to crash or become unusable. Typically used for catastrophic events.

Example:
```plaintext
"System crash: OutOfMemoryException - shutting down"
```

### **6. Performance**
Logs performance-related metrics to monitor response times or identify bottlenecks in the application.

Example:
```plaintext
"API response time: 350ms for /getOrderDetails"
```

### **7. Security**
Logs security-related events, such as failed authentication attempts, unauthorized access, or policy violations.

Example:
```plaintext
"Unauthorized access attempt detected: IP 192.168.1.101"
```

### **8. Custom (Extendable)**
LogFusionX allows defining custom log levels tailored to specific organizational needs. Examples include:

- **Audit**: Tracks user actions or data modifications for compliance.
- **Critical**: Highlights issues that may not crash the application but need immediate attention.

Example:
```plaintext
"User JohnDoe updated record #1234 at 2:15 PM."
```

---
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
