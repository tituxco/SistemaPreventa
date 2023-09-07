using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaPreventa
{    
    public class SyncService : BackgroundService
    {
        DatabaseConfigSqlite databaseConfigSqlite = new DatabaseConfigSqlite(DatosComunes.BaseDeDatosLocal);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Aquí puedes escribir tu lógica de sincronización en segundo plano
                await SyncData();

                // Espera un intervalo de tiempo antes de la próxima ejecución
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }

        private async Task SyncData()
        {

            // Lógica de sincronización de datos
            // Puedes realizar llamadas a API, actualizar bases de datos, etc.
        }
    }
}
