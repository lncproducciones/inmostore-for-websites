// Archivo: InmostoreApiClient.java

import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import org.json.JSONArray;
import org.json.JSONObject;

/**
 * Esta clase implementa las funcionalidades del API de Portal Services
 * para web, adaptando las llamadas del plugin de Nuxt.js a un entorno Java.
 * Se encarga de la comunicación con la API desde el servidor.
 */
public class InmostoreApiClient {

    // Las siguientes constantes son para manejo de errores.
    public static final String ERROR_MESSAGE = "{\"Operacion\":{\"codigo\":-1, \"mensaje\":\"Error en la llamada a la API\"}, \"resultado\":null, \"items\":[]}";
    public static final String API_ROOT = "https://inmostore-api.psweb.me/";

    private String apiId;
    private String apiKey;

    /**
     * Constructor de la clase InmostoreApiClient.
     * @param apiId ID de la API (generalmente el ID de la empresa).
     * @param apiKey Clave de la API.
     */
    public InmostoreApiClient(String apiId, String apiKey) {
        System.out.println("InmoStore API Client for Java");
        this.apiId = apiId;
        this.apiKey = apiKey;

        // Si la clave de la API no está configurada, se muestra un error en la consola del servidor.
        if (this.apiKey == null || this.apiKey.isEmpty()) {
            System.err.println("*** No se ha detectado la configuración del sitio web. ***");
            // No se puede redirigir desde el servidor como en el JS, solo se imprime el error.
        }
    }

