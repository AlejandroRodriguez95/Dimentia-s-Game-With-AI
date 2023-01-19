# Dimentia-s-Game-With-AI
Games programming final project (still on the making). Uses model-view-controller pattern, AI based on stockfish (from chess)

Board game, based on a game that I used to play with a few friends on Tibia (a MMORPG)

Rules:
  -4x6 board (24 slots)
  -3 different types of pieces:
      -> Player piece
      -> "Pillow"
      -> "Tower"
  -3 different types of slots:
      -> Teleport:
            -2 in opposite corners of the board
            -Only player pieces can be moved over it
            -If a player piece is moved to one, the player piece position is changed to the opposite teleport
            
      -> Normal slot:
            -Any piece can be moved over it, max 4 in total (3 pillows and a tower/player)
            -20 on the board
            
      -> Star slot:
            -same as a normal slot, but it's the starting point of a player, and the goal of the opponent
            -game ends if a player reaches the opponent's star

Turn system:
    a turn consists of:
      -> player A turn starts:
          -he moves his player piece to a new available slot in a radius of 1 around his current position
          -once he moves, the game checks if the opponent is trapped or player reached the star
          -if not, player A moves a piece (tower, pillow) in a radius of 2 around himself (based on tibia mechanics)
          -game checks again if win conditions are met
          -player B turn starts...


Win conditions:
    1. Trapping your opponent (making the opponent unable to move)
    2. reaching the opponent's star slot
    3. Not being able to move a piece after moving the player piece (loss)


Model contains all the logic of what happens on the board
View are all the assets and scripts that interact with the assets
Controller is used by the player (and AI) to interact with the model and update the view
Ideally, thanks to this model, the game would be expandable and a online feature could be added easily.


How AI works:

-> Similar to stockfish: 
          - Scans legal moves in a position
          - Assigns a value to each move, based on different factors
              Factors:
                -> hot area (own or opponent pieces are close to the winning slot)
                -> possible legal moves for self or opponent
                -> defensive/ofensive moves
                
               
