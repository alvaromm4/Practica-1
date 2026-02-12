# Práctica 1 - Desarrollo de Videojuegos

**Autor:** Álvaro Morazo Mosquera 
**Curso:** 4º Ingeniería Informática  
**Profesora:** Elisa Todd

## 1. Descripción del Proyecto
Esta práctica consiste en la creación de un entorno 3D en Unity donde se han implementado mecánicas básicas de movimiento, interacción con el entorno y gestión de plataformas. Se ha utilizado el **Universal Render Pipeline (URP)** para la gestión visual.

## 2. Contenidos y Mecánicas Implementadas
En este proyecto se han desarrollado y configurado los siguientes elementos técnicos:

* **Control del Jugador:** Sistema de control basado en `PlayerController.cs` para gestionar el movimiento y las acciones del personaje.
* **Plataformas Móviles:** Uso de `MovingPlatformClase.cs` para crear plataformas con movimiento cíclico en la escena.
* **Plataformas Que Caen:** Implementación de `FallingPlatform.cs`, que añade un desafío al jugador al interactuar con suelos inestables.
* **Gestión de Assets:** Organización de modelos 3D (personajes, vegetación y estructuras) y uso de sistemas de partículas para efectos visuales (Orbs Effects).

## 3. Estructura del Repositorio
El proyecto sigue una estructura limpia para facilitar su revisión:
* **Assets/Scripts:** Lógica de programación en C#.
* **Assets/Prefabs:** Objetos preconfigurados (Jugador, plataformas, props).
* **Assets/Models:** Modelos 3D y texturas.
* **Assets/Scenes:** Escena principal del proyecto (`SampleScene`).
