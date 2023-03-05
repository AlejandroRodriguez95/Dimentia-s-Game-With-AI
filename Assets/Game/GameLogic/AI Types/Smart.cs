using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Smart : AI
{
    private List<AIMoves> possibleMovesThisStage; // all the possible moves that need to be checked by the AI
    // first tuple is player move, second tuple is piece move

    Dictionary<(int,int), int> enemyHotArea;
    Dictionary<(int,int), int> targetHotArea;

    int randomFactor;

    private int defaultBehavior; // 0 attack hot area, 1 defend hot area, 2 attack player, 3 defend self from trap

    Dictionary<int, string> debuggingPriorityDictionary;

    (int, int) playerOriginalPosThisTurn;
    Player enemy;

    private int amountOfLegalMovesForAI;
    private int amountOfLegalMovesForOpponent;

    private int priorityThisTurn; // 1 = attack enemy hot area, 2 = defend own hot area, 3 = reduce enemy possible moves, 4 = increase own possible moves
    private AIMoves bestMoveThisTurn;

    public Player Enemy
    {
        set { enemy = value; }
    }
    public int DefaultBehavior
    {
        set { defaultBehavior = value; }
    }

    public Smart(Board boardReference, Player player) : base(boardReference, player)
    {

        this.boardReference = boardReference;
        this.player = player;
        this.legalMoves = new List<(int, int)>();

        amountOfLegalMovesForAI = 0;
        amountOfLegalMovesForOpponent = 0;
        playerOriginalPosThisTurn = (3,5);
        possibleMovesThisStage = new List<AIMoves>();
        randomFactor = GlobalData.randomFactor;

        InitializeHotAreaMap();

        debuggingPriorityDictionary = new Dictionary<int, string>();

        debuggingPriorityDictionary.Add(0, "Attack enemy hot area");
        debuggingPriorityDictionary.Add(1, "Defend own hot area");
        debuggingPriorityDictionary.Add(2, "try to trap opponent");
        debuggingPriorityDictionary.Add(3, "defend self from trap");
    }




    public override bool GenerateMove(ref (int, int) AImove, E_TurnStages turnstage) // should generate a move for the Play() method
    {
    
        switch (turnstage)
        {
            case E_TurnStages.TurnStart:
                return true;

            case E_TurnStages.MovePlayer:
                IterateThroughAllPossibleMoves();
                ChooseMove();
                AImove = bestMoveThisTurn.playerMove;
                return true;

            case E_TurnStages.SelectPiece:
                AImove = bestMoveThisTurn.selectedPiece;
                return true;

            case E_TurnStages.MovePiece:
                AImove = bestMoveThisTurn.selectedPieceMove;
                return true;

            default:
                Debug.Log("Warning! this shouldn't happen");
                break;
        }

        return true;
    }

    private int[] AnalyseMove(int towerOutOfTheWay, int towerBlockingEnemy, int playerBlockingEnemy) // returns the analysis of a potential move
    {
        int[] arr = new int[4];

        int legalMovesForEnemyOnTempPosition = boardReference.AmountOfLegalMovesForPlayer(enemy);
        int legalMovesForSelfOnTempPosition = boardReference.AmountOfLegalMovesForPlayer(player);
        int opponentPosInHotArea = enemyHotArea[enemy.PlayerPosOnBoard];
        int selfPosInHotArea = targetHotArea[player.PlayerPosOnBoard];
        int distanceBetweenPlayers = Mathf.Abs(player.PlayerPosOnBoard.Item2 - enemy.PlayerPosOnBoard.Item2) + Mathf.Abs(player.PlayerPosOnBoard.Item1 - enemy.PlayerPosOnBoard.Item1);

        // 0 = attack hot area
        // 1 = defend hot area
        // 2 try to trap enemy
        // 3 avoid trap



        arr[0] = (
            (opponentPosInHotArea - selfPosInHotArea) * opponentPosInHotArea
            - legalMovesForSelfOnTempPosition
            //+ legalMovesForEnemyOnTempPosition / 2
            + towerOutOfTheWay
            + Random.Range(-randomFactor, randomFactor)
            //+ towerBlockingEnemy / 2
            );

        arr[1] = (
            (selfPosInHotArea - opponentPosInHotArea) * selfPosInHotArea
            - distanceBetweenPlayers
            + legalMovesForEnemyOnTempPosition
            - towerBlockingEnemy
            + Random.Range(-randomFactor, randomFactor)

            //- towerOutOfTheWay / 4
            );

        arr[2] = (
            - legalMovesForEnemyOnTempPosition
            - distanceBetweenPlayers
            );

        arr[3] = (
            legalMovesForSelfOnTempPosition * 2
            + Random.Range(-randomFactor, randomFactor)
            + towerOutOfTheWay
            );






        //Debug.Log($"Attack hot area: {arr[0]}. Defend hot area: {arr[1]}. Enemy possible moves: {arr[2]}. Own possible moves: {arr[3]}.");

        return arr;
    }

    private void ChooseMove()
    {
        int highestValueSoFar = -1000;
        int highestSumSoFar = -1000;
        AIMoves bestMove = new AIMoves();

        foreach (AIMoves move in possibleMovesThisStage)
        {
            int value = move.analysis[priorityThisTurn];
            int sum = move.analysis[0] + move.analysis[1] + move.analysis[2] + move.analysis[3];

            if (priorityThisTurn == 2)
                sum = move.analysis[2];

            //Debug.Log($"Player moves to: {move.playerMove}, Moved piece at: {move.selectedPiece} to {move.selectedPieceMove}. Sum: {sum}");

            if (value > highestValueSoFar)
            {
                highestValueSoFar = value;
                highestSumSoFar = sum;
                bestMove = move;
            }
            else if (value == highestValueSoFar && sum > highestSumSoFar)
            {
                highestSumSoFar = sum;
                bestMove = move;
            }
        }

        if (highestValueSoFar == -1000)
        {
            int random = Random.Range(0, possibleMovesThisStage.Count - 1);
            bestMove = possibleMovesThisStage[random];
        }

        bestMoveThisTurn = bestMove;
    }

    private void ChoosePriorityThisTurn()
    {
        amountOfLegalMovesForAI = boardReference.legalMovesForAI.Count;
        amountOfLegalMovesForOpponent = boardReference.ReturnLegalMovesForEnemy(enemy, 1).Count;

        var playersAreWithinReach = false;
        if(Mathf.Abs(player.PlayerPosOnBoard.Item2 - enemy.PlayerPosOnBoard.Item2) <= 1 && Mathf.Abs(player.PlayerPosOnBoard.Item1 - enemy.PlayerPosOnBoard.Item1) <= 1)
        {
            playersAreWithinReach = true;
        }


        //Debug.Log($"AI hot status: {targetHotArea[player.PlayerPosOnBoard]}, enemy hot status: {enemyHotArea[enemy.PlayerPosOnBoard]}, distance between players: {playersAreWithinReach}");
        //Debug.Log($"Amount of legal moves for AI {amountOfLegalMovesForAI}, amount of legal moves for enemy: {amountOfLegalMovesForOpponent}");

        // Opponent is in a hot area with a value lower or equal than 3
        if (enemyHotArea[enemy.PlayerPosOnBoard] <= 2)
        {
            // AI is in a hot area with a value lower or equal than the opponent
            if (targetHotArea[player.PlayerPosOnBoard] <= enemyHotArea[enemy.PlayerPosOnBoard])
            {
                priorityThisTurn = 0; // try to reach enemy hot area
            }
            else
            {
                priorityThisTurn = 1; // defend own hot area
            }
        }
        // Opponent has 2 possible moves and distance to player is equal or lower to 2
        else if (amountOfLegalMovesForOpponent <= 2 && (playersAreWithinReach))
        {
            priorityThisTurn = 2; // try to trap opponent
        }
        // AI has 1 possible move
        else if (amountOfLegalMovesForAI <= 2)
        {
            priorityThisTurn = 3; // defend self from trap
        }
        // Default behavior
        else
        {
            priorityThisTurn = defaultBehavior;
        }

        if(targetHotArea[player.PlayerPosOnBoard] == 5)
        {
            priorityThisTurn = 0;
        }
    }

    private void MovePlayerOnTestBoard((int,int) moveTo)
    {
        boardReference.AISelectPlayer(player);
        boardReference.AIMovePlayer(ref player, moveTo);
    }    
    private void MovePieceOnTestBoard((int,int) moveTo, (int,int) from)
    {
        var useNextPiece = boardReference.AISelectPiece(from, true);
        if (useNextPiece)
            boardReference.AIMovePiece(moveTo, true);
        else
            boardReference.AIMovePiece(moveTo, false);
    }



    private void IterateThroughAllPossibleMoves()
    {
        List<(int, int)> piecesInRangePosition = new List<(int, int)>();
        List<(int, int)> potentialTargetPositions = new List<(int, int)>();
        List<(int, int)> legalMovesForPlayer = boardReference.ReturnLegalMovesForPlayer(player, 1);

        amountOfLegalMovesForAI = legalMovesForPlayer.Count;

        ChoosePriorityThisTurn();
        Debug.Log($"Prioriy this turn: {debuggingPriorityDictionary[priorityThisTurn]}");

        int iteratorI = legalMovesForPlayer.Count, iteratorJ, iteratorK;

        possibleMovesThisStage.Clear();

        for (int i=0; i < iteratorI; i++)
        {
            int playerBlockingEnemyValue;

            MovePlayerOnTestBoard(legalMovesForPlayer[i]);
            piecesInRangePosition = boardReference.ReturnPiecesInRangeOfPlayer(player);
            playerBlockingEnemyValue = enemyHotArea[player.PlayerPosOnBoard];


            iteratorJ = piecesInRangePosition.Count;

 
            if (iteratorJ > 0)
            {
                for (int j=0; j < iteratorJ; j++)
                {
                    boardReference.SelectPiece(piecesInRangePosition[j], true);
                    potentialTargetPositions = boardReference.ReturnLegalMovesForPieces(player, 2);
                    iteratorK = potentialTargetPositions.Count;
                    int towerIntoHotAreaValue = 0;
                    int towerOutOfTheWayValue = 0;
                    int towerDefendingOwnHotArea = 0;

                    for (int k = 0; k < iteratorK; k++)
                    {
                        if(piecesInRangePosition[j] != potentialTargetPositions[k])
                        {
                            if(boardReference.selectedPiece.PieceType == E_PieceType.Tower)
                            {
                                towerIntoHotAreaValue = enemyHotArea[potentialTargetPositions[k]] - enemyHotArea[piecesInRangePosition[j]];

                                towerOutOfTheWayValue = targetHotArea[potentialTargetPositions[k]] - targetHotArea[piecesInRangePosition[j]];
                            }
                            

                            MovePieceOnTestBoard(potentialTargetPositions[k], piecesInRangePosition[j]); // make move

                            var analysis = AnalyseMove(towerOutOfTheWayValue, towerIntoHotAreaValue, playerBlockingEnemyValue);

                            AIMoves aIMoves = new AIMoves(legalMovesForPlayer[i], piecesInRangePosition[j], potentialTargetPositions[k], analysis);

                            possibleMovesThisStage.Add(aIMoves);

                            MovePieceOnTestBoard(piecesInRangePosition[j], potentialTargetPositions[k]); // undo move
                        }
                    }
                }
            }

            MovePlayerOnTestBoard(playerOriginalPosThisTurn); // revert player move
        }
    }

    private void ShowAllPossibleMovesForAI()
    {
        foreach (var x in possibleMovesThisStage)
        {
            Debug.Log($"Player can move to: {x.playerMove} - and move the piece: {x.selectedPiece} - to: {x.selectedPieceMove}");
        }
    }
    private void InitializeHotAreaMap()
    {
 
        //  4 3 2 1
        //  4 3 2 2
        //  4 3 3 3
        //  4 4 4 4
        //  5 5 5 5
        //  6 5 5 6


        enemyHotArea = new Dictionary<(int, int), int>();

        enemyHotArea.Add((0, 0), 6);
        enemyHotArea.Add((0, 3), 6);

        enemyHotArea.Add((0, 1), 5);
        enemyHotArea.Add((1, 0), 5);
        enemyHotArea.Add((1, 1), 5);
        enemyHotArea.Add((2, 0), 5);
        enemyHotArea.Add((2, 1), 5);
        enemyHotArea.Add((3, 1), 5);

        enemyHotArea.Add((0, 2), 4);
        enemyHotArea.Add((1, 2), 4);
        enemyHotArea.Add((2, 2), 4);
        enemyHotArea.Add((3, 2), 4);
        enemyHotArea.Add((0, 4), 4);
        enemyHotArea.Add((0, 5), 4);
        enemyHotArea.Add((3, 0), 4);

        enemyHotArea.Add((1, 3), 3);
        enemyHotArea.Add((1, 4), 3);
        enemyHotArea.Add((1, 5), 3);
        enemyHotArea.Add((2, 3), 3);
        enemyHotArea.Add((3, 3), 3);

        enemyHotArea.Add((2, 4), 2);
        enemyHotArea.Add((2, 5), 2);
        enemyHotArea.Add((3, 4), 2);

        enemyHotArea.Add((3, 5), 1);




        //  6 5 5 6
        //  5 5 5 5
        //  4 4 4 4
        //  3 3 3 4
        //  2 2 3 4
        //  1 2 3 2

        targetHotArea = new Dictionary<(int, int), int>();

        targetHotArea.Add((0, 0), 1);

        targetHotArea.Add((0, 1), 2);
        targetHotArea.Add((1, 0), 2);
        targetHotArea.Add((1, 1), 2);

        targetHotArea.Add((2, 0), 3);
        targetHotArea.Add((2, 1), 3);
        targetHotArea.Add((2, 2), 3);
        targetHotArea.Add((0, 2), 3);
        targetHotArea.Add((1, 2), 3);

        targetHotArea.Add((0, 3), 4);
        targetHotArea.Add((1, 3), 4);
        targetHotArea.Add((2, 3), 4);
        targetHotArea.Add((3, 3), 4);
        targetHotArea.Add((3, 2), 4);
        targetHotArea.Add((3, 1), 4);
        targetHotArea.Add((3, 0), 4);

        targetHotArea.Add((0, 4), 5);
        targetHotArea.Add((1, 4), 5);
        targetHotArea.Add((1, 5), 5);
        targetHotArea.Add((2, 4), 5);
        targetHotArea.Add((2, 5), 5);
        targetHotArea.Add((3, 4), 5);


        targetHotArea.Add((0, 5), 6);
        targetHotArea.Add((3, 5), 6);
    }
}
