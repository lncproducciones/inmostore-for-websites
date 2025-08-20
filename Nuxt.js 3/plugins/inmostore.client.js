// plugins/inmostore.client.js

/**
 * Este plugin implementa las funcionalidades del API de Portal Services
 * para web, reemplazando las llamadas a $.ajax con $fetch de Nuxt.
 * Se ejecuta solo en el cliente.
 */

export default defineNuxtPlugin(async (nuxtApp) => {
    // Obtenemos las variables públicas de entorno del archivo nuxt.config.ts
    const config = useRuntimeConfig();

    /**
     * @typedef {object} Operacion
     * @property {int} codigo - Código de la operación.
     * @property {guid} newItemId - ID del nuevo elemento creado.
     * @property {int} affectedRows - Número de filas afectadas por la operación.
     * @property {string} mensaje - Mensaje de la operación.
     * @property {string} detalle - Detalle de la operación.
     */

    /**
     * @typedef {object} Elemento
     * @property {int} elementoId - ID del elemento.
     * @property {int} padre - ID del elemento padre.
     * @property {stirng} nombre - Nombre del elemento.
     * @property {string} tooltiop - Tooltip del elemento.
     * @property {string} valor - Valor del elemento.
     * @property {string} defaultValue - Valor por defecto del elemento.
     * @property {int} status - Estado del elemento (0: Inactivo, 1: Activo).
     */

    /**
     * @typedef {object} Escena
     * @property {guid} escenaId - ID de la escena.
     * @property {guid} empresaId - ID de la empresa a la que pertenece la escena.
     * @property {int} modo - Tipo de escena (0: Inmueble, 1: Proyecto).
     * @property {guid} objecto - ID del objeto asociado a la escena.
     * @property {string} titulo - Nombre de la escena.
     * @property {boolean} principal - Indica si es la escena principal.
     * @property {string} imageUrl - URL de la imagen de la escena.
     * @property {int} status - Estado de la escena (0: Inactiva, 1: Activa).
     */

    /**
     * @typedef {object} EscenaPin
     * @property {guid} pinId - ID del pin.
     * @property {guid} empresaId - ID de la empresa a la que pertenece el pin.
     * @property {guid} escenaId - ID de la escena a la que pertenece el pin.
     * @property {string} etiqueta - Texto a mostrar del pin.
     * @property {string} pitch.
     * @property {string} yaw.
     * @property {guid} target - ID de la escena a la que apunta el hotspot.
     * @property {int} status - Estado del pin (0: Inactivo, 1: Activo).
     */

    /**
     * @typedef {object} Imagen
     * @property {guid} imageId - ID de la imagen.
     * @property {guid} empresaId - ID de la empresa a la que pertenece la imagen.
     * @property {int} modo - Modo de la imagen (0: Inmueble, 1: Proyecto).
     * @property {guid} objectId - ID del objeto asociado a la imagen.
     * @property {string} titulo - Título de la imagen.
     * @property {string} imageCaption - Descripción de la imagen.
     * @property {string} imageUrl - URL de la imagen.
     * @property {int} status - Estado de la imagen (0: Inactiva, 1: Activa).
     */

    /**
     * @typedef {object} Inmueble
     * @property {string} inmuebleId - Identificador único del inmueble (UUID).
     * @property {string} empresaId - Identificador de la empresa propietaria (UUID).
     * @property {number} tipoInmueble - Código para el tipo de inmueble (ej. casa, apartamento).
     * @property {number} tipoOperacion - Código para el tipo de operación (ej. venta, renta).
     * @property {string} codigo - Código interno del inmueble.
     * @property {string} titulo - Título de la publicación.
     * @property {string} encabezado - Encabezado de la descripción.
     * @property {string} contenido - Contenido principal de la descripción.
     * @property {number} habitaciones - Número de habitaciones.
     * @property {number} banios - Número de baños.
     * @property {number} terreno - Tamaño del terreno en una unidad específica (ej. m²).
     * @property {number} construccion - Tamaño de la construcción en una unidad específica (ej. m²).
     * @property {number} precioMoneda - Código de la moneda del precio.
     * @property {number} precio - Valor del precio.
     * @property {number} precioTipo - Código para el tipo de precio.
     * @property {number} pais - Código del país.
     * @property {number} estado - Código del estado.
     * @property {number} municipio - Código del municipio.
     * @property {number} ciudad - Código de la ciudad.
     * @property {string} direccion - Dirección completa del inmueble.
     * @property {string} referencia - Referencias adicionales para la ubicación.
     * @property {string} googleMapsScript - Script de Google Maps para mostrar el mapa.
     * @property {number} showDireccionDb - Valor de base de datos para mostrar la dirección.
     * @property {number} showMapaDb - Valor de base de datos para mostrar el mapa.
     * @property {boolean} showDireccion - Booleano para mostrar la dirección.
     * @property {boolean} showMapa - Booleano para mostrar el mapa.
     * @property {number} status - Código de estado de la publicación.
     * @property {string} statusDescr - Descripción del estado de la publicación.
     */

    /**
     * @typedef {object} ComboItem
     * @property {string} texto - Texto a mostrar del item.
     * @property {string} valor - Valor del item.
     */

    /**
     * @typedef {object} CaracteristicasInmueble
     * @description Estructura de datos para las características de un inmueble.
     * @property {string} caracteristicaId - Identificador único de las características (UUID).
     * @property {string} empresaId - Identificador de la empresa propietaria (UUID).
     * @property {string} inmuebleId - Identificador del inmueble al que pertenecen las características (UUID).
     * @property {boolean} nuevaConstruccion - Indica si el inmueble es de nueva construcción.
     * @property {boolean} portada - Indica si el inmueble es destacado en la portada.
     * @property {boolean} amoblado - Indica si el inmueble está amoblado.
     * @property {boolean} cocinaEmpotrada - Indica si el inmueble tiene cocina empotrada.
     * @property {boolean} aireAcondicionado - Indica si tiene aire acondicionado.
     * @property {boolean} piscina - Indica si tiene piscina.
     * @property {boolean} areaSocial - Indica si tiene área social.
     * @property {boolean} areaDeportiva - Indica si tiene área deportiva.
     * @property {boolean} vigilancia - Indica si tiene vigilancia.
     * @property {boolean} ascensor - Indica si tiene ascensor.
     * @property {boolean} camaras - Indica si tiene cámaras de seguridad.
     * @property {boolean} telefono - Indica si tiene servicio de teléfono.
     * @property {boolean} internet - Indica si tiene servicio de internet.
     * @property {boolean} cercoElectrico - Indica si tiene cerco eléctrico.
     * @property {boolean} portonElectrico - Indica si tiene portón eléctrico.
     * @property {boolean} asegurado - Indica si el inmueble está asegurado.
     * @property {boolean} financiamiento - Indica si el inmueble acepta financiamiento.
     * @property {boolean} comercial - Indica si el inmueble tiene uso comercial.
     * @property {boolean} residencial - Indica si el inmueble tiene uso residencial.
     * @property {boolean} patioDelantero - Indica si tiene patio delantero.
     * @property {boolean} patioTrasero - Indica si tiene patio trasero.
     * @property {boolean} calentador - Indica si tiene calentador de agua.
     * @property {boolean} balcon - Indica si tiene balcón.
     * @property {boolean} sotano - Indica si tiene sótano.
     * @property {boolean} armarios - Indica si tiene armarios o clósets.
     * @property {number} puestosEstacionamiento - Número de puestos de estacionamiento.
     * @property {number} antiguedad - Antigüedad del inmueble en años.
     * @property {number} pisos - Número de pisos.
     * @property {number} tipoPiso - Código para el tipo de piso.
     * @property {number} tipoTecho - Código para el tipo de techo.
     * @property {number} estadoActual - Código para el estado de conservación.
     * @property {number} status - Código de estado de la publicación.
     */

    /**
     * @typedef {object} PropiedadInmueble
     * @description Estructura de datos para una propiedad específica de un inmueble.
     * @property {string} propertyId - Identificador único de la propiedad (UUID).
     * @property {string} empresaId - Identificador de la empresa (UUID).
     * @property {string} inmuebleId - Identificador del inmueble al que pertenece la propiedad (UUID).
     * @property {number} tipoPropiedad - Código para el tipo de propiedad.
     * @property {number} coleccionElementoId - Identificador del elemento en la colección.
     * @property {string} property - Valor de la propiedad.
     * @property {number} status - Código de estado de la propiedad.
     * @property {string} creado - Identificador del usuario que creó el registro (UUID).
     * @property {string} creadoFecha - Fecha de creación del registro (formato ISO 8601).
     * @property {string} modificado - Identificador del usuario que modificó el registro (UUID).
     * @property {string} modificadoFecha - Fecha de la última modificación del registro (formato ISO 8601).
     */

    /**
     * @typedef {object} Proyecto
     * @description Estructura de datos para un proyecto inmobiliario.
     * @property {string} proyectoId - Identificador único del proyecto (UUID).
     * @property {string} empresaId - Identificador de la empresa propietaria (UUID).
     * @property {string} nombreProyecto - Nombre del proyecto.
     * @property {number} pais - Código del país.
     * @property {number} estado - Código del estado.
     * @property {number} municipio - Código del municipio.
     * @property {number} ciudad - Código de la ciudad.
     * @property {string} direccion - Dirección completa del proyecto.
     * @property {string} referencia - Referencias adicionales para la ubicación.
     * @property {string} googleMapsScript - Script de Google Maps para mostrar el mapa.
     * @property {string} subdominio - Subdominio asignado al proyecto.
     * @property {boolean} websiteOnline - Indica si el sitio web del proyecto está en línea.
     * @property {number} status - Código de estado del proyecto.
     * @property {string} creado - Identificador del usuario que creó el registro (UUID).
     * @property {string} creadoFecha - Fecha de creación del registro (formato ISO 8601).
     * @property {string} modificado - Identificador del usuario que modificó el registro (UUID).
     * @property {string} modificadoFecha - Fecha de la última modificación del registro (formato ISO 8601).
     */

    /**
     * @typedef {object} Modelo
     * @description Estructura de datos para un modelo de inmueble dentro de un proyecto.
     * @property {string} modeloId - Identificador único del modelo (UUID).
     * @property {string} empresaId - Identificador de la empresa (UUID).
     * @property {string} proyectoId - Identificador del proyecto al que pertenece el modelo (UUID).
     * @property {string} nombre - Nombre del modelo.
     * @property {string} descripcion - Descripción del modelo.
     * @property {string} nivel1 - Nivel jerárquico 1.
     * @property {string} nivel2 - Nivel jerárquico 2.
     * @property {string} nivel3 - Nivel jerárquico 3.
     * @property {string} nivel4 - Nivel jerárquico 4.
     * @property {string} nivel5 - Nivel jerárquico 5.
     * @property {number} status - Código de estado del modelo.
     */

    /**
     * @typedef {object} getElementoResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Elemento} resultado - Detalle del elemento consultado.
     */

    /**
     * @typedef {object} getEmpresaResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {object} resultado - Detalle de la empresa consultada.
     */

    /**
     * @typedef {object} listEscenasResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Escena[]} items - Lista de escenas consultadas.
     */

    /**
     * @typedef {object} listEscenasPinsResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {EscenaPin[]} items - Lista de pins de escena consultados.
     */

    /**
     * @typedef {object} listImagenesResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Imagen[]} items - Lista de imágenes consultadas.
     */

    /**
     * @typedef {object} getImagenPortadaResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Imagen} resultado - Imagen de portada del objeto consultado.
     */

    /**
     * @typedef {object} getInmuebleResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Inmueble} resultado - Detalle del inmueble consultado.
     */

    /**
     * @typedef {object} listInmueblesResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Inmueble[]} items - Lista de inmuebles consultados.
     */

    /**
     * @typedef {object} getComboResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {ComboItem[]} items - Lista de items del combo consultado.
     */

    /**
     * @typedef {object} getCaracteristicasInmuebleResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {CaracteristicasInmueble} resultado - Detalle de las características del inmueble consultado.
     */

    /**
     * @typedef {object} listPropiedadesInmuebleResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {PropiedadInmueble[]} items - Lista de propiedades del inmueble consultado.
     */

    /**
     * @typedef {object} getProyectoResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Proyecto} resultado - Detalle del proyecto consultado.
     */

    /**
     * @typedef {object} listProyectosResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Proyecto[]} items - Lista de proyectos consultados.
     */

    /**
     * @typedef {object} getProyectoModeloResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Modelo} resultado - Detalle del modelo consultado.
     */

    /**
     * @typedef {object} listProyectoModelosResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {Modelo[]} items - Lista de modelos del proyecto consultado.
     */

    /**
     * @typedef {object} getSettingResult
     * @property {Operacion} Operacion - Resultado de la operación.
     * @property {string} resultado - Valor de la configuración consultada.
     */

    /**
     * @class Website
     * Contiene las rutinas para consumir el API de InmoStore.
     */
    class Website{

        constructor(apiId, apiKey) {
            console.log("InmoStore for Nuxt.js");

            this.apiId = apiId;
            this.apiKey = apiKey;
            this.apiRoot = "https://inmostore-api.psweb.me/";
            this.apiVersion = null;
        }
        
        /**
         * Inicia la conexión con el servidor y valida la versión.
         */
        async init() {
            if (!this.apiKey) {
                console.error("*** No se ha detectado la configuración del sitio web. ***");
                window.location.href = "https://www.lncproducciones.com/web";
            }
            else {
                try {
                    const response = await $fetch(`${this.apiRoot}sys/version/${this.apiKey}`);
                    this.apiVersion = response.Resultado;
                    useHead({ meta: [ { name: 'generator', content: 'InmoStore by Portal Services v.' + this.apiVersion } ] });
                    console.log("Versión " + this.apiVersion);
                } catch (error) {
                    console.error("Error al conectar con el servidor de la API:", error);
                }
            }
        }

        /**
         * Obtiene la versión actual del API.
         * @returns {string} La versión del API.
         */
        getCurrentVersion() {
            return this.apiVersion;
        }

        /**
         * Obtiene el detalle de un elemento, consultado por su ID.
         * @param {int} id del elemento a consultar.
         * @returns {getElementoResult} Detalle del elemento consultado.
         */
        getElemento(id) {
            return $fetch(`${this.apiRoot}elementos/get/${id}/${this.apiKey}`);
        }

        /**
         * Obtiene el detalle de la empresa.
         * @param {guid} id - ID de la empresa a consultar.
         * @returns {getEmpresaResult} Detalle de la empresa consultada.
         */
        getEmpresa(id) {
            return $fetch(`${this.apiRoot}empresas/get/${id}/${this.apiKey}`);
        }

        /**
         * Obtiene la lista de escenas activas de un objeto.
         * @param {guid} empresaId - ID de la empresa a consultar.
         * @param {int} modo - Modo de la escena (0: Inmueble, 1: Proyecto).
         * @param {guid} objectId - ID del objeto asociado a la escena.
         * @returns {listEscenasResult} Lista de escenas activas del objeto.
         */
        listEscenas(empresaId, modo, objectId) {
            return $fetch(`${this.apiRoot}escenas/active/${empresaId}/${modo}/${objectId}/${this.apiKey}`);
        }

        /**
         * Obtiene la lista de pins de la escena actual.
         * @param {guid} escenaId - ID de la escena a consultar.
         * @returns {listEscenasPinsResult} Lista de pins de la escena consultada.
         */
        listEscenaPins(escenaId) {
            return $fetch(`${this.apiRoot}pins/list/${escenaId}/${this.apiKey}`);
        }

        /**
         * Obtiene la URL de una imagen para embebder, basado en su id.
         * @param {guid} imageId - ID de la imagen a consultar.
         * @returns {string} URL de la imagen a embebder.
         */
        getEmbededImageUrl(imageId) {
            return `${this.apiRoot}images/embed/${imageId}/${this.apiKey}`;
        }

        /**
         * Obtiene el listado de imágenes de un objeto.
         * @param {int} modo - Modo de la imagen (0: Inmueble, 1: Proyecto).
         * @param {guid} objectId - ID del objeto asociado a la imagen.
         * @returns {listImagenesResult} Lista de imágenes del objeto consultado.
         */
        listImagenes(modo, objectId) {
            return $fetch(`${this.apiRoot}imagenes/list/${modo}/${objectId}/${this.apiKey}`);
        }

        /**
         * Obtiene la imagen de portada de un inmueble.
         * @param {guid} inmuebleId - ID del inmueble a consultar.
         * @returns {getImagenPortadaResult} Imagen de portada del inmueble consultado.
         */
        getImagenPortada(inmuebleId) {
            return $fetch(`${this.apiRoot}imagenes/portada/${inmuebleId}/${this.apiKey}`);
        }

        /**
         * Obtiene el detalle de un inmueble por su ID.
         * @param {guid} inmuebleId - ID del inmueble a consultar.
         * @returns {getInmuebleResult} Detalle del inmueble consultado.
         */
        getInmueble(inmuebleId) {
            return $fetch(`${this.apiRoot}inmuebles/get/${inmuebleId}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de inmuebles filtrados por empresa, tipo de inmueble, tipo de operación y ciudad.
         * @param {guid} empresaId - ID de la empresa a consultar.
         * @param {int} tipoInmueble - Código del tipo de inmueble (ej. casa, apartamento).
         * @param {int} tipoOperacion - Código del tipo de operación (ej. venta, renta).
         * @param {int} ciudad - Código de la ciudad a consultar.
         * @returns {listInmueblesResult} Lista de inmuebles filtrados.
         */
        listInmueblesBy(empresaId, tipoInmueble, tipoOperacion, ciudad) {
            return $fetch(`${this.apiRoot}inmuebles/listweb/${empresaId}/${tipoInmueble}/${tipoOperacion}/${ciudad}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de tipos de inmueble disponibles para una empresa.
         * @param {guid} empresaId - ID de la empresa a consultar.
         * @returns {getComboResult} Lista de tipos de inmueble disponibles.
         */
        listTiposInmueble(empresaId) {
            return $fetch(`${this.apiRoot}inmuebles/tiposInmueble/${empresaId}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de tipos de operación disponibles para una empresa.
         * @param {guid} empresaId - ID de la empresa a consultar.
         * @returns {getComboResult} Lista de tipos de operación disponibles.
         */
        listTiposOperacion(empresaId) {
            return $fetch(`${this.apiRoot}inmuebles/tiposOperacion/${empresaId}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de países disponibles para una empresa.
         * @param {guid} empresaId - ID de la empresa a consultar.
         * @returns {getComboResult} Lista de países disponibles.
         */
        listCiudades(empresaId) {
            return $fetch(`${this.apiRoot}inmuebles/ciudades/${empresaId}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de inmuebles destacados para la portada de una empresa.
         * @param {guid} empresaId - ID de la empresa a consultar.
         * @returns {listInmueblesResult} Lista de inmuebles destacados para la portada.
         */
        listInmueblesPortada(empresaId) {
            return $fetch(`${this.apiRoot}inmuebles/portada/${empresaId}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de inmuebles recientes para una empresa.
         * @param {guid} empresaId - ID de la empresa a consultar.
         * @returns {listInmueblesResult} Lista de inmuebles recientes.
         */
        listInmueblesRecientes(empresaId) {
            return $fetch(`${this.apiRoot}inmuebles/recientes/${empresaId}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de características de un inmueble.
         * @param {guid} inmuebleId - ID del inmueble a consultar.
         * @returns {getCaracteristicasInmuebleResult} Detalle de las características del inmueble consultado.
         */
        getInmuebleCaracteristicas(inmuebleId) {
            return $fetch(`${this.apiRoot}caracteristicas/get/${inmuebleId}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de propiedades de un inmueble.
         * @param {guid} inmuebleId - ID del inmueble a consultar.
         * @param {int} tipoPropiedad - Código del tipo de propiedad a consultar (0, 1).
         * @returns {listPropiedadesInmuebleResult} Lista de propiedades del inmueble consultado.
         */
        listPropiedadesInmueble(inmuebleId, tipoPropiedad) {
            return $fetch(`${this.apiRoot}properties/list/${inmuebleId}/${tipoPropiedad}/${this.apiKey}`);
        }

        /**
         * Obtiene la información de un proyecto inmobiliario por su ID.
         * @param {guid} proyectoId - ID del proyecto a consultar.
         * @returns {getProyectoResult} Detalle del proyecto consultado.
         */
        getProyecto(proyectoId) {
            return $fetch(`${this.apiRoot}proyectos/get/${proyectoId}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de proyectos de una empresa.
         * @param {guid} empresaId - ID de la empresa a consultar.
         * @returns {listProyectosResult} Lista de proyectos de la empresa consultada.
         */
        listProyectos(empresaId) {
            return $fetch(`${this.apiRoot}proyectos/active/${empresaId}/${this.apiKey}`);
        }

        /**
         * Obtiene el ID del proyecto basado en su subdominio.
         * @param {stirng} subdomain - Subdominio del proyecto a consultar.
         * @returns {guid} ID del proyecto asociado al subdominio.
         */
        getProyectoIdBySubdomain(subdomain) {
            return $fetch(`${this.apiRoot}proyectos/pidsubdir/${subdomain}/${this.apiKey}`);
        }

        /**
         * Obtiene el detalle de un modelo de inmueble dentro de un proyecto.
         * @param {guid} modeloId - ID del modelo a consultar.
         * @returns {getProyectoModeloResult} Detalle del modelo consultado.
         */
        getProyectoModelo(modeloId) {
            return $fetch(`${this.apiRoot}modelos/get/${modeloId}/${this.apiKey}`);
        }

        /**
         * Obtiene el listado de modelos de un proyecto.
         * @param {guid} proyectoId - ID del proyecto a consultar.
         * @returns {listProyectoModelosResult} Lista de modelos del proyecto consultado.
         */
        listProyectoModelos(proyectoId) {
            return $fetch(`${this.apiRoot}modelos/active/${proyectoId}/${this.apiKey}`);
        }

        /**
         * Obtiene un valor de configuración específico de la empresa.
         * @param {guid} empresaId - ID de la empresa a consultar.
         * @param {string} clave - Clave de la configuración a consultar.
         * @returns {getSettingResult} Resultado de la operación y valor de la configuración consultada.
         */
        getSetting(empresaId, clave) {
            return $fetch(`${this.apiRoot}settings/get/${empresaId}/${clave}/${this.apiKey}`);
        }
    }

    // Instanciamos la clase con las variables de entorno de Nuxt.
    const inmostoreClient = new Website(config.public.pswebId, config.public.pswebKey);

    // Inicializamos la conexión.
    await inmostoreClient.init();

    // Hacemos el cliente disponible globalmente en la aplicación de Nuxt.
    // Ahora puedes acceder a sus métodos usando `$inmostore`.
    nuxtApp.provide('inmostore', inmostoreClient);
});
