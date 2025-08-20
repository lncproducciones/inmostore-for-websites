<%@ page import="com.tudominio.InmostoreApiClient" %>
<%@ page import="org.json.JSONObject" %>
<%@ page import="org.json.JSONArray" %>

<%--
  Ejemplo de uso de InmostoreApiClient en una página JSP.
  Asegúrate de tener el archivo InmostoreApiClient.java compilado y en el classpath de tu proyecto web.
  También necesitarás la librería org.json.jar en tu carpeta lib.
--%>
<!DOCTYPE html>
<html>
<head>
    <title>InmoStore API Client en JSP</title>
</head>
<body>
    <h1>Información del Inmueble</h1>

    <%
        // Reemplaza estos valores con tu ID y clave de la API reales
        String pswebId = "tu-id-de-api-aqui";
        String pswebKey = "tu-clave-de-api-aqui";
        String inmuebleId = "9c235472-3580-4966-ac15-3746c483a910"; // Ejemplo de un ID de inmueble

        // Instanciar el cliente de la API
        InmostoreApiClient apiClient = new InmostoreApiClient(pswebId, pswebKey);
        
        // Llamar a uno de los métodos de la API
        String inmuebleJson = apiClient.getInmueble(inmuebleId);
        
        // Procesar la respuesta JSON
        try {
            JSONObject jsonObject = new JSONObject(inmuebleJson);
            JSONObject operacion = jsonObject.getJSONObject("Operacion");
            JSONObject resultado = jsonObject.getJSONObject("resultado");
            
            if (operacion.getInt("codigo") == 1) {
                String titulo = resultado.getString("titulo");
                String descripcion = resultado.getString("contenido");
                double precio = resultado.getDouble("precio");

                out.println("<h2>Título: " + titulo + "</h2>");
                out.println("<p>Descripción: " + descripcion + "</p>");
                out.println("<p>Precio: " + precio + "</p>");
            } else {
                out.println("<p>Error en la operación: " + operacion.getString("mensaje") + "</p>");
            }
        } catch (Exception e) {
            out.println("<p>Error al procesar la respuesta JSON: " + e.getMessage() + "</p>");
        }
    %>

</body>
</html>
