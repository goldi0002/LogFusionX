using LogFusionX.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LogFusionX.Core.Security
{
    [Serializable]
    [DebuggerStepThrough]
    internal class XsqlServer
    {
        private readonly string _connectionString;
        private readonly string _userDefinedTableName;
        [DebuggerHidden]
        [DebuggerStepThrough]
        public XsqlServer(string connectionString, string userDefinedTableName)
        {
            _connectionString = connectionString;
            _userDefinedTableName = userDefinedTableName;
        }
        [DebuggerStepThrough]
        public bool InsertLog(XDbLogEntry logEntry)
        {
            try
            {
                if (!string.IsNullOrEmpty(_userDefinedTableName) && CreateTable())
                {
                    string query = CreateInsertQuery();
                    if (!string.IsNullOrEmpty(query))
                    {
                        //using (SqlConnection connection = new SqlConnection(_connectionString))
                        //{
                        //    connection.Open();
                        //    using (SqlCommand command = new SqlCommand(query, connection))
                        //    {
                        //        command.Parameters.AddWithValue("@log_level", IsFieldNull(logEntry.log_level));
                        //        command.Parameters.AddWithValue("@log_severity", IsFieldNull(logEntry.log_severity));
                        //        command.Parameters.AddWithValue("@log_timestamp", IsFieldNull(logEntry.log_timestamp));
                        //        command.Parameters.AddWithValue("@log_message", IsFieldNull(logEntry.log_message));
                        //        command.Parameters.AddWithValue("@exception_message", IsFieldNull(logEntry.exception_message));
                        //        command.Parameters.AddWithValue("@exception_type", IsFieldNull(logEntry.exception_type));
                        //        command.Parameters.AddWithValue("@exception_stack_trace", IsFieldNull(logEntry.exception_stack_trace));
                        //        command.Parameters.AddWithValue("@thread_id", IsFieldNull(logEntry.thread_id));
                        //        command.Parameters.AddWithValue("@method_name", IsFieldNull(logEntry.method_name));
                        //        command.Parameters.AddWithValue("@class_name", IsFieldNull(logEntry.class_name));
                        //        command.Parameters.AddWithValue("@namespace_name", IsFieldNull(logEntry.namespace_name));
                        //        command.Parameters.AddWithValue("@source_file_path", IsFieldNull(logEntry.source_file_path));
                        //        command.Parameters.AddWithValue("@source_line_number", IsFieldNull(logEntry.source_line_number));
                        //        command.Parameters.AddWithValue("@application_name", IsFieldNull(logEntry.application_name));
                        //        command.Parameters.AddWithValue("@application_version", IsFieldNull(logEntry.application_version));
                        //        command.Parameters.AddWithValue("@environment_name", IsFieldNull(logEntry.environment_name));
                        //        command.Parameters.AddWithValue("@host_name", IsFieldNull(logEntry.host_name));
                        //        command.Parameters.AddWithValue("@operating_system", IsFieldNull(logEntry.operating_system));
                        //        command.Parameters.AddWithValue("@framework_version", IsFieldNull(logEntry.framework_version));
                        //        command.Parameters.AddWithValue("@client_ip", IsFieldNull(logEntry.client_ip));
                        //        command.Parameters.AddWithValue("@server_ip", IsFieldNull(logEntry.server_ip));
                        //        command.Parameters.AddWithValue("@port", IsFieldNull(logEntry.port));
                        //        command.Parameters.AddWithValue("@url", IsFieldNull(logEntry.url));
                        //        command.Parameters.AddWithValue("@http_method", IsFieldNull(logEntry.http_method));
                        //        command.Parameters.AddWithValue("@user_id", IsFieldNull(logEntry.user_id));
                        //        command.Parameters.AddWithValue("@user_name", IsFieldNull(logEntry.user_name));
                        //        command.Parameters.AddWithValue("@user_role", IsFieldNull(logEntry.user_role));
                        //        command.Parameters.AddWithValue("@correlation_id", IsFieldNull(logEntry.correlation_id));
                        //        command.Parameters.AddWithValue("@transaction_id", IsFieldNull(logEntry.transaction_id));
                        //        command.Parameters.AddWithValue("@execution_time_ms", IsFieldNull(logEntry.execution_time_ms));
                        //        command.Parameters.AddWithValue("@memory_usage_kb", IsFieldNull(logEntry.memory_usage_kb));
                        //        command.Parameters.AddWithValue("@cpu_usage_percentage", IsFieldNull(logEntry.cpu_usage_percentage));
                        //        command.Parameters.AddWithValue("@authentication_type", IsFieldNull(logEntry.authentication_type));
                        //        command.Parameters.AddWithValue("@is_authenticated", logEntry.is_authenticated);
                        //        command.Parameters.AddWithValue("@client_user_agent", IsFieldNull(logEntry.client_user_agent));
                        //        command.Parameters.AddWithValue("@log_category", IsFieldNull(logEntry.log_category));
                        //        command.Parameters.AddWithValue("@tags", IsFieldNull(logEntry.tags));
                        //        command.Parameters.AddWithValue("@custom_data", IsFieldNull(logEntry.custom_data));
                        //        command.Parameters.AddWithValue("@log_partition_key", IsFieldNull(logEntry.log_partition_key));
                        //        command.Parameters.AddWithValue("@log_retention_date", IsFieldNull(logEntry.log_retention_date));
                        //        command.ExecuteNonQuery();
                        //    }
                        //    connection.Close();
                        //}
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        [DebuggerHidden]
        [DebuggerStepThrough]
        private object IsFieldNull<T>(T item)
        {
            if (item == null)
            {
                return DBNull.Value;
            }
            if (item is string str && string.IsNullOrEmpty(str))
            {
                return DBNull.Value;
            }
            return item.ToString();
        }
        [DebuggerHidden]
        [DebuggerStepThrough]
        private bool DoesTableExist()
        {
            if (!string.IsNullOrEmpty(_userDefinedTableName))
            {
                //string query = @"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName)
                //BEGIN  SELECT 1 END  ELSE BEGIN SELECT 0 END";
                //using (SqlConnection connection = new SqlConnection(_connectionString))
                //{
                //    connection.Open();
                //    using (SqlCommand command = connection.CreateCommand())
                //    {
                //        command.CommandText = query;
                //        command.Parameters.AddWithValue("@TableName", _userDefinedTableName);
                //        return (int)command.ExecuteScalar() == 1;
                //    }
                //}
            }
            return false;
        }
        [DebuggerHidden]
        [DebuggerStepThrough]
        private bool CreateTable()
        {
            if (DoesTableExist())
            {
                return true;
            }
            else
            {
                string tableQuery = $@"CREATE TABLE {_userDefinedTableName} (
                    fusion_x_seq BIGINT PRIMARY KEY IDENTITY(1,1),           -- Unique Identifier for the log entry
                    log_level NVARCHAR(50) NOT NULL,                           -- Log Level (e.g., INFO, ERROR)
                    log_severity INT NOT NULL,                                 -- Numeric severity (e.g., 1 = Debug, 2 = Info, etc.)
                    log_timestamp DATETIME NOT NULL DEFAULT GETUTCDATE(),      -- Log Timestamp in UTC
                    log_message NVARCHAR(MAX),                                 -- Main Log Message
                    exception_message NVARCHAR(MAX),                           -- Exception Message
                    exception_type NVARCHAR(100),                              -- Exception Type (e.g., InvalidOperationException)
                    exception_stack_trace NVARCHAR(MAX),                       -- Exception Stack Trace
                    thread_id NVARCHAR(50),                                    -- Thread ID
                    method_name NVARCHAR(100),                                  -- Method Name where the log originated
                    class_name NVARCHAR(100),                                   -- Class Name where the log originated
                    namespace_name NVARCHAR(100),                               -- Namespace of the class
                    source_file_path NVARCHAR(255),                             -- Source File Path (if available)
                    source_line_number INT,                                     -- Line Number in the Source File
                    application_name NVARCHAR(100),                             -- Application Name
                    application_version NVARCHAR(50),                           -- Application Version
                    environment_name NVARCHAR(50),                              -- Environment Name (e.g., Production, Development)
                    host_name NVARCHAR(100),                                    -- Host Name
                    operating_system NVARCHAR(100),                             -- Operating System (e.g., Windows, Linux)
                    framework_version NVARCHAR(50),                             -- Framework Version (e.g., .NET 6.0)
                    client_ip NVARCHAR(50),                                     -- Client IP Address
                    server_ip NVARCHAR(50),                                     -- Server IP Address
                    port INT,                                                   -- Port Number (if applicable)
                    url NVARCHAR(MAX),                                          -- URL or endpoint (for web apps)
                    http_method NVARCHAR(10),                                   -- HTTP Method (e.g., GET, POST)
                    user_id NVARCHAR(50),                                       -- User ID who triggered the log
                    user_name NVARCHAR(100),                                    -- Username of the user who triggered the log
                    user_role NVARCHAR(50),                                     -- User Role (e.g., Admin, Guest)
                    correlation_id NVARCHAR(50),                                -- Correlation ID for tracing requests
                    transaction_id NVARCHAR(50),                                -- Transaction ID for grouping logs
                    execution_time_ms FLOAT,                                    -- Execution Time in milliseconds
                    memory_usage_kb BIGINT,                                     -- Memory Usage in KB
                    cpu_usage_percentage INT,                                  -- CPU Usage percentage
                    authentication_type NVARCHAR(50),                           -- Authentication Type (e.g., OAuth, JWT)
                    is_authenticated BIT NOT NULL,                              -- Whether the user is authenticated (TRUE/FALSE)
                    client_user_agent NVARCHAR(255),                            -- Client User-Agent string (e.g., browser info)
                    log_category NVARCHAR(100),                                 -- Log Category (e.g., Database, UI, API)
                    tags NVARCHAR(300),                                         -- Tags for categorization (comma-separated)
                    custom_data NVARCHAR(MAX),                                  -- Custom Metadata in JSON or string format
                    log_partition_key NVARCHAR(50),                             -- Partition Key for optimization
                    log_retention_date DATETIME                                 -- Log Retention Date (when the log should be deleted)
                );";
                //using (SqlConnection connection = new SqlConnection(_connectionString))
                //{
                //    connection.Open();
                //    using (SqlCommand command = new SqlCommand(tableQuery, connection))
                //    {
                //        command.ExecuteNonQuery();
                //    }
                //}
                return true;
            }
        }
        [DebuggerHidden]
        [DebuggerStepThrough]
        private string CreateInsertQuery()
        {
            string query = $@"
            INSERT INTO {_userDefinedTableName} (
                log_level,log_severity,log_timestamp,log_message,exception_message,exception_type,exception_stack_trace,thread_id, 
                method_name,class_name,namespace_name,source_file_path,source_line_number,application_name,application_version,environment_name,host_name,operating_system, 
                framework_version,client_ip,server_ip,port,url,http_method,user_id,user_name,user_role,correlation_id,transaction_id, 
                execution_time_ms,memory_usage_kb,cpu_usage_percentage,authentication_type,is_authenticated,client_user_agent,log_category, 
                tags,custom_data,log_partition_key,log_retention_date) 
            VALUES (@log_level,@log_severity,@log_timestamp,@log_message,@exception_message,@exception_type,@exception_stack_trace,@thread_id,@method_name, 
                @class_name,@namespace_name,@source_file_path,@source_line_number,@application_name,@application_version,@environment_name,@host_name,@operating_system, 
                @framework_version,@client_ip,@server_ip,@port,@url, @http_method,@user_id,@user_name,@user_role, @correlation_id,@transaction_id,@execution_time_ms, @memory_usage_kb,@cpu_usage_percentage,@authentication_type,@is_authenticated,@client_user_agent,@log_category,@tags,@custom_data,@log_partition_key,@log_retention_date
            );
            ";
            return query;
        }
    }
}
