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
							title: data.Collaborator.FirstName,  
							description: data.EventName,  
							start: moment(data.StartDate).format('YYYY-MM-DD'),  
							end: moment(data.EndDate).format('YYYY-MM-DD'),  
							backgroundColor: data.Color,  
						    borderColor: "#707e93"
                       });  
                    });  
  
                    callback(events);  
                }  
            });  
        },  
  
        eventRender: function (event, element)  
        {  
            element.qtip(  
            {  
                content: event.description  
            });  
        },  
  
        editable: false  
    });  
});  