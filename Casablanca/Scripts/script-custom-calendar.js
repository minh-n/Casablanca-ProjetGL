$(document).ready(function ()  
{  
    $('#calendar').fullCalendar({  
        header:  
        {  
            left: 'prev,next today',  
            center: 'title',  
            right: 'month,agendaWeek,agendaDay'  
        },  
        buttonText: {  
            today: 'today',  
            month: 'month',  
            week: 'week',  
            day: 'day'  
        },  
  
        events: function (start, end, timezone, callback)  
        {  
            $.ajax({  
                url: '/Leave/GetCalendarData',  
                type: "GET",  
                dataType: "JSON",  
  
                success: function (result)  
                {  
                    var events = [];  
  
                    $.each(result, function (i, data)  
                    {  
                        events.push(  
                       {  
							title: data.Title,  
							description: data.Desc,  
							start: moment(data.Start_Date).format('YYYY-MM-DD HH:mm'),  
							end: moment(data.End_Date).format('YYYY-MM-DD HH:mm'),  
							backgroundColor: data.Color,  
							borderColor: data.Color,
							group: data.Group
                       });  
                    });  
  
                    callback(events);  
                }  
            });  
        },  
		
        eventRender: function (event, element, view)  
        {  
            element.qtip(  
            {  
                content: event.description  
			});
			return ['all', event.group].indexOf($('#cal_selector').val()) >= 0

        },  
		displayEventTime: false,
        editable: false  
    });  
});  
