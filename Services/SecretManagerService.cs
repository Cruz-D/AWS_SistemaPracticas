using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using MvcAppAws_Daniel_delaCruz.Application.DTO;
using Newtonsoft.Json;

namespace MvcAppAws_Daniel_delaCruz.Services
{
    public class SecretManagerService
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonSecretsManager _secretsManager;
        private readonly BuildConnectionDTO _buildConnectionDTO;

        public SecretManagerService(
            IConfiguration configuration)
        {
            _configuration = configuration;

            //Crear isntancia de SecretsManager
            _secretsManager = new AmazonSecretsManagerClient(

                // Obtener las credenciales de AWS desde la configuración
                _configuration["AWS:AccessKeyId"],
                _configuration["AWS:SecretAccessKey"],

                // Obtener la región de AWS desde la configuración
                Amazon.RegionEndpoint.GetBySystemName(_configuration["AWS:Region"])

            );
        }

        //Funcion para obtner el secreto y deseerializarlo
        public async Task<string> GetSecretValuesAsync(string secretName)
        {
            try
            {
                // Obtener el secreto de AWS Secrets Manager
                var request = new GetSecretValueRequest
                {
                    // asignar el nombre del secreto a un objeto de tipo GetSecretValueRequest
                    SecretId = secretName
                };

                //peticion al servicio de AWS
                var response = await _secretsManager.GetSecretValueAsync(request);

                // Verificar si el secreto fue encontrado
                if (response.SecretString != null)
                {
                    //verificar el formato en el que se encuentra el secreto, si es un string o binario
                    string secretString = response.SecretString ?? Convert.ToBase64String(response.SecretBinary.ToArray());

                    // Deserializar el secreto JSON a un objeto
                    var secretValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretString);

                    // Crear una instancia de BuildConnectionDTO y asignar los valores del secreto
                    var buildConnectionDTO = new BuildConnectionDTO
                    {
                        username = secretValue["username"],
                        password = secretValue["password"],
                        engine = secretValue["engine"],
                        host = secretValue["host"],
                        port = secretValue["port"],
                        dbname = secretValue["dbname"]
                    };

                    // construir la cadena de conexión
                    string connectionString = $"Server={buildConnectionDTO.host},{buildConnectionDTO.port};Database={buildConnectionDTO.dbname};User Id={buildConnectionDTO.username};Password={buildConnectionDTO.password};TrustServerCertificate=True;";

                    // Devolver la cadena de conexión
                    return connectionString;

                }
                else
                {
                    throw new Exception("No se encontró el secreto.");
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception($"Error al obtener el secreto: {ex.Message}");
            }

        }

    }
}
