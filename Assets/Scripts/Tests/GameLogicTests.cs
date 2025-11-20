using NUnit.Framework;

public class GameLogicTests
{
    // Una prueba simple que verifica la inicialización correcta.
    [Test]
    public void GameLogic_Initialization_SetsObjectivesCorrectly()
    {
        // ARRANGE: Preparamos el escenario de la prueba
        GameLogic gameLogic;

        // ACT: Ejecutamos la acción que queremos probar (en este caso, el constructor)
        gameLogic = new GameLogic(5); // Asumiendo que GameLogic tiene un constructor que recibe el número de objetivos

        // ASSERT: Verificamos que el resultado es el esperado.
        Assert.AreEqual(5, gameLogic.ObjectivesToWin);
        Assert.AreEqual(0, gameLogic.ObjectivesCompleted);
        Assert.IsFalse(gameLogic.IsVictoryConditionMet);
    }
}