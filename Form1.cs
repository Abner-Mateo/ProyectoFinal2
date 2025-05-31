using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


namespace ProyectoFinal2

{
    public partial class Form1 : Form
    {
        // API Key de OpenAI (reemplaza con tu clave válida)
        private readonly string apiKey = ConfigurationManager.AppSettings["apiKey"];
        private readonly string endpoint = "https://api.openai.com/v1/chat/completions";

        // Cadena de conexión a tu base de datos
        private readonly string connectionString = "Data Source=DESKTOP-P1UPCAU\\SQLEXPRESS;Initial Catalog=ProblemasSoluciones;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        public class ProblemaMedico
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public string PasosAccion { get; set; }
            public string Advertencias { get; set; }
            public string TiempoEstimado { get; set; }
        }

        public class CentroSalud
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Direccion { get; set; }
            public string Telefono { get; set; }
            public string Municipio { get; set; }
            public string Tipo { get; set; }
            public decimal Latitud { get; set; }
            public decimal Longitud { get; set; }
        }

        private async Task<string> ObtenerRecomendacionMedica(string problema)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return "⚠️ **API DE INTELIGENCIA ARTIFICIAL NO CONFIGURADA**\n\nPara usar consultas con IA, configure su API key de OpenAI en el archivo App.config.\nActualmente solo se pueden realizar consultas desde la base de datos local.";
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                    client.Timeout = TimeSpan.FromSeconds(30);

                    // NUEVO PROMPT con formato muy específico
                    var prompt = $@"
Eres un asistente médico virtual para Guatemala. El usuario reporta: '{problema}'

INSTRUCCIONES IMPORTANTES:
- Responde EXACTAMENTE con el formato que te muestro
- Usa saltos de línea dobles entre cada sección
- No agregues texto adicional fuera del formato
- Usa lenguaje sencillo para población guatemalteca

FORMATO REQUERIDO:

🔍 DIAGNÓSTICO SUGERIDO:
[Escribir 2-3 oraciones sobre qué podría ser, sin dar diagnóstico definitivo]


💊 RECOMENDACIONES GENERALES:
• [Primera recomendación específica]
• [Segunda recomendación específica] 
• [Tercera recomendación específica]
• [Cuarta recomendación específica]


🚨 BUSCAR ATENCIÓN MÉDICA INMEDIATA SI:
• [Primera señal de alarma]
• [Segunda señal de alarma]
• [Tercera señal de alarma]


⏰ TIEMPO ESTIMADO DE RECUPERACIÓN:
[Una oración sobre duración típica de los síntomas]


📍 CONSEJOS PARA EL CLIMA DE GUATEMALA:
[Consejo específico considerando calor, humedad, altitud de Guatemala]


⚠️ RECORDATORIO IMPORTANTE:
Esta información es educativa. Consulte siempre a un médico profesional para diagnóstico y tratamiento precisos.";

                    var requestBody = new
                    {
                        model = "gpt-4o-mini",
                        messages = new[]
                        {
                    new { role = "system", content = "Eres un asistente médico que SIEMPRE respeta el formato exacto solicitado. Usas saltos de línea dobles entre secciones. No agregas texto extra." },
                    new { role = "user", content = prompt }
                },
                        max_tokens = 900,
                        temperature = 0.5 // Menos creatividad para más consistencia en formato
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(endpoint, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        return FormatearErrorResponse($"❌ Error en la API: {response.StatusCode}\n\nDetalles: {errorContent}");
                    }

                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    if (json?.choices != null && json.choices.Count > 0)
                    {
                        string respuestaIA = json.choices[0].message.content.ToString();

                        // IMPORTANTE: Procesar la respuesta para asegurar formato correcto
                        return FormatearRespuestaIA(respuestaIA);
                    }
                    else
                    {
                        return FormatearErrorResponse("❌ Error: No se recibió una respuesta válida de la API de IA.");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return FormatearErrorResponse($"🌐 Error de conexión: {ex.Message}\n\nVerifique su conexión a internet.");
            }
            catch (TaskCanceledException)
            {
                return FormatearErrorResponse("⏱️ Timeout: La solicitud tardó demasiado tiempo. Verifique su conexión a internet.");
            }
            catch (Exception ex)
            {
                return FormatearErrorResponse($"❌ Error inesperado en IA: {ex.Message}");
            }
        }


        


