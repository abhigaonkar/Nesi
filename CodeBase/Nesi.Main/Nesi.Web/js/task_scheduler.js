var task_scheduler = {
	task: null,
	taskOverride: null,
	timeoutPerTask: 5000, // 5 seconds
	timeoutPerFullRun: 300000, // 5 minutes
	waitingForNextFullRun: false,
	iterationType: "task",
	currentTask: null,
	taskIterator: 1,
	endHour: 22,
	beginHour: 6,
	taskList: [],
	defaultTasks: [1,4,5,7],
	setDefaultTasks: true,
	getFirst: function()
		{
		task_scheduler.task	= $(document).find(".module").first();
		return task_scheduler.task;
		},
	run: function()
		{
		task_scheduler.setClickable();
		if(task_scheduler.setDefaultTasks)
			{
			task_scheduler.tasks.getList();
			task_scheduler.currentTask = setTimeout("task_scheduler.tasks.process()", task_scheduler.timeoutPerTask);
			}
		},
	setClickable: function()
		{
		var taskN = 1;
		$(document).find(".module").each(function(){
			$(this).attr("id", "module_"+taskN);
			if(task_scheduler.setDefaultTasks && task_scheduler.defaultTasks.includes(taskN))
				{
				$(this).find("input:checkbox").attr("checked", "checked");
				$(this).find(".title").addClass("on");
				}
			else
				{
				$(this).find(".title").addClass("off");
				}
			taskN++;

			var method = $(this).data("method");
			if(method === undefined)
			{
				$(this).find("input:checkbox").first().attr("disabled", "disabled");
				$(this).addClass("disabled");
				$(this).css({"cursor":"not-allowed"});
			}

			$(this).attr("onclick", "task_scheduler.module.click(this);");
			}
			);
		},
	module:
		{
		click: function(target)
			{
			var targetModule = $(target);
			var title = targetModule.find(".title");
			var cb = targetModule.find("input:checkbox");
			var status = targetModule.find(".status");
			if(cb.is(":enabled"))
				{
				var isChecked = cb.prop("checked");
				cb.prop("checked", !isChecked);
				if(!isChecked)
					{
					title.removeClass("off").addClass("on");
					if(task_scheduler.currentTask != null)
						{
						clearTimeout(task_scheduler.currentTask);
						}
					task_scheduler.currentTask = setTimeout("task_scheduler.tasks.process()", task_scheduler.timeoutPerTask);
					}
				else
					{
					title.removeClass("on").addClass("off");
					status.html("");
					}
				task_scheduler.tasks.getList();
				if(task_scheduler.taskList.length == 0)
					{
					if(task_scheduler.currentTask != null)
						{
						clearTimeout(task_scheduler.currentTask);
						}
					if(task_scheduler.timer.started != null)
						{
						clearInterval(task_scheduler.timer.started);
						}
					}
				}
			}
		},
	timer: 
	{
		timeBegan: null,
    	timeStopped: null,
		stoppedDuration: 0,
		started: null,
		currentElapsed: null,
		addTime: true,
		countdownDate: null,
		start: function(){

			if (task_scheduler.timer.timeBegan === null) {
				task_scheduler.timer.timeBegan = new Date();
			}

			if (task_scheduler.timer.timeStopped !== null) {
				if(task_scheduler.timer.addTime)
				{
				task_scheduler.timer.stoppedDuration += (new Date() - task_scheduler.timer.timeStopped);
				}
				else
				{
				task_scheduler.timer.stoppedDuration -= (new Date() - task_scheduler.timer.timeStopped);
				}
			}
			task_scheduler.timer.started = setInterval(task_scheduler.timer.clockRunning, 10);
			console.log(task_scheduler.waitingForNextFullRun);
		},
		clockRunning: function(){
			var currentTime = new Date();//;
			var timeElapsed = task_scheduler.timer.addTime 
								? new Date(currentTime - task_scheduler.timer.timeBegan - task_scheduler.timer.stoppedDuration)
								: new Date(task_scheduler.timer.countdownDate - currentTime - task_scheduler.timer.stoppedDuration);
			var hour = timeElapsed.getUTCHours();
			var min = timeElapsed.getUTCMinutes();
			var sec = timeElapsed.getUTCSeconds();
			var ms = timeElapsed.getUTCMilliseconds();
			if(isNaN(hour) || isNaN(min) || isNaN(sec) || isNaN(ms))
				{
				$(".timer .time").html("Error running timer");
				}
			else
				{
				if(task_scheduler.waitingForNextFullRun)
					{
					task_scheduler.timer.currentElapsed =  
						(hour > 9 ? hour : "0" + hour) + ":" + 
						(min > 9 ? min : "0" + min) + ":" + 
						(sec > 9 ? sec : "0" + sec) + "." + 
						(ms > 99 ? ms : ms > 9 ? "0" + ms : "00" + ms);
					}
				else
					{
					task_scheduler.timer.currentElapsed =  
						(min > 9 ? min : "0" + min) + ":" + 
						(sec > 9 ? sec : "0" + sec) + "." + 
						(ms > 99 ? ms : ms > 9 ? "0" + ms : "00" + ms);
					}
				var subtext = task_scheduler.iterationType == "task" ? "Till next task" : task_scheduler.iterationType == "batch" ?  "Till next run" : "Till next start @ 6:00am";
				$(".timer .time").html(task_scheduler.timer.currentElapsed);
				$(".timer .subtext").html(subtext);
				if(!task_scheduler.timer.addTime && task_scheduler.timer.countdownDate < currentTime)
					{
					task_scheduler.timer.reset();
					var tasks = task_scheduler.taskList.length > 0;
	    			$(".timer .time").html(tasks > 0 ? "Starting Tasks..." : "No tasks to start");
	    			if(tasks > 0)
	    				{
						task_scheduler.currentTask = setTimeout("task_scheduler.tasks.process()", task_scheduler.timeoutPerTask);
						}
					}
				}
			},
		reset: function(){
			clearInterval(task_scheduler.timer.started);
		    task_scheduler.timer.stoppedDuration = 0;
		    task_scheduler.timer.addTime = true;
		    task_scheduler.timer.timeBegan = null;
		    task_scheduler.timer.countdownDate = null;
		    task_scheduler.timer.timeStopped = null;
		    $(".timer .time").html("");
		    $(".timer .subtext").html("");
		},
		stop: function() {
			task_scheduler.timer.timeStopped = new Date();
		    $(".timer .time").html("");
		    $(".timer .subtext").html("");
			clearInterval(task_scheduler.timer.started);
		}
	},
	tasks:
	{
		getList: function()
			{
			task_scheduler.taskList = [];
			$(document).find(".module").each(function(){
				var checkbox = $(this).find("input:checkbox");
				var isChecked = checkbox.is(":checked");
				if(isChecked)
				{
					task_scheduler.taskList.push($(this));
				}
			});
			if(task_scheduler.taskList.length == 0)
				{
			    $(".timer .time").html("");
			    $(".timer .subtext").html("");
				}
			},
		process: function()
			{
			console.log("Running module - "+(task_scheduler.taskIterator-1));
			if(task_scheduler.taskList.length == 0)
			{
				return;
			}
			if(task_scheduler.waitingForNextFullRun)
				{
				task_scheduler.waitingForNextFullRun = false;
				}
			task_scheduler.task = task_scheduler.taskList[task_scheduler.taskIterator-1];
			var isFirst = task_scheduler.taskIterator == 1;
			var isLast = task_scheduler.taskList.length == task_scheduler.taskIterator;
			if(task_scheduler.task == undefined)
			{
				var x = 2;
			}
			var checkbox = task_scheduler.task.find("input:checkbox");
			var method = task_scheduler.task.attr("data-method");
			var isChecked = checkbox.is(":checked");
				
			if(method === undefined)
				{
				checkbox.attr("checked", false);
				return;
				}
			if(isChecked)
				{
				task_scheduler.task.find(".status").html("<div class='running'></div>");
				checkbox.attr("disabled", true);
				$.ajax(	{
					type:		"GET",
					url:		"invoice_service.aspx",
					data: 		{
								task: method,
								token: $(".token").val(),
								access_key: $(".access_key").val()
								},
					dataType:	"text",
					cache:		false,
					beforeSend:	
						function()
							{
							task_scheduler.timer.reset();
							task_scheduler.timer.start();
							},
					success: 
						function(resp)
							{
							task_scheduler.timer.stop();
							if(resp.includes("SUCCESS") && resp.includes("|"))
								{
								var resps = resp.split("|");
								checkbox.attr("disabled", false);
								var endDate = new Date();
								var records = resps[1] == 0 ? "No changes" : resps[1]+" record(s)";
								task_scheduler.task.find(".status").html("<div class='updated'>"+records+"</div><div class='lastran'>Last Ran<br/>"+endDate.toLocaleTimeString()+"</div><div class='total'>"+task_scheduler.timer.currentElapsed+"</div>");
								}
							else
								{
								checkbox.attr("disabled", false).prop("checked", false);
								console.log("Hit soft error on #"+task_scheduler.taskIterator+":"+resp);
								task_scheduler.handleError(resp);
								task_scheduler.tasks.getList();
								task_scheduler.taskIterator--;
								location.reload();
								}
							},
					error:
						function(xhr, textStatus, errorThrown)
							{
							checkbox.attr("disabled", false).prop("checked", false);
							console.log("Hit hard error on #"+task_scheduler.taskIterator+":"+resp);
							task_scheduler.handleError(errorThrown);
							task_scheduler.tasks.getList();
							task_scheduler.taskIterator--;
							},
					complete: function()
						{
						task_scheduler.timer.stop();
						page_obj.sleep(task_scheduler.timeoutPerTask).then(function(){
							task_scheduler.moveNext();
							});
						}
					});

				}
			}
	},
	handleError: function(err)
		{
		task_scheduler.task.find(".title").removeClass("on").addClass("off");
		if(err != "")
			{
			task_scheduler.task.find(".status").html("<button data-error=\""+encodeURI(err)+"\" type='button' onclick='task_scheduler.showError(this, event);'>Error</button>");
			}
		},
	showError: function(obj, e)
		{
		e.stopPropagation();
		$('<div />').html("<pre>"+decodeURI($(obj).data("error"))+"</pre>").dialog({autoOpen: true,
        resizable: false,
        modal: true,
        width:'auto'});
		},
	moveNext: function()
		{
		var d = new Date();
		var hour = d.getHours();
		var timeout = task_scheduler.taskIterator == task_scheduler.taskList.length
			? task_scheduler.timeoutPerFullRun 
			: task_scheduler.timeoutPerTask;
		task_scheduler.iterationType = timeout == task_scheduler.timeoutPerFullRun
											? hour >= task_scheduler.endHour
												? "daily"
												: "batch"
											: "task";
		console.log(task_scheduler.iterationType);
		if(timeout == task_scheduler.timeoutPerFullRun) // Full task iteration 
			{
			task_scheduler.waitingForNextFullRun = true;
			task_scheduler.timer.reset();
			task_scheduler.timer.timeBegan = new Date();
			task_scheduler.timer.addTime = false;
			console.log(hour);
			if(hour >= task_scheduler.endHour) // Stop at 10pm EST.
				{
				// figure out time in MS till 6:00am tomorrow
				var today = new Date();
				var tomorrow = new Date();
				tomorrow.setDate(today.getDate()+1);
				var targetDate = new Date(tomorrow.getUTCFullYear(), tomorrow.getUTCMonth(), tomorrow.getDate(), task_scheduler.beginHour, 0, 0, 0);
				var ms = targetDate-today;
				console.log("ms="+ms);
				console.log("targetDate="+targetDate);
				console.log("tomorrow="+tomorrow);
				task_scheduler.taskIterator = 1;
				task_scheduler.timeoutPerFullRun = ms;
				}

			task_scheduler.timer.countdownDate = d.getTime() + task_scheduler.timeoutPerFullRun;
			task_scheduler.timer.start();
			}
		else // Normal task iteration
			{
			task_scheduler.taskIterator = task_scheduler.taskIterator + 1 > task_scheduler.taskList.length 
											? 1 
											: task_scheduler.taskIterator + 1;
			console.log(timeout +" != "+task_scheduler.timeoutPerFullRun);
			if(timeout != task_scheduler.timeoutPerFullRun)
				{
				task_scheduler.currentTask = setTimeout("task_scheduler.tasks.process()", timeout);
				}
			}
		}
}