    /**
     * Helper para realizar una llamada GET a la API y devolver el resultado como String JSON.
     * @param url La URL completa de la API.
     * @return String JSON con el resultado de la llamada, o un JSON de error si falla.
     */
    private String getApiResponse(String url) {
        HttpClient client = HttpClient.newHttpClient();
        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(url))
                .build();
        try {
            HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());
            if (response.statusCode() == 200) {
                return response.body();
            } else {
                System.err.println("Error en la respuesta de la API: " + response.statusCode());
                return ERROR_MESSAGE;
            }
        } catch (IOException | InterruptedException e) {
            System.err.println("Error al conectar con el servidor de la API: " + e.getMessage());
            return ERROR_MESSAGE;
        }
    }

    /**
     * Obtiene el detalle de un elemento, consultado por su ID.
     * @param id ID del elemento a consultar.
     * @return String JSON con el detalle del elemento.
     */
    public String getElemento(int id) {
        String url = API_ROOT + "elementos/get/" + id + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el detalle de la empresa.
     * @param id ID de la empresa a consultar.
     * @return String JSON con el detalle de la empresa.
     */
    public String getEmpresa(String id) {
        String url = API_ROOT + "empresas/get/" + id + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene la lista de escenas activas de un objeto.
     * @param empresaId ID de la empresa a consultar.
     * @param modo Modo de la escena (0: Inmueble, 1: Proyecto).
     * @param objectId ID del objeto asociado a la escena.
     * @return String JSON con la lista de escenas.
     */
    public String listEscenas(String empresaId, int modo, String objectId) {
        String url = API_ROOT + "escenas/active/" + empresaId + "/" + modo + "/" + objectId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene la lista de pins de la escena actual.
     * @param escenaId ID de la escena a consultar.
     * @return String JSON con la lista de pins de la escena.
     */
    public String listEscenaPins(String escenaId) {
        String url = API_ROOT + "pins/list/" + escenaId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene la URL de una imagen para embeber.
     * @param imageId ID de la imagen a consultar.
     * @return String con la URL de la imagen.
     */
    public String getEmbededImageUrl(String imageId) {
        return API_ROOT + "images/embed/" + imageId + "/" + this.apiKey;
    }

    /**
     * Obtiene el listado de imágenes de un objeto.
     * @param modo Modo de la imagen (0: Inmueble, 1: Proyecto).
     * @param objectId ID del objeto asociado a la imagen.
     * @return String JSON con la lista de imágenes.
     */
    public String listImagenes(int modo, String objectId) {
        String url = API_ROOT + "imagenes/list/" + modo + "/" + objectId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene la imagen de portada de un inmueble.
     * @param inmuebleId ID del inmueble a consultar.
     * @return String JSON con la imagen de portada.
     */
    public String getImagenPortada(String inmuebleId) {
        String url = API_ROOT + "imagenes/portada/" + inmuebleId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el detalle de un inmueble por su ID.
     * @param inmuebleId ID del inmueble a consultar.
     * @return String JSON con el detalle del inmueble.
     */
    public String getInmueble(String inmuebleId) {
        String url = API_ROOT + "inmuebles/get/" + inmuebleId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de inmuebles filtrados.
     * @param empresaId ID de la empresa.
     * @param tipoInmueble Tipo de inmueble.
     * @param tipoOperacion Tipo de operación.
     * @param ciudad Ciudad.
     * @return String JSON con la lista de inmuebles.
     */
    public String listInmueblesBy(String empresaId, int tipoInmueble, int tipoOperacion, int ciudad) {
        String url = API_ROOT + "inmuebles/listweb/" + empresaId + "/" + tipoInmueble + "/" + tipoOperacion + "/" + ciudad + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de tipos de inmueble disponibles para una empresa.
     * @param empresaId ID de la empresa.
     * @return String JSON con la lista de tipos de inmueble.
     */
    public String listTiposInmueble(String empresaId) {
        String url = API_ROOT + "inmuebles/tiposInmueble/" + empresaId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de tipos de operación disponibles para una empresa.
     * @param empresaId ID de la empresa.
     * @return String JSON con la lista de tipos de operación.
     */
    public String listTiposOperacion(String empresaId) {
        String url = API_ROOT + "inmuebles/tiposOperacion/" + empresaId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de ciudades disponibles para una empresa.
     * @param empresaId ID de la empresa.
     * @return String JSON con la lista de ciudades.
     */
    public String listCiudades(String empresaId) {
        String url = API_ROOT + "inmuebles/ciudades/" + empresaId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de inmuebles destacados para la portada.
     * @param empresaId ID de la empresa.
     * @return String JSON con la lista de inmuebles.
     */
    public String listInmueblesPortada(String empresaId) {
        String url = API_ROOT + "inmuebles/portada/" + empresaId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de inmuebles recientes.
     * @param empresaId ID de la empresa.
     * @return String JSON con la lista de inmuebles.
     */
    public String listInmueblesRecientes(String empresaId) {
        String url = API_ROOT + "inmuebles/recientes/" + empresaId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de características de un inmueble.
     * @param inmuebleId ID del inmueble.
     * @return String JSON con las características del inmueble.
     */
    public String getInmuebleCaracteristicas(String inmuebleId) {
        String url = API_ROOT + "caracteristicas/get/" + inmuebleId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de propiedades de un inmueble.
     * @param inmuebleId ID del inmueble.
     * @param tipoPropiedad Tipo de propiedad.
     * @return String JSON con la lista de propiedades.
     */
    public String listPropiedadesInmueble(String inmuebleId, int tipoPropiedad) {
        String url = API_ROOT + "properties/list/" + inmuebleId + "/" + tipoPropiedad + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene la información de un proyecto inmobiliario por su ID.
     * @param proyectoId ID del proyecto.
     * @return String JSON con los detalles del proyecto.
     */
    public String getProyecto(String proyectoId) {
        String url = API_ROOT + "proyectos/get/" + proyectoId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de proyectos de una empresa.
     * @param empresaId ID de la empresa.
     * @return String JSON con la lista de proyectos.
     */
    public String listProyectos(String empresaId) {
        String url = API_ROOT + "proyectos/active/" + empresaId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el ID del proyecto basado en su subdominio.
     * @param subdomain Subdominio del proyecto.
     * @return String JSON con el ID del proyecto.
     */
    public String getProyectoIdBySubdomain(String subdomain) {
        String url = API_ROOT + "proyectos/pidsubdir/" + subdomain + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el detalle de un modelo de inmueble dentro de un proyecto.
     * @param modeloId ID del modelo.
     * @return String JSON con el detalle del modelo.
     */
    public String getProyectoModelo(String modeloId) {
        String url = API_ROOT + "modelos/get/" + modeloId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene el listado de modelos de un proyecto.
     * @param proyectoId ID del proyecto.
     * @return String JSON con la lista de modelos.
     */
    public String listProyectoModelos(String proyectoId) {
        String url = API_ROOT + "modelos/active/" + proyectoId + "/" + this.apiKey;
        return getApiResponse(url);
    }

    /**
     * Obtiene un valor de configuración específico de la empresa.
     * @param empresaId ID de la empresa.
     * @param clave Clave de la configuración.
     * @return String JSON con el valor de la configuración.
     */
    public String getSetting(String empresaId, String clave) {
        String url = API_ROOT + "settings/get/" + empresaId + "/" + clave + "/" + this.apiKey;
        return getApiResponse(url);
    }
}
