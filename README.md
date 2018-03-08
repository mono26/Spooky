# Spooky
Sppoky alpha v2. Refactor code. More organized folders and dependencies.
# Iteration cycle #1
Spooky, enemy, ui, repeat.
# Iteration cycle #2
Spooky, enemies, ui, plants, level, repeat.

## Spooky
	* Se puede mover (IMovable)???
		* Mueve en la direccion deseada (RigidBody Move)
		* Limita la posicion a los valores maximos
		* Cambia de velocidad segun el caso
	* Puede atacar a los enemigos
		* Informacion de la mano, la rota en direccion del movimiento o direccion del autoEnemigo
		* Informacion de la bala, informacion de la velocidad con que se lanza la bala. (Pensar si se almacena en soky o la bala)
		* Puede cargar o disparar en rafaga.
		* Cargar aumenta el daño.
	* Deteca enemigos cercanos, autoaim (IDetect)
		* Alamacena en una fila los enemigos dentro del rango
		* Saca de la fila los objetos que salen del rango
		* Mira el estado del ultimo de la fila
	* Detecta lugares para plantar cercanos
	* Puede plantar
	* Puede mejorar las plantas

## Enemigos
	* EnemyMove tiene el metodo para mover al Enemigo, se va a usar navMesh para calcular direccion y velocidad. (IMovable)???
		* Se calcula producto punto con el transformRight y transformUp con la direccion deseada.
		* Resultado de los calculos da un input mas parecido al humano.
	* Enemy tiene la informacion del enemigo. Los ataques, la velocidad y la vida. Este se la pasa los demas componentes.
		* MonoBehaviour que ejecuta los metodos de los demas componentes.
		* Tiene informacion del target.
		* Velocidad de ataque o cooldown basico. Lo mismo el movimiento. TODOS LOS ENEMIGOS SE MUEVEN?
		* EnemyState maquina de estados para cada enemigo, con acciones, transiciones y decisiones.
			* Se comunica con el EnemyAttack y el EnemyMove para ejecutar las acciones de los estados.
		* EnemyAttack almacena los attackes: basico y especial.
			* Tambien maneja los cooldowns de cada uno de los ataques 
			* (no en update solo usando variable de realTime y una con lastTime).
	* Enemigo1 (Ladron)
		* Se mueve desde donde nace hasta el objetivo y luego al contrariov (movimiento)
		* Roba vida al jugador cuando esta en la posicion adecuada (habilidad)
	* Enemigo2 (Atacante)
		* Se mueve en direccion del jugador para atacarlo (movimiento)
		* Detecta cuando el juegador es atacado
		* Ataca al jugador (habilidad)

## Plantas
	* Planta -> tiene la informacion del target
		* Attack speed
		* Vida
		* Distancia al target
		* Mira si esta muerto.
	* Planta1 (Tomate)
		* Ataca a rango (ataque) (interface)
			* Interface de IAttack 
		* Ataca a rango con municion especial (nivel 2)
		* Recibe daño
		* Detecta enemigos en rango (IDetect)
		* Sube de nivel

## Loading screen
	* El loadingText debe hacer fade out.
	* Duracion minima.

## Game Manager
	* Preload scene.
	* Va a poder reproducir los sonidos en el juego.
	* Va a tener la informacion de la dificultad, el nivel actual y el highScore.
	* Informacion de los talentos. (Cuando se implemente).
	* Los player prefabs.
	* LoadScene y loadSceneAsync la escena objectivo.
	* Puede hacer el fade de cualquier objeto.
	* Pausa el nivel que esta en la parte de atras. (OnLoadingScreen) -> Awake
	* Inicia el nivel cuando sale de la loading screen. (OnGameStart) -> Start

## Level Manager
	* Cantidad de vida restante.
	* Pausa y reinicia el nivel.
	* Gana o pierde el nivel y se lo comunica al GameManager.

## Wave Spawner
	* Spawns waves que contiene la informacion de los enemigos en un array, lo mismo la informacion de la cantidad en un array
	* El enemigo y la cantidad deben compartir el mismo index.
	* WaveCountdown -> el tiempo empieza a correr desde que se termina de spawner el ultimo enemigo.

## ISPawnable
	* Interfaz de la puede heredar cualquier entidad que pueda ser spawneada. Ejemplo Enmigo, Balas, Effectos, etc.
	* Plantas no debido a que es limitado el Instantiate en run-time.

## PlantStore
	* Informacion del blueprint de cada planta: 
		* Planta lvl1
		* Planta lvl2
		* Costo1
		* Costo2
		* Ambos reembolsos.
	* Compra cada planta y suma el dinero.
	* Vende cada planta y resta el dinero.
	* Botones conectados a los metodos del script.