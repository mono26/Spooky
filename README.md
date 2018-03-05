# Spooky
Sppoky alpha v2. Refactor code. More organized folders and dependencies.
# Iteration cycle
Spooky, enemy, ui, repeat.

## Spooky
	* Se puede mover
		* Mueve en la direccion deseada (RigidBody Move)
		* Limita la posicion a los valores maximos
		* Cambia de velocidad segun el caso
	* Puede atacar a los enemigos
		* Informacion de la mano, la rota en direccion del movimiento o direccion del autoEnemigo
		* Informacion de la bala, informacion de la velocidad con que se lanza la bala. (Pensar si se almacena en soky o la bala)
		* Puede cargar o disparar en rafaga.
		* Cargar aumenta el daño.
	* Deteca enemigos cercanos, autoaim
		* Alamacena en una fila los enemigos dentro del rango
		* Saca de la fila los objetos que salen del rango
		* Mira el estado del ultimo de la fila
	* Detecta lugares para plantar cercanos
	* Puede plantar
	* Puede mejorar las plantas

## Enemigos
	* EnemyState maquina de estados para cada enemigo, con acciones, transiciones y decisiones.
		* Se comunica con el EnemyAttack y el EnemyMove para ejecutar las acciones de los estados.
	* EnemyAttack almacena los attackes: basico y especial.
		* Tambien maneja los cooldowns de cada uno de los ataques 
		* (no en update solo usando variable de realTime y una con lastTime).
	* EnemyMove tiene el metodo para mover al Enemigo, se va a usar navMesh para calcular direccion y velocidad.
		* Se calcula producto punto con el transformRight y transformUp con la direccion deseada.
		* Resultado de los calculos da un input mas parecido al humano.
	* Enemy tiene la informacion del enemigo. Los ataques, la velocidad y la vida. Este se la pasa los demas componentes.
		* MonoBehaviour que ejecuta los metodos de los demas componentes.
	* Enemigo1 (Ladron)
		* Se mueve desde donde nace hasta el objetivo y luego al contrariov (movimiento)
		* Roba vida al jugador cuando esta en la posicion adecuada (habilidad)
	* Enemigo2 (Atacante)
		* Se mueve en direccion del jugador para atacarlo (movimiento)
		* Detecta cuando el juegador es atacado
		* Ataca al jugador (habilidad)

## Plantas
	* Planta1 (Tomate)
		* Ataca a rango (ataque)
		* Ataca a rango con municion especial (nivel 2)
		* Recibe daño
		* Detecta enemigos en rango (Trigger o SphereCast)
		* Sube de nivel

## Loading screen
	* LoadScene y loadSceneAsync la escena objectivo
	* El loadingText debe hacer fade out.
	* Puede hacer el fade de cualquier objeto.
