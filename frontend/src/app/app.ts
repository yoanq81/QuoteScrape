import { httpResource } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { environment } from '../environments/environment.development';
import { CardWithTags } from "./card-with-tags/card-with-tags";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [CardWithTags]
})
export class App {
  protected readonly title = signal('frontend');

  quotes = httpResource<{
    phrase: string,
    author: string,
    tags: string[],
  }[]>(() => `${environment.apiUrl}/api/quotes`);
}
