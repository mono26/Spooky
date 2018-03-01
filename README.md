# Spooky
Sppoky alpha v2. Refactor code. More organized folders and dependencies.
# Iteration cycle
Spooky, enemy, ui, repeat.

//Spooky
	//Se puede mover
		//Mueve en la direccion deseada
		//Limita la posicion a los valores maximos
		//Cambia de velocidad segun el caso
	//Puede atacar a los enemigos
	//Deteca enemigos cercanos, autoaim
		//Alamacena en una fila los enemigos dentro del rango
		//Saca de la fila los objetos que salen del rango
		//Mira el estado del ultimo de la fila
	//Detecta lugares para plantar cercanos
	//Puede plantar
	//Puede mejorar las plantas

//Enemigos
	//Enemigo1 (Ladron)
		//Se mueve desde donde nace hasta el objetivo y luego al contrariov (movimiento)
		//Roba vida al jugador cuando esta en la posicion adecuada (habilidad)
	//Enemigo2 (Atacante)
		//Se mueve en direccion del jugador para atacarlo (movimiento)
		//Detecta cuando el juegador es atacado
		//Ataca al jugador (habilidad)

//Plantas
	//Planta1 (Tomate)
		//Ataca a rango (ataque)
		//Ataca a rango con municion especial (nivel 2)
		//Recibe daño
		//Detecta enemigos en rango (Trigger o SphereCast)
		//Sube de nivel
