using LogFusionX.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogFusionX.Core.Configurations
{
    public class XLogEntry
    {
        public string LogLevel { get; set; } = FusionXLoggerLevel.Info.ToString().ToUpper();
        public string? Message { get; set; }
        public string? ExceptionDetails { get; set; }
        public string? ThreadId { get; set; }
        public string? MethodName { get; set; }
        public DateTime? LogTimeStamp { get; set; }
    }
    /// <summary>
    /// Represents a detailed log entry for saving into the database.
    /// </summary>
    public class XDbLogEntry
    {
        // Unique Identifier
        public long fusion_x_seq { get; set; } // Primary Key: Unique identifier for the log entry

        // Core Log Details
        public string log_level { get; set; } = FusionXLoggerLevel.Info.ToString().ToUpper(); // e.g., INFO, ERROR
        public int log_severity { get; set; } // Numeric severity (e.g., 1 = Debug, 2 = Info, 3 = Warn, 4 = Error)
        public DateTime log_timestamp { get; set; } = DateTime.UtcNow; // Timestamp of the log in UTC
        public string? log_message { get; set; } // Main log message

        // Exception Details
        public string? exception_message { get; set; } // Exception message
        public string? exception_type { get; set; } // Type of exception (e.g., InvalidOperationException)
        public string? exception_stack_trace { get; set; } // Full stack trace of the exception

        // Contextual Information
        public string? thread_id { get; set; } // Identifier of the thread that generated the log
        public string? method_name { get; set; } // Name of the method where the log originated
        public string? class_name { get; set; } // Class name where the log originated
        public string? namespace_name { get; set; } // Namespace of the class
        public string? source_file_path { get; set; } // File path of the source code (if available)
        public int? source_line_number { get; set; } // Line number in the source file

        // Application & System Information
        public string? application_name { get; set; } // Name of the application generating the log
        public string? application_version { get; set; } // Version of the application
        public string? environment_name { get; set; } // Environment name (e.g., Production, Development)
        public string? host_name { get; set; } // Host name or machine name
        public string? operating_system { get; set; } // OS information (e.g., Windows, Linux, macOS)
        public string? framework_version { get; set; } // .NET runtime version (e.g., .NET 6.0)

        // Networking Information
        public string? client_ip { get; set; } // IP address of the client (if applicable)
        public string? server_ip { get; set; } // IP address of the server (if applicable)
        public int? port { get; set; } // Port number for the server (if applicable)
        public string? url { get; set; } // URL or endpoint that caused the log (for web apps)
        public string? http_method { get; set; } // HTTP method (e.g., GET, POST)

        // User Information
        public string? user_id { get; set; } // ID of the user who caused the log
        public string? user_name { get; set; } // Username of the user who caused the log
        public string? user_role { get; set; } // Role of the user (e.g., Admin, Guest)

        // Request and Transaction Correlation
        public string? correlation_id { get; set; } // Unique correlation ID for tracing requests
        public string? transaction_id { get; set; } // Transaction ID for grouping related logs

        // Performance and Metrics
        public double? execution_time_ms { get; set; } // Execution time in milliseconds (if applicable)
        public long? memory_usage_kb { get; set; } // Memory usage in KB at the time of the log
        public long? cpu_usage_percentage { get; set; } // CPU usage percentage (if available)

        // Security Information
        public string? authentication_type { get; set; } // Authentication type (e.g., OAuth, JWT)
        public bool is_authenticated { get; set; } // Whether the user is authenticated
        public string? client_user_agent { get; set; } // Client's user-agent string (e.g., browser info)

        // Custom Metadata
        public string? log_category { get; set; } // Category or group of the log (e.g., Database, UI, API)
        public string? tags { get; set; } // Tags for categorization (comma-separated)
        public string? custom_data { get; set; } // Custom metadata in JSON or string format

        // Log Partitioning or Retention
        public string? log_partition_key { get; set; } // Partition key for log storage optimization
        public DateTime? log_retention_date { get; set; } // Date until which the log should be retained
        public XDbLogEntry CreateLogEntry(long fusion_x_seq, string log_level = "INFO", int log_severity = 1, DateTime? log_timestamp = null, string? log_message = null, string? exception_message = null, string? exception_type = null, string? exception_stack_trace = null, string? thread_id = null, string? method_name = null,
    string? class_name = null, string? namespace_name = null, string? source_file_path = null, int? source_line_number = null, string? application_name = null,
    string? application_version = null, string? environment_name = null, string? host_name = null, string? operating_system = null, string? framework_version = null,
    string? client_ip = null, string? server_ip = null, int? port = null, string? url = null, string? http_method = null, string? user_id = null, string? user_name = null, string? user_role = null, string? correlation_id = null, string? transaction_id = null, double? execution_time_ms = null, long? memory_usage_kb = null, long? cpu_usage_percentage = null, string? authentication_type = null, bool is_authenticated = false, string? client_user_agent = null, string? log_category = null, string? tags = null, string? custom_data = null, string? log_partition_key = null, DateTime? log_retention_date = null)
        {
            return new XDbLogEntry
            {
                fusion_x_seq = fusion_x_seq,
                log_level = log_level,
                log_severity = log_severity,
                log_timestamp = log_timestamp ?? DateTime.UtcNow,
                log_message = log_message,
                exception_message = exception_message,
                exception_type = exception_type,
                exception_stack_trace = exception_stack_trace,
                thread_id = thread_id,
                method_name = method_name,
                class_name = class_name,
                namespace_name = namespace_name,
                source_file_path = source_file_path,
                source_line_number = source_line_number,
                application_name = application_name,
                application_version = application_version,
                environment_name = environment_name,
                host_name = host_name,
                operating_system = operating_system,
                framework_version = framework_version,
                client_ip = client_ip,
                server_ip = server_ip,
                port = port,
                url = url,
                http_method = http_method,
                user_id = user_id,
                user_name = user_name,
                user_role = user_role,
                correlation_id = correlation_id,
                transaction_id = transaction_id,
                execution_time_ms = execution_time_ms,
                memory_usage_kb = memory_usage_kb,
                cpu_usage_percentage = cpu_usage_percentage,
                authentication_type = authentication_type,
                is_authenticated = is_authenticated,
                client_user_agent = client_user_agent,
                log_category = log_category,
                tags = tags,
                custom_data = custom_data,
                log_partition_key = log_partition_key,
                log_retention_date = log_retention_date
            };
        }
    }
}