private string FormatearRespuestaIA(string respuesta)
    {
        var sb = new StringBuilder();

        sb.AppendLine("🤖 CONSULTA DE INTELIGENCIA ARTIFICIAL");
        sb.AppendLine("".PadRight(50, '='));
        sb.AppendLine();

        // Limpiar y formatear la respuesta
        string respuestaLimpia = respuesta.Trim();

        // Insertar doble salto de línea antes de cada sección reconocida
        string[] secciones = new[]
        {
        "DIAGNÓSTICO SUGERIDO:",
        "RECOMENDACIONES GENERALES:",
        "BUSCAR ATENCIÓN MÉDICA INMEDIATA SI:",
        "TIEMPO ESTIMADO DE RECUPERACIÓN:",
        "CONSEJOS PARA EL CLIMA DE GUATEMALA:",
        "RECORDATORIO IMPORTANTE:"
    };

        foreach (var seccion in secciones)
        {
            // Agrega doble salto de línea antes de cada encabezado (excepto si está al inicio)
            respuestaLimpia = Regex.Replace(
                respuestaLimpia,
                $@"(?!^)\s*([🔍💊🚨⏰📍⚠️]*\s*{Regex.Escape(seccion)})",
                "\n\n$1",
                RegexOptions.IgnoreCase);
        }

        sb.AppendLine(respuestaLimpia);
        sb.AppendLine();
        sb.AppendLine("".PadRight(50, '='));

        return sb.ToString();
    }

    private string FormatearErrorResponse(string mensaje)
        {
            var sb = new StringBuilder();

            sb.AppendLine("❌ ERROR EN CONSULTA");
            sb.AppendLine("".PadRight(30, '='));
            sb.AppendLine();
            sb.AppendLine(mensaje);
            sb.AppendLine();
            sb.AppendLine("".PadRight(30, '='));

            return sb.ToString();
        }




        private async Task<string> ObtenerRecomendacionMedicaDesdeBaseDeDatos(string problema)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Usar el procedimiento almacenado para buscar
                    using (SqlCommand command = new SqlCommand("sp_BuscarProblemaMedico", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Sintoma", problema);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return $@"📋 **INFORMACIÓN DE BASE DE DATOS LOCAL**

🏥 **Problema Médico:** {reader["Nombre"]}
📂 **Categoría:** {reader["Categoria"]}

💊 **PASOS DE ACCIÓN RECOMENDADOS:**
{reader["PasosAccion"]}

⚠️ **ADVERTENCIAS IMPORTANTES:**
{reader["Advertencias"]}

⏰ **TIEMPO ESTIMADO DE RECUPERACIÓN:**
{reader["TiempoEstimado"]}

📝 **NOTA IMPORTANTE:** Esta información es de carácter general y educativo. Para un diagnóstico preciso y tratamiento adecuado, consulte siempre a un profesional médico calificado.";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return $"❌ **Error de base de datos:** {ex.Message}\n\nVerifique que la base de datos esté configurada correctamente.";
            }
            catch (Exception ex)
            {
                return $"❌ **Error inesperado en BD:** {ex.Message}";
            }

            return "🔍 **No se encontró información específica en la base de datos local.**\n\nSe consultará la inteligencia artificial si está disponible.";
        }

        private async Task<string> ObtenerCentroDeSalud(double latitud, double longitud)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("sp_BuscarCentroSaludCercano", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Latitud", latitud);
                        command.Parameters.AddWithValue("@Longitud", longitud);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return $@"🏥 **{reader["Nombre"]}**

📍 **Dirección:** {reader["Direccion"]}
📞 **Teléfono:** {reader["Telefono"]}
🏘️ **Municipio:** {reader["Municipio"]}
🏢 **Tipo:** {reader["Tipo"]}

💡 **Recomendación:** Llame antes de acudir para confirmar horarios de atención y disponibilidad de servicios.";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return $"Error de base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.Message}";
            }

            return @"🏥 **Hospital Nacional de Jutiapa** (Por defecto)
