import { Component, input } from '@angular/core';

@Component({
  selector: 'app-card-with-tags',
  imports: [],
  templateUrl: './card-with-tags.html',
})
export class CardWithTags {
  phrase = input('');
  author = input('');
  tags = input<string[]>([]);
}
