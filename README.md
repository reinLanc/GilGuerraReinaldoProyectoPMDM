# Pinkman

Juego de plataformas 2D hecho con Unity 6 (6000.3 LTS) y C#. Controlas a Pinkman, un personaje que corre, salta y puede hacer doble salto, a través de tres niveles llenos de plataformas fijas y móviles, trampas y piñas coleccionables. El objetivo es llegar a la bandera de meta de cada nivel sin quedarte sin vidas.

El juego combina precisión en el salto con gestión de riesgo: las caídas largas hacen daño según la altura, las diversas trampas obligan a calcular el momento de pasar, y las piñas devuelven vidas a quien sepa dónde buscarlas. Las vidas se conservan de un nivel a otro, así que cada error cuenta hasta el final.

El proyecto incluye animaciones por estados, música y efectos de sonido, cámara con límites por nivel, niveles construidos con Tilemaps y pantallas de inicio, Game Over y final.

> **Autor:** Reinaldo Antonio Gil Guerra

<img width="948" height="535" alt="pantalla de inicio" src="https://github.com/user-attachments/assets/ee850b82-b60f-4d36-9352-cf2b553809da" />

---

## Índice

1. [Descripción del juego](#1-descripción-del-juego)
2. [Controles](#2-controles)
3. [Mecánicas](#3-mecánicas)
4. [Cómo se ganan y se pierden vidas](#4-cómo-se-ganan-y-se-pierden-vidas)
5. [Elementos del juego](#5-elementos-del-juego)
6. [Niveles](#6-niveles)
7. [Pantallas de inicio, Game Over y final](#7-pantallas-de-inicio-game-over-y-final)
8. [Estructura del proyecto](#8-estructura-del-proyecto)
9. [Cómo ejecutarlo](#9-cómo-ejecutarlo)

---

## 1. Descripción del juego

Pinkman empieza cada partida con **3 vidas** y debe superar **3 niveles**. En cada nivel hay que subir, saltar entre plataformas fijas y móviles, esquivar trampas y llegar a la **bandera de meta**. Por el camino hay **piñas** que devuelven vidas. Si pierdes todas las vidas, aparece la pantalla de **Game Over**; si superas el nivel 3, llegas a la pantalla **final**.

Rasgos propios del juego:

- No todas las caídas son iguales: cuanto más alto caes, más vidas pierdes.
- El doble salto solo se puede usar cuando el personaje ya no está subiendo.
- Las vidas **se conservan** al pasar de un nivel al siguiente.

## 2. Controles

| Acción | Tecla |
|---|---|
| Moverse a izquierda / derecha | Flechas de dirección ← → |
| Saltar | Barra espaciadora |
| Doble salto | Barra espaciadora otra vez, en el aire (ver [mecánicas](#31-movimiento-y-salto)) |
| Salir del juego | Tecla `Esc` |

Al empezar el nivel 1 aparece un texto con las instrucciones que se desvanece a los pocos segundos.

## 3. Mecánicas

### 3.1 Movimiento y salto

- Pinkman corre a velocidad constante y puede **saltar dos veces** antes de volver a tocar el suelo.
- El segundo salto solo se acepta **cuando el personaje deja de subir** (en lo más alto del salto o ya cayendo). Si pulsas el salto mientras sube, no se ejecuta.
- Si caes de un borde sin saltar, cuenta como haber gastado el primer salto: solo queda uno en el aire.
- Las **plataformas móviles arrastran** al jugador, tanto en horizontal como en vertical.

### 3.2 Daño por caídas

La altura de la caída se mide desde el punto más alto alcanzado hasta el suelo. Los valores se pueden cambiar en el Inspector de Unity (componente `Jugador Salud`); estos son los de la versión entregada:

| Caída (unidades de Unity) | Efecto |
|---|---|
| Menos de 10.5 | Sin daño |
| De 10.5 a 17.5 | Pierdes **1 vida** |
| De 17.5 a 27 | Pierdes **2 vidas** (máximo por caída) |
| 27 o más | **Muerte** instantánea |

### 3.3 Invulnerabilidad y retroceso

- Tras recibir daño, Pinkman es **invulnerable durante 1 segundo** y parpadea para indicarlo.
- Al tocar el **fuego** o la **bola de pinchos** el personaje **rebota hacia atrás** (fuerza 6 en horizontal y 7 en vertical) y pierde el control durante **0,3 s**.
- El **aplastador** no hace retroceder sino que mata directamente al detectar la colisión (componente `Peligro` con *Muerte instantánea* marcada).

### 3.4 Muerte

Se muere al quedarte sin vidas, con una caída mortal o al tocar una trampa de muerte instantánea (el aplastador). Se reproduce la animación de morir, luego el efecto de desaparecer y se carga la pantalla de Game Over.

### 3.5 Meta y paso de nivel

Al tocar la **bandera** el personaje desaparece con un efecto, suena un sonido y se carga el siguiente nivel. Las vidas que tenías se mantienen. Tras el nivel 3 se carga la pantalla final.

### 3.6 Cámara y sonido

- La cámara sigue al jugador y nunca muestra nada fuera de los límites de cada nivel.
- Hay **música de fondo** continua entre escenas y efectos de sonido para salto, recoger piña, recibir daño, morir y llegar a la meta.

## 4. Cómo se ganan y se pierden vidas

**Se ganan:**

| Cómo | Detalle |
|---|---|
| Recoger una **piña** | +1 vida y se reproduce la animación de recogida |
| Límite | Máximo **10 vidas**. Con el máximo, la piña no se recoge y se queda en su sitio |

**Se pierden:**

| Cómo | Detalle |
|---|---|
| Caída larga | 1 o 2 vidas según la altura (ver [3.2](#32-daño-por-caídas)) |
| Tocar el **fuego** | **1 vida** y retroceso haciendo daño solo cuando está encendido |
| Tocar la **bola de pinchos** | **1 vida** y retroceso |
| Tocar el **aplastador** | **Muerte instantánea**: se pierden todas las vidas |
| Caída mortal | Se pierden todas las vidas |

Las vidas se muestran en la esquina superior izquierda, junto al icono de la piña.

<img width="176" height="83" alt="vidas" src="https://github.com/user-attachments/assets/df6a5448-7ae5-47de-a355-446cd3249227" />


## 5. Elementos del juego

| Elemento | Descripción |
|---|---|
| **Pinkman** | Personaje del jugador, con animaciones de parado, correr, saltar, doble salto, caer, morir y desaparecer |
| **Plataformas fijas** | Plataformas estáticas para subir por el nivel |
| **Plataformas móviles** | Se desplazan en horizontal o en vertical y llevan al jugador encima |
| **Terreno** | Muros y suelo hechos con Tilemaps |
| **Piña** | Objeto coleccionable animado que da una vida |
| **Fuego** | Trampa que alterna: apagado unos 2 s durante este tiempo se puede pisar como una plataforma más, un breve aviso de 0,4 s y encendido 2 s el cual hace daño al contacto. |
| **Bola de pinchos** | Trampa que sube y baja entre dos alturas, hay que saltarla o pasar por debajo |
| **Aplastador** | Trampa que sube despacio y **cae muy rápido**, ejecutando su Animator al caer y **mata al contacto** |
| **Bandera de meta** | Termina el nivel |

<img width="948" height="535" alt="plataformas" src="https://github.com/user-attachments/assets/17e5cdcc-61c0-4c04-9405-51b7d901bef1" />

<img width="943" height="540" alt="trampas" src="https://github.com/user-attachments/assets/72ccc626-82bb-42c7-8440-dcfef569e346" />

<img width="943" height="529" alt="pinas" src="https://github.com/user-attachments/assets/7ff11f2f-20ae-4eec-ad9c-9fc5f0c9ed92" />


## 6. Niveles

### Nivel 1
Primer nivel: plataformas sencillas, primera piña y el texto con los controles. Nivel totalmente en vertical.

<img src="https://github.com/user-attachments/assets/47a9673f-7c1d-491c-bb52-dcb64138d8a2" alt="Nivel 1" width="640" />

### Nivel 2
Más complejo que el anterior. Incluye tramos de recorrido horizontal y presenta una nueva trampa aplastadora que produce muerte instantánea, agotando todas las vidas.

<img src="https://github.com/user-attachments/assets/5ccc8d03-b6e4-495f-b78e-b6facb71e452" alt="Nivel 2" width="640" />

### Nivel 3
Incluye mayor recorrido horizontal y menos caídas entre plataformas, pero muchas más trampas en poco espacio.

<img src="https://github.com/user-attachments/assets/d39a225c-0a65-41d0-943e-5a30057d8327" alt="Nivel 3" width="640" />

## 7. Pantallas de inicio, Game Over y final

| Pantalla | Qué ocurre |
|---|---|
| **Inicio** (`MenuInicio`) | Imagen del juego y botón **JUGAR**, que empieza la partida en el nivel 1 |
| **Game Over** (`GameOver`) | Se muestra al morir, presenta al usuario el botón **Reiniciar juego**, que vuelve al nivel 1 con 3 vidas |
| **Final** (`FinJuego`) | Se muestra al superar el nivel 3, presenta al usuario los botones **Volver a jugar** y **Salir** |

<img width="939" height="525" alt="inicio" src="https://github.com/user-attachments/assets/df2759f4-65fe-4509-aa77-f17ad0e5eb9d" />

<img width="947" height="533" alt="gameover" src="https://github.com/user-attachments/assets/5b1e350b-dc45-450a-aeb9-3bb585a20233" />

<img width="953" height="534" alt="juego completado" src="https://github.com/user-attachments/assets/12f6e564-0f7e-425d-bdad-8f4eb2c97952" />


## 8. Estructura del proyecto

### Escenas (orden en Build Profiles)

| Índice | Escena |
|---|---|
| 0 | `MenuInicio` |
| 1 | `Nivel1` |
| 2 | `Nivel2` |
| 3 | `Nivel3` |
| 4 | `GameOver` |
| 5 | `FinJuego` |

### Scripts principales (`Assets/Scripts`)

| Script | Función |
|---|---|
| `JugadorController` | Movimiento, salto, detección de suelo, animaciones y retroceso |
| `EstadoJugador` | Enum con los estados de animación del jugador |
| `JugadorSalud` | Vidas, daño por caída, invulnerabilidad con parpadeo y muerte |
| `CamaraSeguimiento` | Cámara que sigue al jugador con límites por nivel |
| `MovimientoPlataforma` | Plataformas móviles (horizontal o vertical) |
| `Peligro` | Trampas que dañan o matan al jugador al tocarlas |
| `TrampaVertical` | Trampa que sube y baja entre dos alturas, con velocidad de subida y de bajada independientes y pausas (bola de pinchos y aplastador) |
| `RotacionTrampa` | Hace girar la bola de pinchos sobre sí misma a velocidad constante |
| `AplastadorAnimacion` | Cambia la animación de la trampa Aplastador según su estado |
| `PinaVida` | Piña que devuelve una vida |
| `Meta` | Bandera que termina el nivel y guarda las vidas |
| `DatosPartida` | Guarda las vidas entre niveles |
| `HUDVidas` | Muestra las vidas en pantalla |
| `TextoTemporal` | Texto de controles que se desvanece |
| `GestorAudio` | Música de fondo y efectos de sonido |
| `GestorEscenas` | Botones de jugar, reiniciar y salir |
| `SalidaJuego` | Cierra el juego con la tecla `Esc` |

## 9. Cómo ejecutarlo

**Desde el editor de Unity**

1. Abre el proyecto con **Unity 6.3 LTS (6000.3.22f1)** desde Unity Hub.
2. Abre la escena `Assets/Scenes/MenuInicio`.
3. Pulsa **Play** y haz clic en **JUGAR**. Con `Esc` se detiene la ejecución.

**Versión ejecutable**

1. Para Windows directorio `juego ejecutable` contiene archivo `.exe`.
2. Ejecuta el `.exe`. Con `Esc` o el botón **Salir** se cierra el juego.

