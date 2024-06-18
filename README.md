# Minerva

Minerva is my take on a chess engine. I have no idea how well it will play, but I'm pretty
sure it will be able to beat me :-<>

## The board
### Bitboard representation

8 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒  
7 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓  
6 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒  
5 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓  
4 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒  
3 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓  
2 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒  
1 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓  
  a b c d e f g h  

The board is represented by a bitboard where the least signinficant bit (LSB) corresponds to H1 and the most
significant bit corresponds to A8, with A1 being bit 7, h2 bit 8, and so on through H8 which would be bit 56.
In this way, I can easily figure out the representation in the board of any HEX or binary number.

### Compass Rose


  noWe         nort         noEa  
          +9    +8    +9  
              \  |  /  
  west    +1 <-  0 -> -1    east  
              /  |  \  
          -7    -8    -9  
  soWe         sout         soEa  

