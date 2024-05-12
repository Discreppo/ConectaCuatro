namespace ConnectFour;

public class GameState
{

	static GameState()
	{
		CalculateWinningPlaces();
	}

    /// <summary>
    /// Indica si un jugador ha ganado, el juego está empatado o el juego está en curso
    /// </summary>
    public enum WinState
	{
		No_Winner = 0,
		Player1_Wins = 1,
		Player2_Wins = 2,
		Tie = 3
	}

    /// <summary>
    /// El jugador que tiene el turno. Por defecto, el jugador 1 empieza primero.
    /// </summary>
    public int PlayerTurn => TheBoard.Count(x => x != 0) % 2 + 1;

    /// <summary>
    /// Número de turnos completados y piezas jugadas hasta ahora en el juego
    /// </summary>
    public int CurrentTurn { get { return TheBoard.Count(x => x != 0); } }

	public static readonly List<int[]> WinningPlaces = new();

	public static void CalculateWinningPlaces()
	{

        // Filas horizontales
        for (byte row = 0; row < 6; row++)
		{

			byte rowCol1 = (byte)(row * 7);
			byte rowColEnd = (byte)((row + 1) * 7 - 1);
			byte checkCol = rowCol1;
			while (checkCol <= rowColEnd - 3)
			{
				WinningPlaces.Add(new int[] {
					checkCol,
					(byte)(checkCol + 1),
					(byte)(checkCol + 2),
					(byte)(checkCol + 3)
					});
				checkCol++;
			}

		}

        // Columnas verticales
        for (byte col = 0; col < 7; col++)
		{

			byte colRow1 = col;
			byte colRowEnd = (byte)(35 + col);
			byte checkRow = colRow1;
			while (checkRow <= 14 + col)
			{
				WinningPlaces.Add(new int[] {
					checkRow,
					(byte)(checkRow + 7),
					(byte)(checkRow + 14),
					(byte)(checkRow + 21)
					});
				checkRow += 7;
			}

		}

        // Diagonal hacia adelante "/"
        for (byte col = 0; col < 4; col++)
		{

            // La columna de inicio debe ser 0-3
            byte colRow1 = (byte)(21 + col);
			byte colRowEnd = (byte)(35 + col);
			byte checkPos = colRow1;
			while (checkPos <= colRowEnd)
			{
				WinningPlaces.Add(new int[] {
					checkPos,
					(byte)(checkPos - 6),
					(byte)(checkPos - 12),
					(byte)(checkPos - 18)
					});
				checkPos += 7;
			}

		}

        // Diagonal inversa "\"
        for (byte col = 0; col < 4; col++)
		{

            // La columna de inicio debe ser 0-3
            byte colRow1 = (byte)(0 + col);
			byte colRowEnd = (byte)(14 + col);
			byte checkPos = colRow1;
			while (checkPos <= colRowEnd)
			{
				WinningPlaces.Add(new int[] {
					checkPos,
					(byte)(checkPos + 8),
					(byte)(checkPos + 16),
					(byte)(checkPos + 24)
					});
				checkPos += 7;
			}

		}


	}

    /// <summary>
    /// Comprobar el estado del tablero para un escenario ganador
    /// </summary>
    /// <returns>0 - sin ganador, 1 - jugador 1 gana, 2 - jugador 2 gana, 3 - empate</returns>
    public WinState CheckForWin()
	{

        // Salir inmediatamente si se han jugado menos de 7 piezas
        if (TheBoard.Count(x => x != 0) < 7) return WinState.No_Winner;

		foreach (var scenario in WinningPlaces)
		{

			if (TheBoard[scenario[0]] == 0) continue;

			if (TheBoard[scenario[0]] ==
				TheBoard[scenario[1]] &&
				TheBoard[scenario[1]] ==
				TheBoard[scenario[2]] &&
				TheBoard[scenario[2]] ==
				TheBoard[scenario[3]]) return (WinState)TheBoard[scenario[0]];

		}

		if (TheBoard.Count(x => x != 0) == 42) return WinState.Tie;

		return WinState.No_Winner;

	}

    /// <summary>
    /// Toma el turno actual y coloca una pieza en la columna indexada en 0 solicitada
    /// </summary>
    /// <param name="column">Columna indexada en 0 donde colocar la pieza</param>
    /// <returns>El índice final del array donde reside la pieza</returns>
    public byte PlayPiece(int column)
	{

        // Comprobar si hay una victoria actual
        if (CheckForWin() != 0) throw new ArgumentException("El juego ha finalizado");

        // Comprobar la columna
        if (TheBoard[column] != 0) throw new ArgumentException("La columna esta llena");

        // Colocar la pieza
        var landingSpot = column;
		for (var i = column; i < 42; i += 7)
		{
			if (TheBoard[landingSpot + 7] != 0) break;
			landingSpot = i;
		}

		TheBoard[landingSpot] = PlayerTurn;

		return ConvertLandingSpotToRow(landingSpot);

	}

	public List<int> TheBoard { get; private set; } = new List<int>(new int[42]);

	public void ResetBoard()
	{
		TheBoard = new List<int>(new int[42]);
	}

	private byte ConvertLandingSpotToRow(int landingSpot)
	{

		return (byte)(Math.Floor(landingSpot / (decimal)7) + 1);

	}

}