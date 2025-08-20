// Archivo: WebsiteClient.cs

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Lnc.InmoStore.Clients
{
    /// <summary>
    /// Este cliente implementa las funcionalidades del API de Portal Services
    /// para web, reemplazando las llamadas del plugin de Nuxt.js.
    /// </summary>
    public class WebsiteClient
    {
        // Se utiliza un HttpClient estático para optimizar el uso de recursos.
        private static readonly HttpClient _client = new HttpClient();
        private readonly string _apiKey;
        private string _apiVersion;

        // La URL raíz del API.
        private const string ApiRoot = "https://inmostore-api.psweb.me/";

        /// <summary>
        /// Constructor de la clase WebsiteClient.
        /// </summary>
        /// <param name="apiId">El ID del API (no se utiliza en este cliente).</param>
        /// <param name="apiKey">La clave de acceso para el API.</param>
        public WebsiteClient(string apiId, string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey), "La clave del API no puede ser nula o vacía.");
            }
            _apiKey = apiKey;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Modelos de Datos (DTOs)
        
        // Aquí se definen las clases que representan las estructuras de datos del API.
        // Se utilizan atributos como [DataMember] para asegurar la correcta serialización.

        [DataContract]
        public class Operacion
        {
            [DataMember(Name = "codigo")]
            public int Codigo { get; set; }
            [DataMember(Name = "newItemId")]
            public Guid? NewItemId { get; set; }
            [DataMember(Name = "affectedRows")]
            public int AffectedRows { get; set; }
            [DataMember(Name = "mensaje")]
            public string Mensaje { get; set; }
            [DataMember(Name = "detalle")]
            public string Detalle { get; set; }
        }

        [DataContract]
        public class Elemento
        {
            [DataMember(Name = "elementoId")]
            public int ElementoId { get; set; }
            [DataMember(Name = "padre")]
            public int Padre { get; set; }
            [DataMember(Name = "nombre")]
            public string Nombre { get; set; }
            [DataMember(Name = "tooltiop")] // Manteniendo el typo original del JSON
            public string Tooltiop { get; set; }
            [DataMember(Name = "valor")]
            public string Valor { get; set; }
            [DataMember(Name = "defaultValue")]
            public string DefaultValue { get; set; }
            [DataMember(Name = "status")]
            public int Status { get; set; }
        }

        [DataContract]
        public class Escena
        {
            [DataMember(Name = "escenaId")]
            public Guid EscenaId { get; set; }
            [DataMember(Name = "empresaId")]
            public Guid EmpresaId { get; set; }
            [DataMember(Name = "modo")]
            public int Modo { get; set; }
            [DataMember(Name = "objecto")]
            public Guid Objecto { get; set; }
            [DataMember(Name = "titulo")]
            public string Titulo { get; set; }
            [DataMember(Name = "principal")]
            public bool Principal { get; set; }
            [DataMember(Name = "imageUrl")]
            public string ImageUrl { get; set; }
            [DataMember(Name = "status")]
            public int Status { get; set; }
        }
        
        [DataContract]
        public class EscenaPin
        {
            [DataMember(Name = "pinId")]
            public Guid PinId { get; set; }
            [DataMember(Name = "empresaId")]
            public Guid EmpresaId { get; set; }
            [DataMember(Name = "escenaId")]
            public Guid EscenaId { get; set; }
            [DataMember(Name = "etiqueta")]
            public string Etiqueta { get; set; }
            [DataMember(Name = "pitch")]
            public string Pitch { get; set; }
            [DataMember(Name = "yaw")]
            public string Yaw { get; set; }
            [DataMember(Name = "target")]
            public Guid Target { get; set; }
            [DataMember(Name = "status")]
            public int Status { get; set; }
        }

        [DataContract]
        public class Imagen
        {
            [DataMember(Name = "imageId")]
            public Guid ImageId { get; set; }
            [DataMember(Name = "empresaId")]
            public Guid EmpresaId { get; set; }
            [DataMember(Name = "modo")]
            public int Modo { get; set; }
            [DataMember(Name = "objectId")]
            public Guid ObjectId { get; set; }
            [DataMember(Name = "titulo")]
            public string Titulo { get; set; }
            [DataMember(Name = "imageCaption")]
            public string ImageCaption { get; set; }
            [DataMember(Name = "imageUrl")]
            public string ImageUrl { get; set; }
            [DataMember(Name = "status")]
            public int Status { get; set; }
        }

        [DataContract]
        public class Inmueble
        {
            [DataMember(Name = "inmuebleId")]
            public Guid InmuebleId { get; set; }
            [DataMember(Name = "empresaId")]
            public Guid EmpresaId { get; set; }
            [DataMember(Name = "tipoInmueble")]
            public int TipoInmueble { get; set; }
            [DataMember(Name = "tipoOperacion")]
            public int TipoOperacion { get; set; }
            [DataMember(Name = "codigo")]
            public string Codigo { get; set; }
            [DataMember(Name = "titulo")]
            public string Titulo { get; set; }
            [DataMember(Name = "encabezado")]
            public string Encabezado { get; set; }
            [DataMember(Name = "contenido")]
            public string Contenido { get; set; }
            [DataMember(Name = "habitaciones")]
            public int Habitaciones { get; set; }
            [DataMember(Name = "banios")]
            public int Banios { get; set; }
            [DataMember(Name = "terreno")]
            public double Terreno { get; set; }
            [DataMember(Name = "construccion")]
            public double Construccion { get; set; }
            [DataMember(Name = "precioMoneda")]
            public int PrecioMoneda { get; set; }
            [DataMember(Name = "precio")]
            public double Precio { get; set; }
            [DataMember(Name = "precioTipo")]
            public int PrecioTipo { get; set; }
            [DataMember(Name = "pais")]
            public int Pais { get; set; }
            [DataMember(Name = "estado")]
            public int Estado { get; set; }
            [DataMember(Name = "municipio")]
            public int Municipio { get; set; }
            [DataMember(Name = "ciudad")]
            public int Ciudad { get; set; }
            [DataMember(Name = "direccion")]
            public string Direccion { get; set; }
            [DataMember(Name = "referencia")]
            public string Referencia { get; set; }
            [DataMember(Name = "googleMapsScript")]
            public string GoogleMapsScript { get; set; }
            [DataMember(Name = "showDireccionDb")]
            public int ShowDireccionDb { get; set; }
            [DataMember(Name = "showMapaDb")]
            public int ShowMapaDb { get; set; }
            [DataMember(Name = "showDireccion")]
            public bool ShowDireccion { get; set; }
            [DataMember(Name = "showMapa")]
            public bool ShowMapa { get; set; }
            [DataMember(Name = "status")]
            public int Status { get; set; }
            [DataMember(Name = "statusDescr")]
            public string StatusDescr { get; set; }
        }

        [DataContract]
        public class ComboItem
        {
            [DataMember(Name = "texto")]
            public string Texto { get; set; }
            [DataMember(Name = "valor")]
            public string Valor { get; set; }
        }

        [DataContract]
        public class CaracteristicasInmueble
        {
            [DataMember(Name = "caracteristicaId")]
            public Guid CaracteristicaId { get; set; }
            [DataMember(Name = "empresaId")]
            public Guid EmpresaId { get; set; }
            [DataMember(Name = "inmuebleId")]
            public Guid InmuebleId { get; set; }
            [DataMember(Name = "nuevaConstruccion")]
            public bool NuevaConstruccion { get; set; }
            [DataMember(Name = "portada")]
            public bool Portada { get; set; }
            [DataMember(Name = "amoblado")]
            public bool Amoblado { get; set; }
            [DataMember(Name = "cocinaEmpotrada")]
            public bool CocinaEmpotrada { get; set; }
            [DataMember(Name = "aireAcondicionado")]
            public bool AireAcondicionado { get; set; }
            [DataMember(Name = "piscina")]
            public bool Piscina { get; set; }
            [DataMember(Name = "areaSocial")]
            public bool AreaSocial { get; set; }
            [DataMember(Name = "areaDeportiva")]
            public bool AreaDeportiva { get; set; }
            [DataMember(Name = "vigilancia")]
            public bool Vigilancia { get; set; }
            [DataMember(Name = "ascensor")]
            public bool Ascensor { get; set; }
            [DataMember(Name = "camaras")]
            public bool Camaras { get; set; }
            [DataMember(Name = "telefono")]
            public bool Telefono { get; set; }
            [DataMember(Name = "internet")]
            public bool Internet { get; set; }
            [DataMember(Name = "cercoElectrico")]
            public bool CercoElectrico { get; set; }
            [DataMember(Name = "portonElectrico")]
            public bool PortonElectrico { get; set; }
            [DataMember(Name = "asegurado")]
            public bool Asegurado { get; set; }
            [DataMember(Name = "financiamiento")]
            public bool Financiamiento { get; set; }
            [DataMember(Name = "comercial")]
            public bool Comercial { get; set; }
            [DataMember(Name = "residencial")]
            public bool Residencial { get; set; }
            [DataMember(Name = "patioDelantero")]
            public bool PatioDelantero { get; set; }
            [DataMember(Name = "patioTrasero")]
            public bool PatioTrasero { get; set; }
            [DataMember(Name = "calentador")]
            public bool Calentador { get; set; }
            [DataMember(Name = "balcon")]
            public bool Balcon { get; set; }
            [DataMember(Name = "sotano")]
            public bool Sotano { get; set; }
            [DataMember(Name = "armarios")]
            public bool Armarios { get; set; }
            [DataMember(Name = "puestosEstacionamiento")]
            public int PuestosEstacionamiento { get; set; }
            [DataMember(Name = "antiguedad")]
            public int Antiguedad { get; set; }
            [DataMember(Name = "pisos")]
            public int Pisos { get; set; }
            [DataMember(Name = "tipoPiso")]
            public int TipoPiso { get; set; }
            [DataMember(Name = "tipoTecho")]
            public int TipoTecho { get; set; }
            [DataMember(Name = "estadoActual")]
            public int EstadoActual { get; set; }
            [DataMember(Name = "status")]
            public int Status { get; set; }
        }

        [DataContract]
        public class PropiedadInmueble
        {
            [DataMember(Name = "propertyId")]
            public Guid PropertyId { get; set; }
            [DataMember(Name = "empresaId")]
            public Guid EmpresaId { get; set; }
            [DataMember(Name = "inmuebleId")]
            public Guid InmuebleId { get; set; }
            [DataMember(Name = "tipoPropiedad")]
            public int TipoPropiedad { get; set; }
            [DataMember(Name = "coleccionElementoId")]
            public int ColeccionElementoId { get; set; }
            [DataMember(Name = "property")]
            public string Property { get; set; }
            [DataMember(Name = "status")]
            public int Status { get; set; }
            [DataMember(Name = "creado")]
            public Guid Creado { get; set; }
            [DataMember(Name = "creadoFecha")]
            public DateTime CreadoFecha { get; set; }
            [DataMember(Name = "modificado")]
            public Guid Modificado { get; set; }
            [DataMember(Name = "modificadoFecha")]
            public DateTime ModificadoFecha { get; set; }
        }

        [DataContract]
        public class Proyecto
        {
            [DataMember(Name = "proyectoId")]
            public Guid ProyectoId { get; set; }
            [DataMember(Name = "empresaId")]
            public Guid EmpresaId { get; set; }
            [DataMember(Name = "nombreProyecto")]
            public string NombreProyecto { get; set; }
            [DataMember(Name = "pais")]
            public int Pais { get; set; }
            [DataMember(Name = "estado")]
            public int Estado { get; set; }
            [DataMember(Name = "municipio")]
            public int Municipio { get; set; }
            [DataMember(Name = "ciudad")]
            public int Ciudad { get; set; }
            [DataMember(Name = "direccion")]
            public string Direccion { get; set; }
            [DataMember(Name = "referencia")]
            public string Referencia { get; set; }
            [DataMember(Name = "googleMapsScript")]
            public string GoogleMapsScript { get; set; }
            [DataMember(Name = "subdominio")]
            public string Subdominio { get; set; }
            [DataMember(Name = "websiteOnline")]
            public bool WebsiteOnline { get; set; }
            [DataMember(Name = "status")]
            public int Status { get; set; }
            [DataMember(Name = "creado")]
            public Guid Creado { get; set; }
            [DataMember(Name = "creadoFecha")]
            public DateTime CreadoFecha { get; set; }
            [DataMember(Name = "modificado")]
            public Guid Modificado { get; set; }
            [DataMember(Name = "modificadoFecha")]
            public DateTime ModificadoFecha { get; set; }
        }

        [DataContract]
        public class Modelo
        {
            [DataMember(Name = "modeloId")]
            public Guid ModeloId { get; set; }
            [DataMember(Name = "empresaId")]
            public Guid EmpresaId { get; set; }
            [DataMember(Name = "proyectoId")]
            public Guid ProyectoId { get; set; }
            [DataMember(Name = "nombre")]
            public string Nombre { get; set; }
            [DataMember(Name = "descripcion")]
            public string Descripcion { get; set; }
            [DataMember(Name = "nivel1")]
            public string Nivel1 { get; set; }
            [DataMember(Name = "nivel2")]
            public string Nivel2 { get; set; }
            [DataMember(Name = "nivel3")]
            public string Nivel3 { get; set; }
            [DataMember(Name = "nivel4")]
            public string Nivel4 { get; set; }
            [DataMember(Name = "nivel5")]
            public string Nivel5 { get; set; }
            [DataMember(Name = "status")]
            public int Status { get; set; }
        }
        
        // Clases de resultado que envuelven los datos y la operación.

        [DataContract]
        public class GetElementoResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "resultado")]
            public Elemento Resultado { get; set; }
        }
        
        [DataContract]
        public class GetEmpresaResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "resultado")]
            public object Resultado { get; set; }
        }

        [DataContract]
        public class ListEscenasResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "items")]
            public List<Escena> Items { get; set; }
        }
        
        [DataContract]
        public class ListEscenasPinsResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "items")]
            public List<EscenaPin> Items { get; set; }
        }

        [DataContract]
        public class ListImagenesResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "items")]
            public List<Imagen> Items { get; set; }
        }
        
        [DataContract]
        public class GetImagenPortadaResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "resultado")]
            public Imagen Resultado { get; set; }
        }

        [DataContract]
        public class GetInmuebleResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "resultado")]
            public Inmueble Resultado { get; set; }
        }

        [DataContract]
        public class ListInmueblesResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "items")]
            public List<Inmueble> Items { get; set; }
        }

        [DataContract]
        public class GetComboResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "items")]
            public List<ComboItem> Items { get; set; }
        }

        [DataContract]
        public class GetCaracteristicasInmuebleResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "resultado")]
            public CaracteristicasInmueble Resultado { get; set; }
        }

        [DataContract]
        public class ListPropiedadesInmuebleResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "items")]
            public List<PropiedadInmueble> Items { get; set; }
        }

        [DataContract]
        public class GetProyectoResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "resultado")]
            public Proyecto Resultado { get; set; }
        }

        [DataContract]
        public class ListProyectosResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "items")]
            public List<Proyecto> Items { get; set; }
        }

        [DataContract]
        public class GetProyectoModeloResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "resultado")]
            public Modelo Resultado { get; set; }
        }

        [DataContract]
        public class ListProyectoModelosResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "items")]
            public List<Modelo> Items { get; set; }
        }

        [DataContract]
        public class GetSettingResult
        {
            [DataMember(Name = "Operacion")]
            public Operacion Operacion { get; set; }
            [DataMember(Name = "resultado")]
            public string Resultado { get; set; }
        }
        
        #endregion

        /// <summary>
        /// Inicializa la conexión con el servidor y valida la versión.
        /// </summary>
        /// <returns>Tarea asíncrona.</returns>
        public async Task InitAsync()
        {
            try
            {
                var response = await _client.GetStringAsync($"{ApiRoot}sys/version/{_apiKey}");
                // Suponiendo que la respuesta es un JSON con una propiedad "Resultado"
                // Aquí, la respuesta se parsea de forma simple.
                // Para una deserialización robusta, se podría usar una biblioteca como Newtonsoft.Json.
                if (response.Contains("Resultado"))
                {
                    _apiVersion = response.Split(new[] { "Resultado\":\"" }, StringSplitOptions.None)[1].TrimEnd('"', '}');
                }
                Console.WriteLine("Conectado. Versión " + _apiVersion);
            }
            catch (HttpRequestException ex)
            {
                // Se lanza una excepción más descriptiva en C# en lugar de redirigir.
                throw new InvalidOperationException("Error al conectar con el servidor de la API. Verifique la clave o la conexión.", ex);
            }
        }

        /// <summary>
        /// Obtiene la versión actual del API.
        /// </summary>
        /// <returns>La versión del API.</returns>
        public string GetCurrentVersion()
        {
            return _apiVersion;
        }
        
        /// <summary>
        /// Obtiene el detalle de un elemento, consultado por su ID.
        /// </summary>
        /// <param name="id">ID del elemento a consultar.</param>
        /// <returns>Detalle del elemento consultado.</returns>
        public async Task<GetElementoResult> GetElementoAsync(int id)
        {
            var response = await _client.GetAsync($"{ApiRoot}elementos/get/{id}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetElementoResult>();
        }

        /// <summary>
        /// Obtiene el detalle de la empresa.
        /// </summary>
        /// <param name="id">ID de la empresa a consultar.</param>
        /// <returns>Detalle de la empresa consultada.</returns>
        public async Task<GetEmpresaResult> GetEmpresaAsync(Guid id)
        {
            var response = await _client.GetAsync($"{ApiRoot}empresas/get/{id}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetEmpresaResult>();
        }

        /// <summary>
        /// Obtiene la lista de escenas activas de un objeto.
        /// </summary>
        /// <param name="empresaId">ID de la empresa a consultar.</param>
        /// <param name="modo">Modo de la escena (0: Inmueble, 1: Proyecto).</param>
        /// <param name="objectId">ID del objeto asociado a la escena.</param>
        /// <returns>Lista de escenas activas del objeto.</returns>
        public async Task<ListEscenasResult> ListEscenasAsync(Guid empresaId, int modo, Guid objectId)
        {
            var response = await _client.GetAsync($"{ApiRoot}escenas/active/{empresaId}/{modo}/{objectId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ListEscenasResult>();
        }

        /// <summary>
        /// Obtiene la lista de pins de la escena actual.
        /// </summary>
        /// <param name="escenaId">ID de la escena a consultar.</param>
        /// <returns>Lista de pins de la escena consultada.</returns>
        public async Task<ListEscenasPinsResult> ListEscenaPinsAsync(Guid escenaId)
        {
            var response = await _client.GetAsync($"{ApiRoot}pins/list/{escenaId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ListEscenasPinsResult>();
        }

        /// <summary>
        /// Obtiene la URL de una imagen para embeber, basado en su id.
        /// </summary>
        /// <param name="imageId">ID de la imagen a consultar.</param>
        /// <returns>URL de la imagen a embeber.</returns>
        public string GetEmbededImageUrl(Guid imageId)
        {
            return $"{ApiRoot}images/embed/{imageId}/{_apiKey}";
        }

        /// <summary>
        /// Obtiene el listado de imágenes de un objeto.
        /// </summary>
        /// <param name="modo">Modo de la imagen (0: Inmueble, 1: Proyecto).</param>
        /// <param name="objectId">ID del objeto asociado a la imagen.</param>
        /// <returns>Lista de imágenes del objeto consultado.</returns>
        public async Task<ListImagenesResult> ListImagenesAsync(int modo, Guid objectId)
        {
            var response = await _client.GetAsync($"{ApiRoot}imagenes/list/{modo}/{objectId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ListImagenesResult>();
        }

        /// <summary>
        /// Obtiene la imagen de portada de un inmueble.
        /// </summary>
        /// <param name="inmuebleId">ID del inmueble a consultar.</param>
        /// <returns>Imagen de portada del inmueble consultado.</returns>
        public async Task<GetImagenPortadaResult> GetImagenPortadaAsync(Guid inmuebleId)
        {
            var response = await _client.GetAsync($"{ApiRoot}imagenes/portada/{inmuebleId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetImagenPortadaResult>();
        }

        /// <summary>
        /// Obtiene el detalle de un inmueble por su ID.
        /// </summary>
        /// <param name="inmuebleId">ID del inmueble a consultar.</param>
        /// <returns>Detalle del inmueble consultado.</returns>
        public async Task<GetInmuebleResult> GetInmuebleAsync(Guid inmuebleId)
        {
            var response = await _client.GetAsync($"{ApiRoot}inmuebles/get/{inmuebleId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetInmuebleResult>();
        }

        /// <summary>
        /// Obtiene el listado de inmuebles filtrados por empresa, tipo de inmueble, tipo de operación y ciudad.
        /// </summary>
        /// <param name="empresaId">ID de la empresa a consultar.</param>
        /// <param name="tipoInmueble">Código del tipo de inmueble (ej. casa, apartamento).</param>
        /// <param name="tipoOperacion">Código del tipo de operación (ej. venta, renta).</param>
        /// <param name="ciudad">Código de la ciudad a consultar.</param>
        /// <returns>Lista de inmuebles filtrados.</returns>
        public async Task<ListInmueblesResult> ListInmueblesByAsync(Guid empresaId, int tipoInmueble, int tipoOperacion, int ciudad)
        {
            var response = await _client.GetAsync($"{ApiRoot}inmuebles/listweb/{empresaId}/{tipoInmueble}/{tipoOperacion}/{ciudad}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ListInmueblesResult>();
        }

        /// <summary>
        /// Obtiene el listado de tipos de inmueble disponibles para una empresa.
        /// </summary>
        /// <param name="empresaId">ID de la empresa a consultar.</param>
        /// <returns>Lista de tipos de inmueble disponibles.</returns>
        public async Task<GetComboResult> ListTiposInmuebleAsync(Guid empresaId)
        {
            var response = await _client.GetAsync($"{ApiRoot}inmuebles/tiposInmueble/{empresaId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetComboResult>();
        }

        /// <summary>
        /// Obtiene el listado de tipos de operación disponibles para una empresa.
        /// </summary>
        /// <param name="empresaId">ID de la empresa a consultar.</param>
        /// <returns>Lista de tipos de operación disponibles.</returns>
        public async Task<GetComboResult> ListTiposOperacionAsync(Guid empresaId)
        {
            var response = await _client.GetAsync($"{ApiRoot}inmuebles/tiposOperacion/{empresaId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetComboResult>();
        }

        /// <summary>
        /// Obtiene el listado de ciudades disponibles para una empresa.
        /// </summary>
        /// <param name="empresaId">ID de la empresa a consultar.</param>
        /// <returns>Lista de ciudades disponibles.</returns>
        public async Task<GetComboResult> ListCiudadesAsync(Guid empresaId)
        {
            var response = await _client.GetAsync($"{ApiRoot}inmuebles/ciudades/{empresaId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetComboResult>();
        }

        /// <summary>
        /// Obtiene el listado de inmuebles destacados para la portada de una empresa.
        /// </summary>
        /// <param name="empresaId">ID de la empresa a consultar.</param>
        /// <returns>Lista de inmuebles destacados para la portada.</returns>
        public async Task<ListInmueblesResult> ListInmueblesPortadaAsync(Guid empresaId)
        {
            var response = await _client.GetAsync($"{ApiRoot}inmuebles/portada/{empresaId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ListInmueblesResult>();
        }

        /// <summary>
        /// Obtiene el listado de inmuebles recientes para una empresa.
        /// </summary>
        /// <param name="empresaId">ID de la empresa a consultar.</param>
        /// <returns>Lista de inmuebles recientes.</returns>
        public async Task<ListInmueblesResult> ListInmueblesRecientesAsync(Guid empresaId)
        {
            var response = await _client.GetAsync($"{ApiRoot}inmuebles/recientes/{empresaId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ListInmueblesResult>();
        }

        /// <summary>
        /// Obtiene el listado de características de un inmueble.
        /// </summary>
        /// <param name="inmuebleId">ID del inmueble a consultar.</param>
        /// <returns>Detalle de las características del inmueble consultado.</returns>
        public async Task<GetCaracteristicasInmuebleResult> GetInmuebleCaracteristicasAsync(Guid inmuebleId)
        {
            var response = await _client.GetAsync($"{ApiRoot}caracteristicas/get/{inmuebleId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetCaracteristicasInmuebleResult>();
        }

        /// <summary>
        /// Obtiene el listado de propiedades de un inmueble.
        /// </summary>
        /// <param name="inmuebleId">ID del inmueble a consultar.</param>
        /// <param name="tipoPropiedad">Código del tipo de propiedad a consultar (0, 1).</param>
        /// <returns>Lista de propiedades del inmueble consultado.</returns>
        public async Task<ListPropiedadesInmuebleResult> ListPropiedadesInmuebleAsync(Guid inmuebleId, int tipoPropiedad)
        {
            var response = await _client.GetAsync($"{ApiRoot}properties/list/{inmuebleId}/{tipoPropiedad}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ListPropiedadesInmuebleResult>();
        }

        /// <summary>
        /// Obtiene la información de un proyecto inmobiliario por su ID.
        /// </summary>
        /// <param name="proyectoId">ID del proyecto a consultar.</param>
        /// <returns>Detalle del proyecto consultado.</returns>
        public async Task<GetProyectoResult> GetProyectoAsync(Guid proyectoId)
        {
            var response = await _client.GetAsync($"{ApiRoot}proyectos/get/{proyectoId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetProyectoResult>();
        }

        /// <summary>
        /// Obtiene el listado de proyectos de una empresa.
        /// </summary>
        /// <param name="empresaId">ID de la empresa a consultar.</param>
        /// <returns>Lista de proyectos de la empresa consultada.</returns>
        public async Task<ListProyectosResult> ListProyectosAsync(Guid empresaId)
        {
            var response = await _client.GetAsync($"{ApiRoot}proyectos/active/{empresaId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ListProyectosResult>();
        }

        /// <summary>
        /// Obtiene el ID del proyecto basado en su subdominio.
        /// </summary>
        /// <param name="subdomain">Subdominio del proyecto a consultar.</param>
        /// <returns>ID del proyecto asociado al subdominio.</returns>
        public async Task<Guid> GetProyectoIdBySubdomainAsync(string subdomain)
        {
            var response = await _client.GetAsync($"{ApiRoot}proyectos/pidsubdir/{subdomain}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            // La respuesta es un GUID en un JSON, hay que parsearlo.
            // Para una deserialización robusta, se podría usar una biblioteca como Newtonsoft.Json.
            var content = await response.Content.ReadAsStringAsync();
            var guidString = content.Trim().Trim('"');
            return new Guid(guidString);
        }

        /// <summary>
        /// Obtiene el detalle de un modelo de inmueble dentro de un proyecto.
        /// </summary>
        /// <param name="modeloId">ID del modelo a consultar.</param>
        /// <returns>Detalle del modelo consultado.</returns>
        public async Task<GetProyectoModeloResult> GetProyectoModeloAsync(Guid modeloId)
        {
            var response = await _client.GetAsync($"{ApiRoot}modelos/get/{modeloId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetProyectoModeloResult>();
        }

        /// <summary>
        /// Obtiene el listado de modelos de un proyecto.
        /// </summary>
        /// <param name="proyectoId">ID del proyecto a consultar.</param>
        /// <returns>Lista de modelos del proyecto consultado.</returns>
        public async Task<ListProyectoModelosResult> ListProyectoModelosAsync(Guid proyectoId)
        {
            var response = await _client.GetAsync($"{ApiRoot}modelos/active/{proyectoId}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ListProyectoModelosResult>();
        }

        /// <summary>
        /// Obtiene un valor de configuración específico de la empresa.
        /// </summary>
        /// <param name="empresaId">ID de la empresa a consultar.</param>
        /// <param name="clave">Clave de la configuración a consultar.</param>
        /// <returns>Resultado de la operación y valor de la configuración consultada.</returns>
        public async Task<GetSettingResult> GetSettingAsync(Guid empresaId, string clave)
        {
            var response = await _client.GetAsync($"{ApiRoot}settings/get/{empresaId}/{clave}/{_apiKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GetSettingResult>();
        }
    }
}
