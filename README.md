# Game Of Life
Multiplayer Game Of Life with SignalR and ASP .NET OWIN

## Description
"Game Of Life Multiplayer" is a little game which is the famous ["Game of life"](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life) with a multiplayer feature. A click on a map will create a simple square (a cell). A new generation is created every seconds so you have to click faster as you can to make it the cell survive thanks to his neighbors. If you can't, no worries because a list of 4 patterns is available to insert one of them in a random place. Each player is assigned by a random color. When a cell reborn, a mix of colors is created from his neighbors.

To achive it, a good solution (for optimization) was to check each alive cells and its neighbors rather than all cells of the entire map.
If an user have bad internet connection, a timeout out of 30 secondes is in place before to be discnnected

## How to build/deploy/use
The solution has created with Visual Studio 2017 and it's composed of two projects (ASP .NET with OWIN) that you have to launch at the same time : The client project (GameOfLife.Client, which will send some requests) and the server project (GameOfLife.Server, which will received some requests). Don't forget to change the URL base if you want to publish it for a production (main.js and index.html)

## Link of the demo
http://kenium-001-site6.btempurl.com/client/www/

## Architectural choices
I've chosen to use ASP .NET and signalR because .NET is my favorite theme of developement and most of my knowledge about programming for the back-end is C#. For the front, simple HTML, CSS and Javascript/Jquery are used.

## Additional things To Do
Additionnal things possible to do is add more patterns, create responsive grid, create cells with click and move. Using SASS for the CSS :)

## Other projects
http://www.kevingermain.com/en/blog/2016/06/04/webimages-display-all-images-from-web-page-with-url/
http://www.kevingermain.com/en/blog/2017/05/01/poly-calcul-mobile-application-developed-with-the-xamarin-technology/
http://www.kevingermain.com/fr/blog/2015/02/23/kbreakout-petit-casse-brique-experimental-en-html5-avec-canvas/
http://www.kevingermain.com/fr/blog/2015/02/12/kpong-le-celebre-pong-en-html5/
http://www.kevingermain.com/fr/blog/2015/02/17/ksnake-le-jeu-snake-en-multijoueur-avec-html5-canvas-et-signalr/
