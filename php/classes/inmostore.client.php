<?php

/**
 * Clase que implementa las funcionalidades del API de InmoStore para web.
 * Esta clase replica el comportamiento del plugin de Nuxt.js, utilizando cURL
 * para realizar las peticiones HTTP a la API.
 *
 * NOTA: Esta clase asume que la extensión cURL está habilitada en su instalación de PHP.
 *
 * @package InmoStore
 */
class InmoStore
{
    /**
     * @var string La URL raíz del API.
     */
    private const API_ROOT = "https://inmostore-api.psweb.me/";

    /**
     * @var string El ID de la API.
     */
    private $apiId;

    /**
     * @var string La clave de la API.
     */
    private $apiKey;

    /**
     * @var string|null La versión actual de la API.
     */
    private $apiVersion;

    /**
     * Constructor de la clase InmoStore.
     *
     * @param string $apiId El ID de la API proporcionado.
     * @param string $apiKey La clave de la API proporcionada.
     */
    public function __construct(string $apiId, string $apiKey)
    {
        $this->apiId = $apiId;
        $this->apiKey = $apiKey;
        $this->apiVersion = null;
    }

    /**
     * Inicia la conexión con el servidor y valida la versión.
     *
     * @return bool Verdadero si la inicialización es exitosa, falso en caso de error.
     */
    public function init(): bool
    {
        if (empty($this->apiKey)) {
            // En el original de JS, esto redirigía. Aquí solo imprimimos el error.
            error_log("*** No se ha detectado la configuración del sitio web. ***");
            return false;
        }

        try {
            $response = $this->makeApiCall("sys/version/{$this->apiKey}");
            if ($response && isset($response->Resultado)) {
                $this->apiVersion = $response->Resultado;
                // La parte de Nuxt.js de 'useHead' no tiene un equivalente directo en PHP del lado del servidor.
                // Podrías usar esto para configurar un encabezado en tu HTML si lo deseas.
                return true;
            }
        } catch (Exception $e) {
            error_log("Error al conectar con el servidor de la API: " . $e->getMessage());
        }

        return false;
    }

    /**
     * Obtiene la versión actual del API.
     *
     * @return string|null La versión del API, o null si no se ha inicializado.
     */
    public function getCurrentVersion(): ?string
    {
        return $this->apiVersion;
    }

    /**
     * Obtiene el detalle de un elemento, consultado por su ID.
     *
     * @param int $id El ID del elemento a consultar.
     * @return object|null El resultado de la operación.
     */
    public function getElemento(int $id): ?object
    {
        return $this->makeApiCall("elementos/get/{$id}/{$this->apiKey}");
    }

    /**
     * Obtiene el detalle de la empresa.
     *
     * @param string $id El ID de la empresa a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function getEmpresa(string $id): ?object
    {
        return $this->makeApiCall("empresas/get/{$id}/{$this->apiKey}");
    }

    /**
     * Obtiene la lista de escenas activas de un objeto.
     *
     * @param string $empresaId El ID de la empresa a consultar (GUID).
     * @param int $modo El modo de la escena (0: Inmueble, 1: Proyecto).
     * @param string $objectId El ID del objeto asociado a la escena (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listEscenas(string $empresaId, int $modo, string $objectId): ?object
    {
        return $this->makeApiCall("escenas/active/{$empresaId}/{$modo}/{$objectId}/{$this->apiKey}");
    }

    /**
     * Obtiene la lista de pins de la escena actual.
     *
     * @param string $escenaId El ID de la escena a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listEscenaPins(string $escenaId): ?object
    {
        return $this->makeApiCall("pins/list/{$escenaId}/{$this->apiKey}");
    }

    /**
     * Obtiene la URL de una imagen para embeber, basado en su ID.
     *
     * @param string $imageId El ID de la imagen a consultar (GUID).
     * @return string La URL de la imagen a embeber.
     */
    public function getEmbededImageUrl(string $imageId): string
    {
        return self::API_ROOT . "images/embed/{$imageId}/{$this->apiKey}";
    }

