/// <reference lib="webworker" />

let ticks: number = 0;
let id: number = 0;

onmessage = function (event) {
	switch (event.data.func) {
		case 'start-counter':
			ticks = event.data.start ? event.data.start : ticks;

			postMessage({ticks: ticks++});
			
			id = this.setInterval(function () {
							postMessage({ticks: ticks++});
						}, 1000);
			
			break;
		case 'stop-counter':
			clearInterval(id);

			break;
		case 'reset-counter':
				ticks = 0;
	
				break;
	}
}