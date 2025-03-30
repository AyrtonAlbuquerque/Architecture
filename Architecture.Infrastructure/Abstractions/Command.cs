using System.Data;
using System.Data.Common;
using Architecture.Domain.Abstractions;
using Architecture.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Architecture.Infrastructure.Abstractions
{
        public class Command : ICommand
    {
        private readonly DbCommand command;

        public Command(Context context, string commandText)
        {
            command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
        }

        public ICommand Parameter(string name, object? value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = name.StartsWith("@") ? name : "@" + name;
            parameter.Value = value;

            if (command.CommandType == CommandType.Text && command.CommandText.Contains(parameter.ParameterName))
                command.CommandText = command.CommandText.Replace(name, value?.ToString());

            command.Parameters.Add(parameter);

            return this;
        }

        public ICommand Text(string text)
        {
            command.CommandText = text;
            return this;
        }

        public ICommand Timeout(int timeout)
        {
            command.CommandTimeout = timeout;
            return this;
        }

        public ICommand Type(CommandType type)
        {
            command.CommandType = type;
            return this;
        }

        public int ExecuteNonQuery()
        {
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            var result = command.ExecuteNonQuery();

            command.Dispose();

            return result;
        }

        public async Task<int> ExecuteNonQueryAsync()
        {
            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteNonQueryAsync();

            await command.DisposeAsync();

            return result;
        }

        public DbDataReader ExecuteReader()
        {
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            var result = command.ExecuteReader();

            command.Dispose();

            return result;
        }

        public async Task<DbDataReader> ExecuteReaderAsync()
        {
            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteReaderAsync();

            await command.DisposeAsync();

            return result;
        }

        public object? ExecuteScalar()
        {
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            var result = command.ExecuteScalar();

            command.Dispose();

            return result;
        }

        public async Task<object?> ExecuteScalarAsync()
        {
            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();

            await command.DisposeAsync();

            return result;
        }
    }
}