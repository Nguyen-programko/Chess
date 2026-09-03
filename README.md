# Chess

Chess for two players on one device, or against a built-in engine

![Screenshot](https://files.xuan-tiep.com/chess.png)
![Screenshot](https://files.xuan-tiep.com/chess-2.png)

**Stack:** C# · WinForms · .NET 10

## Running application
Requires the .NET 10 SDK.

Run
```bash
#run this in terminal in root of the project.
dotnet build
dotnet run
```
## Notes
**Engine**  
The main feature of this chess application is the engine that computes next moves of the computer opponent. It's implemented with [Minimax Algorithm](https://chessprogramming.org/Minimax) and [Alpha-Beta pruning](https://chessprogramming.org/Alpha-Beta). For better scoring there are look up tables for position score for each chess piece at certain points of the game. And lastly to minimize repeated calculations positions already calculated are saved to [Transposition tables](https://chessprogramming.org/Transposition_Table).

**Other gameplay features** 
- Game length timer/stopwatch.
- Move history and show board state at that moment.
- saving and loading games from a file.