📍 **Dirección:** 3a Calle 4-45, Zona 1, Jutiapa
📞 **Teléfono:** 7844-4567";
        }

        private async Task<bool> VerificarConexionBaseDeDatos()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                // Mostrar indicador de carga
                txtResultado.Text = "🔍 Buscando información médica, por favor espere...";
                btnBuscar.Enabled = false;
                Application.DoEvents(); // Actualizar la interfaz

                string problema = txtProblema.Text?.Trim();

                if (string.IsNullOrWhiteSpace(problema))
                {
                    MessageBox.Show("Por favor, ingrese un síntoma o problema médico para buscar.",
                                    "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtResultado.Text = "";
                    return;
                }

                // Verificar conexión a base de datos
                bool dbDisponible = await VerificarConexionBaseDeDatos();
                if (!dbDisponible)
                {
                    txtResultado.Text = "❌ **Error:** No se puede conectar a la base de datos.\n\nVerifique que SQL Server esté ejecutándose y la cadena de conexión sea correcta.";
                    return;
                }

                // Buscar en base de datos local primero
                txtResultado.Text = "📋 Consultando base de datos local...";
                Application.DoEvents();

                string resultadoBD = await ObtenerRecomendacionMedicaDesdeBaseDeDatos(problema);

                if (resultadoBD.Contains("No se encontró información específica"))
                {
                    // Si no se encuentra en BD local, usar IA
                    txtResultado.Text = "🤖 Consultando inteligencia artificial...";
                    Application.DoEvents();

                    string resultadoAI = await ObtenerRecomendacionMedica(problema);
                    txtResultado.Text = resultadoAI;
                }
                else
                {
                    txtResultado.Text = resultadoBD;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtResultado.Text = "❌ Error al procesar la consulta.";
            }
            finally
            {
                btnBuscar.Enabled = true;
            }
        }

        private async void btnCentroSalud_Click(object sender, EventArgs e)
        {
            try
            {
                btnCentroSalud.Enabled = false;

                // Verificar conexión a base de datos
                bool dbDisponible = await VerificarConexionBaseDeDatos();
                if (!dbDisponible)
                {
                    MessageBox.Show("❌ Error: No se puede conectar a la base de datos.\n\nVerifique que SQL Server esté ejecutándose.",
                                    "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Coordenadas de ejemplo (Jutiapa Centro)
                // En una versión avanzada, podrías obtener la ubicación real del usuario
                double lat = 14.2911;
                double lon = -89.8957;

                string centro = await ObtenerCentroDeSalud(lat, lon);
                MessageBox.Show(centro, "Centro de Salud Recomendado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar centro de salud: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCentroSalud.Enabled = true;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Configuración inicial del formulario
            this.Text = "🏥 Asistente Médico Virtual - Jutiapa, Guatemala";

            // CONFIGURACIÓN MEJORADA del TextBox de resultados
            if (txtResultado != null)
            {
                txtResultado.Multiline = true;
                txtResultado.ScrollBars = ScrollBars.Vertical;
                txtResultado.ReadOnly = true;
                txtResultado.Font = new Font("Consolas", 9F); // Fuente monospace para mejor alineación
                txtResultado.WordWrap = true; // Permitir ajuste de líneas
                txtResultado.BackColor = Color.White;
                txtResultado.ForeColor = Color.Black;
               
                // IMPORTANTE: Agregar padding interno
                txtResultado.Padding = new Padding(5);
            }

            if (txtProblema != null)
            {
                txtProblema.Font = new Font("Segoe UI", 11F);
                txtProblema.Text = " Dolor de Cabeza";
                txtProblema.ForeColor = Color.Gray;
            }


            // Verificar conexión a base de datos al cargar
            bool dbConectada = await VerificarConexionBaseDeDatos();
            if (dbConectada)
            {
                txtResultado.Text = "✅ Sistema listo. Base de datos conectada correctamente.\n\n💡 Ingrese un síntoma para obtener recomendaciones médicas.";
            }
            else
            {
                txtResultado.Text = "⚠️ Advertencia: No se puede conectar a la base de datos.\n\nVerifique que SQL Server esté ejecutándose y ejecute el script de configuración de base de datos.";
            }
        }

        // Método para probar la base de datos (opcional - para debugging)
        private async void btnProbarBD_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM ProblemasMedicos", connection))
                    {
                        int count = (int)await command.ExecuteScalarAsync();
                        MessageBox.Show($"✅ Conexión exitosa!\n\nProblemas médicos en BD: {count}",
                                        "Prueba de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error de conexión: {ex.Message}",
                                "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProblema_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void lblTermino_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
