using Azure.Data.Tables;
using Azure.Data.Tables.Sas;

namespace ApiStorageTokens.Services
{
    public class ServiceSasToken
    {
        //esta clase va a generar tokens para la tabla alumnos
        private TableClient tablaAlumnos;

        public ServiceSasToken(IConfiguration configuration)
        {
            string azureKeys = 
                configuration.GetValue<string>
                ("AzureKeys:StorageAccount");

            TableServiceClient serviceClient =
                new TableServiceClient (azureKeys);

            this.tablaAlumnos = 
                serviceClient.GetTableClient("alumnos");
        }

        public string GenerateToken(string curso)
        {
            //necesitamos el tipo de permisos de acceso
            //por ahoa, solovamos a dar permisos de lectura
            TableSasPermissions permisos =
                TableSasPermissions.Read;

            //el acceso a los elementos con el token está delimitado
            //mediante un tiempo. Necesitamos un constructor de permisos
            //con un tiempo determinado de acceso
            TableSasBuilder builder =
                this.tablaAlumnos.GetSasBuilder(permisos,
                    DateTime.UtcNow.AddMinutes(30));

            //queremos limitar el acceso por curso
            builder.PartitionKeyStart = curso;
            builder.PartitionKeyEnd = curso;

            //con esto ya podemos generar el token de acceso
            //que será una uri con los permisos y el tiempo
            Uri uriToken =
                this.tablaAlumnos.GenerateSasUri(builder);

            //extraemos la ruta HTTPS del uri
            string token = uriToken.AbsoluteUri;

            return token;
        }
    }
}
