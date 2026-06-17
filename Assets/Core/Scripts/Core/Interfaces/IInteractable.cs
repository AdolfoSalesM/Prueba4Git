/// <summary>
/// IInteractable:
/// Interfaz base para cualquier objeto del juego con el que el jugador pueda interactuar.
/// </summary>
/// <remarks>
/// Forma parte del sistema de interacción genérico.
/// Permite que el PlayerInteraction no dependa de clases concretas
/// como Chest, Stone, NPCs, etc.
/// </remarks>
public interface IInteractable
{
    /// <summary>
    /// Método que se ejecuta cuando el jugador interactúa con el objeto.
    /// Cada objeto define su propio comportamiento.
    /// </summary>
    void Interact();
}