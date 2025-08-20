# -*- coding: utf-8 -*-
"""
Este módulo implementa una clase de Python para consumir el API de Portal Services.
Reemplaza las funcionalidades del plugin de Nuxt.js para su uso en entornos de servidor.
"""

import requests

class Website:
    """
    Contiene las rutinas para consumir el API de InmoStore.
    """

    # Definiciones de tipo para la documentación
    # (No son clases, solo referencias para la claridad)
    # ---
    # :type Operacion: dict
    # :param Operacion:
    #     codigo (int): Código de la operación.
    #     newItemId (str): ID del nuevo elemento creado (GUID).
    #     affectedRows (int): Número de filas afectadas por la operación.
    #     mensaje (str): Mensaje de la operación.
    #     detalle (str): Detalle de la operación.
    #
    # :type Elemento: dict
    # :param Elemento:
    #     elementoId (int): ID del elemento.
    #     padre (int): ID del elemento padre.
    #     nombre (str): Nombre del elemento.
    #     tooltiop (str): Tooltip del elemento.
    #     valor (str): Valor del elemento.
    #     defaultValue (str): Valor por defecto del elemento.
    #     status (int): Estado del elemento (0: Inactivo, 1: Activo).
    #
    # :type Escena: dict
    # :param Escena:
    #     escenaId (str): ID de la escena (GUID).
    #     empresaId (str): ID de la empresa a la que pertenece la escena (GUID).
    #     modo (int): Tipo de escena (0: Inmueble, 1: Proyecto).
    #     objecto (str): ID del objeto asociado a la escena (GUID).
    #     titulo (str): Nombre de la escena.
    #     principal (bool): Indica si es la escena principal.
    #     imageUrl (str): URL de la imagen de la escena.
    #     status (int): Estado de la escena (0: Inactiva, 1: Activa).
    #
    # :type EscenaPin: dict
    # :param EscenaPin:
    #     pinId (str): ID del pin (GUID).
    #     empresaId (str): ID de la empresa a la que pertenece el pin (GUID).
    #     escenaId (str): ID de la escena a la que pertenece el pin (GUID).
    #     etiqueta (str): Texto a mostrar del pin.
    #     pitch (str): ...
    #     yaw (str): ...
    #     target (str): ID de la escena a la que apunta el hotspot (GUID).
    #     status (int): Estado del pin (0: Inactivo, 1: Activo).
    #
    # :type Imagen: dict
    # :param Imagen:
    #     imageId (str): ID de la imagen (GUID).
    #     empresaId (str): ID de la empresa a la que pertenece la imagen (GUID).
    #     modo (int): Modo de la imagen (0: Inmueble, 1: Proyecto).
    #     objectId (str): ID del objeto asociado a la imagen (GUID).
    #     titulo (str): Título de la imagen.
    #     imageCaption (str): Descripción de la imagen.
    #     imageUrl (str): URL de la imagen.
    #     status (int): Estado de la imagen (0: Inactiva, 1: Activa).
    #
    # :type Inmueble: dict
    # :param Inmueble:
    #     (contiene muchas propiedades, ver el código JavaScript original)
    #
    # :type ComboItem: dict
    # :param ComboItem:
    #     texto (str): Texto a mostrar del item.
    #     valor (str): Valor del item.
    #
    # :type CaracteristicasInmueble: dict
    # :param CaracteristicasInmueble:
    #     (contiene muchas propiedades, ver el código JavaScript original)
    #
    # :type PropiedadInmueble: dict
    # :param PropiedadInmueble:
    #     (contiene muchas propiedades, ver el código JavaScript original)
    #
    # :type Proyecto: dict
    # :param Proyecto:
    #     (contiene muchas propiedades, ver el código JavaScript original)
    #
    # :type Modelo: dict
    # :param Modelo:
    #     (contiene muchas propiedades, ver el código JavaScript original)
    #
    # :type getElementoResult: dict
    # :param getElementoResult:
    #     Operacion (Operacion): Resultado de la operación.
    #     resultado (Elemento): Detalle del elemento consultado.
    #
    # ... y así sucesivamente para todas las estructuras de datos.

    def __init__(self, api_id: str, api_key: str):
        """
        Inicializa la clase.

        :param api_id: El ID de la API.
        :param api_key: La clave de la API.
        """
        print("InmoStore para Python")

        self.api_id = api_id
        self.api_key = api_key
        self.api_root = "https://inmostore-api.psweb.me/"
        self.api_version = None

    def _fetch(self, url: str):
        """
        Realiza una llamada HTTP asíncrona.

        Este es un método privado que se encarga de la lógica de la petición
        y el manejo básico de errores.

        :param url: La URL de la API a la que se va a llamar.
        :return: El JSON de la respuesta.
        :raises requests.exceptions.RequestException: Si ocurre un error de red o de HTTP.
        """
        try:
            response = requests.get(url, timeout=10)
            response.raise_for_status()  # Lanza una excepción para códigos de estado 4xx/5xx
            return response.json()
        except requests.exceptions.RequestException as e:
            print(f"Error en la llamada a la API: {e}")
            raise

    def init(self):
        """
        Inicia la conexión con el servidor y valida la versión.

        Nota: A diferencia de la versión de Nuxt.js, este código no puede
        redirigir la página ni modificar el HTML. Simplemente imprime un
        mensaje en la consola.
        """
        if not self.api_key:
            print("*** No se ha detectado la configuración del sitio web. ***")
        else:
            try:
                response = self._fetch(f"{self.api_root}sys/version/{self.api_key}")
                self.api_version = response.get("Resultado")
                print(f"Versión {self.api_version}")
            except requests.exceptions.RequestException as e:
                print(f"Error al conectar con el servidor de la API: {e}")

    def get_current_version(self) -> str:
        """
        Obtiene la versión actual del API.

        :returns: La versión del API.
        """
        return self.api_version

    def get_elemento(self, id: int) -> dict:
        """
        Obtiene el detalle de un elemento, consultado por su ID.

        :param id: ID del elemento a consultar.
        :returns: Detalle del elemento consultado.
        """
        return self._fetch(f"{self.api_root}elementos/get/{id}/{self.api_key}")

    def get_empresa(self, id: str) -> dict:
        """
        Obtiene el detalle de la empresa.

        :param id: ID de la empresa a consultar.
        :returns: Detalle de la empresa consultada.
        """
        return self._fetch(f"{self.api_root}empresas/get/{id}/{self.api_key}")

    def list_escenas(self, empresa_id: str, modo: int, object_id: str) -> dict:
        """
        Obtiene la lista de escenas activas de un objeto.

        :param empresa_id: ID de la empresa a consultar.
        :param modo: Modo de la escena (0: Inmueble, 1: Proyecto).
        :param object_id: ID del objeto asociado a la escena.
        :returns: Lista de escenas activas del objeto.
        """
        return self._fetch(f"{self.api_root}escenas/active/{empresa_id}/{modo}/{object_id}/{self.api_key}")

    def list_escena_pins(self, escena_id: str) -> dict:
        """
        Obtiene la lista de pins de la escena actual.

        :param escena_id: ID de la escena a consultar.
        :returns: Lista de pins de la escena consultada.
        """
        return self._fetch(f"{self.api_root}pins/list/{escena_id}/{self.api_key}")

    def get_embeded_image_url(self, image_id: str) -> str:
        """
        Obtiene la URL de una imagen para embeber, basado en su id.

        :param image_id: ID de la imagen a consultar.
        :returns: URL de la imagen a embeber.
        """
        return f"{self.api_root}images/embed/{image_id}/{self.api_key}"

    def list_imagenes(self, modo: int, object_id: str) -> dict:
        """
        Obtiene el listado de imágenes de un objeto.

        :param modo: Modo de la imagen (0: Inmueble, 1: Proyecto).
        :param object_id: ID del objeto asociado a la imagen.
        :returns: Lista de imágenes del objeto consultado.
        """
        return self._fetch(f"{self.api_root}imagenes/list/{modo}/{object_id}/{self.api_key}")

    def get_imagen_portada(self, inmueble_id: str) -> dict:
        """
        Obtiene la imagen de portada de un inmueble.

        :param inmueble_id: ID del inmueble a consultar.
        :returns: Imagen de portada del inmueble consultado.
        """
        return self._fetch(f"{self.api_root}imagenes/portada/{inmueble_id}/{self.api_key}")

    def get_inmueble(self, inmueble_id: str) -> dict:
        """
        Obtiene el detalle de un inmueble por su ID.

        :param inmueble_id: ID del inmueble a consultar.
        :returns: Detalle del inmueble consultado.
        """
        return self._fetch(f"{self.api_root}inmuebles/get/{inmueble_id}/{self.api_key}")

    def list_inmuebles_by(self, empresa_id: str, tipo_inmueble: int, tipo_operacion: int, ciudad: int) -> dict:
        """
        Obtiene el listado de inmuebles filtrados por empresa, tipo de inmueble, tipo de operación y ciudad.

        :param empresa_id: ID de la empresa a consultar.
        :param tipo_inmueble: Código del tipo de inmueble (ej. casa, apartamento).
        :param tipo_operacion: Código del tipo de operación (ej. venta, renta).
        :param ciudad: Código de la ciudad a consultar.
        :returns: Lista de inmuebles filtrados.
        """
        return self._fetch(f"{self.api_root}inmuebles/listweb/{empresa_id}/{tipo_inmueble}/{tipo_operacion}/{ciudad}/{self.api_key}")

    def list_tipos_inmueble(self, empresa_id: str) -> dict:
        """
        Obtiene el listado de tipos de inmueble disponibles para una empresa.

        :param empresa_id: ID de la empresa a consultar.
        :returns: Lista de tipos de inmueble disponibles.
        """
        return self._fetch(f"{self.api_root}inmuebles/tiposInmueble/{empresa_id}/{self.api_key}")

    def list_tipos_operacion(self, empresa_id: str) -> dict:
        """
        Obtiene el listado de tipos de operación disponibles para una empresa.

        :param empresa_id: ID de la empresa a consultar.
        :returns: Lista de tipos de operación disponibles.
        """
        return self._fetch(f"{self.api_root}inmuebles/tiposOperacion/{empresa_id}/{self.api_key}")

    def list_ciudades(self, empresa_id: str) -> dict:
        """
        Obtiene el listado de ciudades disponibles para una empresa.

        :param empresa_id: ID de la empresa a consultar.
        :returns: Lista de ciudades disponibles.
        """
        return self._fetch(f"{self.api_root}inmuebles/ciudades/{empresa_id}/{self.api_key}")

    def list_inmuebles_portada(self, empresa_id: str) -> dict:
        """
        Obtiene el listado de inmuebles destacados para la portada de una empresa.

        :param empresa_id: ID de la empresa a consultar.
        :returns: Lista de inmuebles destacados para la portada.
        """
        return self._fetch(f"{self.api_root}inmuebles/portada/{empresa_id}/{self.api_key}")

    def list_inmuebles_recientes(self, empresa_id: str) -> dict:
        """
        Obtiene el listado de inmuebles recientes para una empresa.

        :param empresa_id: ID de la empresa a consultar.
        :returns: Lista de inmuebles recientes.
        """
        return self._fetch(f"{self.api_root}inmuebles/recientes/{empresa_id}/{self.api_key}")

    def get_inmueble_caracteristicas(self, inmueble_id: str) -> dict:
        """
        Obtiene el listado de características de un inmueble.

        :param inmueble_id: ID del inmueble a consultar.
        :returns: Detalle de las características del inmueble consultado.
        """
        return self._fetch(f"{self.api_root}caracteristicas/get/{inmueble_id}/{self.api_key}")

    def list_propiedades_inmueble(self, inmueble_id: str, tipo_propiedad: int) -> dict:
        """
        Obtiene el listado de propiedades de un inmueble.

        :param inmueble_id: ID del inmueble a consultar.
        :param tipo_propiedad: Código del tipo de propiedad a consultar (0, 1).
        :returns: Lista de propiedades del inmueble consultado.
        """
        return self._fetch(f"{self.api_root}properties/list/{inmueble_id}/{tipo_propiedad}/{self.api_key}")

    def get_proyecto(self, proyecto_id: str) -> dict:
        """
        Obtiene la información de un proyecto inmobiliario por su ID.

        :param proyecto_id: ID del proyecto a consultar.
        :returns: Detalle del proyecto consultado.
        """
        return self._fetch(f"{self.api_root}proyectos/get/{proyecto_id}/{self.api_key}")

    def list_proyectos(self, empresa_id: str) -> dict:
        """
        Obtiene el listado de proyectos de una empresa.

        :param empresa_id: ID de la empresa a consultar.
        :returns: Lista de proyectos de la empresa consultada.
        """
        return self._fetch(f"{self.api_root}proyectos/active/{empresa_id}/{self.api_key}")

    def get_proyecto_id_by_subdomain(self, subdomain: str) -> dict:
        """
        Obtiene el ID del proyecto basado en su subdominio.

        :param subdomain: Subdominio del proyecto a consultar.
        :returns: ID del proyecto asociado al subdominio.
        """
        return self._fetch(f"{self.api_root}proyectos/pidsubdir/{subdomain}/{self.api_key}")

    def get_proyecto_modelo(self, modelo_id: str) -> dict:
        """
        Obtiene el detalle de un modelo de inmueble dentro de un proyecto.

        :param modelo_id: ID del modelo a consultar.
        :returns: Detalle del modelo consultado.
        """
        return self._fetch(f"{self.api_root}modelos/get/{modelo_id}/{self.api_key}")

    def list_proyecto_modelos(self, proyecto_id: str) -> dict:
        """
        Obtiene el listado de modelos de un proyecto.

        :param proyecto_id: ID del proyecto a consultar.
        :returns: Lista de modelos del proyecto consultado.
        """
        return self._fetch(f"{self.api_root}modelos/active/{proyecto_id}/{self.api_key}")

    def get_setting(self, empresa_id: str, clave: str) -> dict:
        """
        Obtiene un valor de configuración específico de la empresa.

        :param empresa_id: ID de la empresa a consultar.
        :param clave: Clave de la configuración a consultar.
        :returns: Resultado de la operación y valor de la configuración consultada.
        """
        return self._fetch(f"{self.api_root}settings/get/{empresa_id}/{clave}/{self.api_key}")


if __name__ == "__main__":
    # Ejemplo de uso en un entorno local (fuera de un framework web)
    # Reemplaza con tus valores reales de API
    api_id = "TU_API_ID"
    api_key = "TU_API_KEY"

    inmostore_client = Website(api_id, api_key)

    try:
        inmostore_client.init()

        if inmostore_client.get_current_version():
            print(f"Versión de la API: {inmostore_client.get_current_version()}")

        # Ejemplo de una llamada a la API
        # Suponiendo que tienes un ID de empresa y de inmueble
        # inmueble_id_ejemplo = "ID_DEL_INMUEBLE_A_CONSULTAR"
        # inmueble = inmostore_client.get_inmueble(inmueble_id_ejemplo)
        # print(inmueble)

    except Exception as e:
        print(f"La aplicación falló: {e}")
