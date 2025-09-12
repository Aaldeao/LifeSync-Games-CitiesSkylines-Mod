# LifeSync-Games – Cities: Skylines Mod  

**LifeSync-Games** es una iniciativa enfocada en el **desarrollo responsable de videojuegos**.  

A través de sensores, se registran tus actividades del mundo real y se convierten en **puntos**, los cuales puedes utilizar dentro del juego.  

Este repositorio contiene un **mod para Cities: Skylines (versión PC)** que podrás conectar tu cuenta, canjear puntos por dinero extra en el juego y recibir notificaciones, por cada hora, que te recomendarán tomar una pausa lejos de la pantalla.

---

## ✨ Características principales  

- 🎮 **Botón LifeSync-Games** integrado en la interfaz del juego, que permite acceder a las nuevas funcionalidades.  
- 🔑 **Inicio de sesión** con tu cuenta de LifeSync-Games directamente desde el juego.  
- 👀 **Visualización de los puntos LSG** dentro del videojuego.  
- 💰 **Canje de puntos por dinero extra**.  
- ⏳ **Gestión del tiempo de juego**, mostrando advertencias si superas los límites de juego saludable luego de haber iniciado sesión.  

---

## 📥 Descargar mod

Si tienes el juego adquirido en **Steam**, puedes descargar el mod directamente desde [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3519587566).  

Solo debes hacer clic en **Suscribirse**, y el mod se descargará automáticamente. Luego aparecerá en el **Gestor de Contenido** de Cities: Skylines, donde podrás activarlo y comenzar a utilizarlo.  

También puedes explorar y descargarlo desde el propio **Gestor de Contenido** del juego, utilizando la opción de búsqueda en **Steam Workshop** con el nombre: **LifeSync Games - Cities: Skylines**   

👉 Si no cuentas con la versión de Steam pero sí con el juego en **PC**, puedes instalar el mod manualmente siguiendo los pasos descritos en la sección de **Instalación**.

---

## 📥 Instalación  

### 🔹 Instalación del mod: 

1. Descarga este repositorio.  
2. Crea una carpeta en una de las siguientes rutas: 
    ```
   <DISCO>\SteamLibrary\steamapps\common\Cities_Skylines\Files\Mods\
    ```  
    ```
   C:\Users\<NOMBRE_DEL_EQUIPO>\AppData\Local\Colossal Order\Cities_Skylines\Addons\Mods\
    ```  
3. Luego de extraer el repositorio, busca y copia los archivos **.dll** necesarios dentro de la carpeta creada:  
- `ModCitiesSkylines.dll` → se encuentra en `ModCitiesSkylines\bin\Debug`  
- `bGamesAPI.dll` → se encuentra en `bGamesAPI\bin\Debug`  
4. Inicia **Cities: Skylines** y en el **Gestor de Contenido** activa el mod.  
5. Es esencial tener en ejecución los **servicios LifeSync-Games** en tu máquina para poder utilizar las funcionalidades del mod (explicado en el siguiente punto).  

---

### 🔹 Instalación de servicios de LifeSync-Games:  

Para instalar los servicios de **LifeSync-Games**, accede al repositorio correspondiente en [GitHub](https://github.com/BlendedGames-bGames/bGames-dev-services) y sigue las instrucciones de instalación.  

Se recomienda utilizar **Docker** para desplegar los servicios de forma sencilla y rápida.