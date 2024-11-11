using System;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Npgsql;
using Serilog;
using System.Text.Json;
using Sample.Notify.WebApi.Core.Models;

namespace Sample.Notify.WebApi.Workers;

public class WorkerAuditPostgresql : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (true)
		{
			await NotifyAsyn();
		}
	}

	private async Task NotifyAsyn()
	{
		try
		{
			var connString = "Host=localhost;Username=postgres;Password=mysecretpassword;Database=mydb";

			await using var connection = new NpgsqlConnection(connString);

			connection.Notification += async (o, e) =>
			{
				try
				{
					var result = JsonSerializer.Deserialize<AuditPostgresqlModel>(e.Payload);

					Log.Information($"Notificação recebida: {result.Json}");
				}
				catch (Exception ex)
				{
					Log.Error(ex, nameof(connection.Notification) + " - " + e?.Payload);
				}
			};

			await using (var cmd = new NpgsqlCommand("LISTEN datachange;", connection))
			{
				await cmd.ExecuteNonQueryAsync();
			}

			while (true)
				await connection.WaitAsync();
		}
		catch (Exception ex)
		{
			if (ex is PostgresException { SqlState: "57P01" })
				NpgsqlConnection.ClearAllPools();

			await Task.Delay(TimeSpan.FromSeconds(2));
			Log.Error(ex, nameof(ExecuteAsync));
		}
	}

}
