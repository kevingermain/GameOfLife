(function ($) {

    var settings, gameOfLife = {

        params: {
            canvas: $("#game"),
            context: $("#game")[0].getContext('2d'),
            width: $("#game").width(),
            height: $("#game").height(),
            sizeCell: 10
        },

        init: function (id) {
            settings = this.params;
            this.playerId = id;
            this.addEvents();
            this.draw();
        },

        addEvents: function () {
            settings.canvas.on('click', function (e) {
                var x = e.pageX - this.offsetLeft;
                var y = e.pageY - this.offsetTop;
                $.connection.gameOfLifeHub.server.onNewCell(gameOfLife.playerId, x, y);
            });

            $('button').on('click', function () {
                var valueOfSelect = $("#list-patterns select").val();
                $.connection.gameOfLifeHub.server.setPattern(gameOfLife.playerId, valueOfSelect);
            });
        },

        draw: function () {
            settings.context.clearRect(0, 0, settings.width, settings.height);
            settings.context.lineWidth = "1px";
            settings.context.strokeStyle = "#ddd";
            settings.context.beginPath();

            for (var i = 0; i < settings.height - 1; i += settings.sizeCell) {
                settings.context.moveTo(0, (i + 0.5));
                settings.context.lineTo(settings.width, (i + 0.5));
            }

            for (var j = 0; j < settings.width - 1; j += settings.sizeCell) {
                settings.context.moveTo(j, (0 + 0.5));
                settings.context.lineTo(j, (settings.height+ 0.5));
            }

            settings.context.closePath();
            settings.context.stroke();
        },
        drawCell: function (x, y, color) {
            settings.context.fillStyle = color;
            settings.context.fillRect(x, y, settings.sizeCell, settings.sizeCell);
            settings.context.stroke();
        }
    }

    $.connection.gameOfLifeHub.client.setListOfPlayers = function (players) {
        $("#list-players").html("");
        for (var i = 0; i < players.length; i++) {
            $("#list-players").append("<li><div class='square-player' style='background-color:" + players[i].Color + "'></div><span data-id='" + players[i].ConnectionId + "'>Player " + (i + 1) + "</span></li>");
        }
    };

    $.connection.gameOfLifeHub.client.setCells = function (cells) {
        if (cells) {
            for (var i = 0; i < cells.length; i++) {
                gameOfLife.drawCell(cells[i].X * 10, cells[i].Y * 10, cells[i].Color);
            }
        }
    };

    $.connection.hub.url = "http://localhost:56537/signalr";

    // Start of the hub connection
    $.connection.hub.start()
        .done(function () {
            console.log('Now connected, connection ID=' + $.connection.hub.id);
            gameOfLife.init($.connection.hub.id);
        })
        .fail(function() {
             console.log('Could not Connect!');
        });

})(jQuery);