    /**
     * Obtiene el listado de imágenes de un objeto.
     *
     * @param int $modo El modo de la imagen (0: Inmueble, 1: Proyecto).
     * @param string $objectId El ID del objeto asociado a la imagen (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listImagenes(int $modo, string $objectId): ?object
    {
        return $this->makeApiCall("imagenes/list/{$modo}/{$objectId}/{$this->apiKey}");
    }

    /**
     * Obtiene la imagen de portada de un inmueble.
     *
     * @param string $inmuebleId El ID del inmueble a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function getImagenPortada(string $inmuebleId): ?object
    {
        return $this->makeApiCall("imagenes/portada/{$inmuebleId}/{$this->apiKey}");
    }

    /**
     * Obtiene el detalle de un inmueble por su ID.
     *
     * @param string $inmuebleId El ID del inmueble a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function getInmueble(string $inmuebleId): ?object
    {
        return $this->makeApiCall("inmuebles/get/{$inmuebleId}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de inmuebles filtrados.
     *
     * @param string $empresaId El ID de la empresa a consultar (GUID).
     * @param int $tipoInmueble El código del tipo de inmueble.
     * @param int $tipoOperacion El código del tipo de operación.
     * @param int $ciudad El código de la ciudad.
     * @return object|null El resultado de la operación.
     */
    public function listInmueblesBy(string $empresaId, int $tipoInmueble, int $tipoOperacion, int $ciudad): ?object
    {
        return $this->makeApiCall("inmuebles/listweb/{$empresaId}/{$tipoInmueble}/{$tipoOperacion}/{$ciudad}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de tipos de inmueble disponibles.
     *
     * @param string $empresaId El ID de la empresa a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listTiposInmueble(string $empresaId): ?object
    {
        return $this->makeApiCall("inmuebles/tiposInmueble/{$empresaId}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de tipos de operación disponibles.
     *
     * @param string $empresaId El ID de la empresa a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listTiposOperacion(string $empresaId): ?object
    {
        return $this->makeApiCall("inmuebles/tiposOperacion/{$empresaId}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de ciudades disponibles.
     *
     * @param string $empresaId El ID de la empresa a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listCiudades(string $empresaId): ?object
    {
        return $this->makeApiCall("inmuebles/ciudades/{$empresaId}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de inmuebles destacados para la portada.
     *
     * @param string $empresaId El ID de la empresa a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listInmueblesPortada(string $empresaId): ?object
    {
        return $this->makeApiCall("inmuebles/portada/{$empresaId}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de inmuebles recientes.
     *
     * @param string $empresaId El ID de la empresa a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listInmueblesRecientes(string $empresaId): ?object
    {
        return $this->makeApiCall("inmuebles/recientes/{$empresaId}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de características de un inmueble.
     *
     * @param string $inmuebleId El ID del inmueble a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function getInmuebleCaracteristicas(string $inmuebleId): ?object
    {
        return $this->makeApiCall("caracteristicas/get/{$inmuebleId}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de propiedades de un inmueble.
     *
     * @param string $inmuebleId El ID del inmueble a consultar (GUID).
     * @param int $tipoPropiedad El código del tipo de propiedad.
     * @return object|null El resultado de la operación.
     */
    public function listPropiedadesInmueble(string $inmuebleId, int $tipoPropiedad): ?object
    {
        return $this->makeApiCall("properties/list/{$inmuebleId}/{$tipoPropiedad}/{$this->apiKey}");
    }

    /**
     * Obtiene la información de un proyecto inmobiliario por su ID.
     *
     * @param string $proyectoId El ID del proyecto a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function getProyecto(string $proyectoId): ?object
    {
        return $this->makeApiCall("proyectos/get/{$proyectoId}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de proyectos de una empresa.
     *
     * @param string $empresaId El ID de la empresa a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listProyectos(string $empresaId): ?object
    {
        return $this->makeApiCall("proyectos/active/{$empresaId}/{$this->apiKey}");
    }

    /**
     * Obtiene el ID del proyecto basado en su subdominio.
     *
     * @param string $subdomain El subdominio del proyecto a consultar.
     * @return object|null El resultado de la operación.
     */
    public function getProyectoIdBySubdomain(string $subdomain): ?object
    {
        return $this->makeApiCall("proyectos/pidsubdir/{$subdomain}/{$this->apiKey}");
    }

    /**
     * Obtiene el detalle de un modelo de inmueble dentro de un proyecto.
     *
     * @param string $modeloId El ID del modelo a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function getProyectoModelo(string $modeloId): ?object
    {
        return $this->makeApiCall("modelos/get/{$modeloId}/{$this->apiKey}");
    }

    /**
     * Obtiene el listado de modelos de un proyecto.
     *
     * @param string $proyectoId El ID del proyecto a consultar (GUID).
     * @return object|null El resultado de la operación.
     */
    public function listProyectoModelos(string $proyectoId): ?object
    {
        return $this->makeApiCall("modelos/active/{$proyectoId}/{$this->apiKey}");
    }

    /**
     * Obtiene un valor de configuración específico de la empresa.
     *
     * @param string $empresaId El ID de la empresa a consultar (GUID).
     * @param string $clave La clave de la configuración a consultar.
     * @return object|null El resultado de la operación.
     */
    public function getSetting(string $empresaId, string $clave): ?object
    {
        return $this->makeApiCall("settings/get/{$empresaId}/{$clave}/{$this->apiKey}");
    }

    /**
     * Método privado para realizar las llamadas al API.
     *
     * @param string $endpoint El endpoint específico del API.
     * @return object|null El objeto decodificado de la respuesta JSON, o null en caso de error.
     */
    private function makeApiCall(string $endpoint): ?object
    {
        $url = self::API_ROOT . $endpoint;
        $ch = curl_init($url);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_HTTPHEADER, ['Content-Type: application/json']);
        $response = curl_exec($ch);
        $httpCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);

        if (curl_errno($ch)) {
            error_log('Error cURL: ' . curl_error($ch));
            curl_close($ch);
            return null;
        }

        curl_close($ch);

        if ($httpCode !== 200) {
            error_log("Error en la petición API: {$url} con código HTTP {$httpCode}");
            return null;
        }

        $decodedResponse = json_decode($response);
        if (json_last_error() !== JSON_ERROR_NONE) {
            error_log('Error de decodificación JSON: ' . json_last_error_msg());
            return null;
        }

        return $decodedResponse;
    }
}
