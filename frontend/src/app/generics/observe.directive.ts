import { Directive, Output, EventEmitter, ElementRef, OnDestroy } from '@angular/core';

@Directive({
    selector: '[watchElement]'
})
export class DOMChangeDirective implements OnDestroy {
    private changes: MutationObserver;

    @Output()
    public DOMChange = new EventEmitter();

    constructor(private elementRef: ElementRef) {
		const element = this.elementRef.nativeElement;

        this.changes = new MutationObserver((mutations: MutationRecord[]) => {
            mutations.forEach((mutation: MutationRecord) => this.DOMChange.emit(mutation));
		});

        this.changes.observe(element, {
			childList: true
        });
	}
	
	ngOnDestroy(): void {
		this.changes.disconnect();
	}
}