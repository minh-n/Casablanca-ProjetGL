// A simple templating method for replacing placeholders enclosed in curly braces.
if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

$(function () {

    var ticker = $.connection.notificationTickerMini, // the generated client-side hub proxy
        $notificationsTable = $('#notificationsTable'),
        $notificationsTableBody = $notificationsTable.find('tbody'),
        rowTemplate = '<tr data-type="{NotifType}"><td>{NotifType}</td><td>{NotifStatus}</td><td>{NotifContent}</td></tr>';

    function formatNotification(notification) {
        return $.extend(notification, {
            NotifStatus: notification.NotifStatus,
            NotifContent: notification.NotifContent
        });
    }

    function init() {
        ticker.server.GetNotifications().done(function (notifications) {            
            $notificationsTableBody.empty();
            $.each(notifications, function () {
                var notification = formatNotification(this);
                $notificationsTableBody.append(rowTemplate.supplant(notification));
            });
        });
    }

    // Add a client-side hub method that the server will call
    ticker.client.updateNotification = function (notification) {
        var displayNotification = formatNotification(notification),
            $row = $(rowTemplate.supplant(displayNotification));

        $notificationsTableBody.find('tr[data-type=' + notification.NotifType + ']')
            .replaceWith($row);
    }

    // Start the connection
    $.connection.hub.logging = true;
    $.connection.hub.start().done(init);

});