# CIS 5680 Multiplayer AR Game: Starpicker

## Created by Jackie, Rachel, and Nick

### Rules

But first...
RULES ARE MADE TO BE BROKEN!

I jest.

1. Tap on stars to collect them. The first person to obtain 10 points wins!
2. Star types:
   
	- Gold star: regular star (+1 point)
	- Red star: negative penalty (-5 points)
	- Green star: positive power up (+5 points)
 	- Glowing yellow star: the Golden Snitch! (+150 points, instantly wins the game.)

### Win/Loss Conditions

Upon the first player achieving 10 points, the game will end for all current players. The winner (the first person who achieves 10 points) will receive a victory screen, while the rest of the players will receive a game over screen. After each round, players can choose to restart the game, or return to the Main Menu.

### New Prefabs

We created several new prefabs for this game:

1. EndGame: handles displaying victory and game over logic.
2. TreasureManager: handles spawning in of different types of stars.
3. Plant: should be the regular star, we changed concepts halfway through so the plant should actually be a star. Gives one point when collected.
4. PositivePowerUp: small green star that gives five points when collected.
5. NegativePowerUp: small red star that gives negative five points when collected.
6. Snitch: bright over powered golden star that gives the player who picks it up an instant win. Just like in Harry Potter.

### Goals of the Game

Main goal: collect 10 points worth of stars before your fellow players!

Additional goal: collect the Snitch to instantly win the game!

### Resources

Positive power up: Gives player 5 points upon collecting. Spawns once for every ten normal stars to spawn.

Negative power up: Gives player -5 points upon collecting. Spawns once for every ten normal stars to spawn.

Golden Snitch: Instantly wins the game by giving the player 150 points. Just like in Harry Potter. Spawns once in a blue moon.
