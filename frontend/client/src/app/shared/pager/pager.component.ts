import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent {
  @Input() totalCount: number = 0;
  @Input() pageSize: number = 0;
  @Output() pageChanged: EventEmitter<number> = new EventEmitter<number>();

  onPageChanged(event: any) {
    this.pageChanged.emit(event.page);
  }
}
