import {
    AfterViewInit,
    Component, ElementRef, Input, OnDestroy, Renderer2
} from '@angular/core';
import { Guid } from 'guid-typescript';

@Component({
    selector: 'timer',
    template: ' <ng-content></ng-content>'
})
export class TimerComponent implements OnDestroy {
    private tickCounter: number = 0;
    private worker: Worker;

    private seconds: number = 0;
    private minutes: number = 0;
    private hours: number = 0;

    @Input() startTime: number;

    constructor(private elt: ElementRef, private renderer: Renderer2) {
        // Initialization
        this.worker = new Worker(new URL('./timer.worker', import.meta.url), { name: Guid.create().toString() });
        this.startTime = 0;
    }


    ngOnDestroy() {        
        this.resetTimeout();
        this.worker.terminate();
    }

    /**
     * Start the timer
     */
    public start() {
        this.initVar();
        this.resetTimeout();
        this.computeTimeUnits();
        this.startTickCount();
    }

    /**
     * Resume the timer
     */
    public resume() {
        this.resetTimeout();

        this.startTickCount();
    }

    /**
     * Stop the timer
     */
    public stop() {
        this.clear();
    }

    /**
     * Reset the timer
     */
    public reset() {
        this.initVar();
        this.resetTimeout();
        this.clear();
        this.computeTimeUnits();
        this.renderText();
    }

    /**
     * Get the time information
     * @returns TimeInterface
     */
    public get() {
        return {
            seconds: this.seconds,
            minutes: this.minutes,
            hours: this.hours,
            tick_count: this.tickCounter
        };
    }

    /**
     * Initialize variable before start
     */
    private initVar() {
        this.startTime = this.startTime || 0;
        this.tickCounter = this.startTime;
    }

    /**
     * Reset timeout
     */
    private resetTimeout() {
        this.worker.postMessage({ func: "reset-counter" });
    }

    /**
     * Render the time to DOM
     */
    private renderText() {
        let outputText = "";

        outputText = this.hours.toString().padStart(2, '0') + ':';
        outputText += this.minutes.toString().padStart(2, '0') + ':';
        outputText += this.seconds.toString().padStart(2, '0');

        this.renderer.setProperty(this.elt.nativeElement, 'innerHTML', outputText);
    }

    private clear() {
        this.resetTimeout();
        this.worker.postMessage({ func: "stop-counter" });
    }

    /**
     * Compute each unit (seconds, minutes, hours, days) for further manipulation
     * @protected
     */
    protected computeTimeUnits() {
        this.seconds = Math.floor(this.tickCounter % 60);
        this.minutes = Math.floor((this.tickCounter / 60) % 60);
        this.hours = Math.floor(this.tickCounter / 3600);

        this.renderText();
    }

    /**
     * Start tick count, base of this component
     * @protected
     */
    protected startTickCount() {
        const that = this;

        this.worker.postMessage({ func: "start-counter", start: this.startTime });

        this.worker.onmessage = ({ data }) => {
            that.computeTimeUnits();

            that.tickCounter = data.ticks;
        };
    }
}
