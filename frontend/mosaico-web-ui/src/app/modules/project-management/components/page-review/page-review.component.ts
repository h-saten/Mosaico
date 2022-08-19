import { Component, OnInit, OnDestroy, Sanitizer } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Store } from '@ngrx/store';
import { PageReview, TokenPageService } from 'mosaico-project';
import { SubSink } from 'subsink';
import { selectProjectPage } from '../../store/project.selectors';

@Component({
  selector: 'app-page-review',
  templateUrl: './page-review.component.html',
  styleUrls: ['./page-review.component.scss']
})
export class PageReviewComponent implements OnInit, OnDestroy {
  subs = new SubSink();
  reviews: PageReview[] = [];
  availableCategories: string[] = [];
  displayedReviews: PageReview[] = [];
  currentCategory: string;
  isLoaded = false;

  constructor(private store: Store, private sanitizer: DomSanitizer, private service: TokenPageService) { }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectProjectPage).subscribe((res) => {
      if (res && res.id) {
        this.getPageReviews(res.id);
      }
    });
  }

  getPageReviews(id: string): void {
    if (!this.isLoaded) {
      this.isLoaded = true;
      this.subs.sink = this.service.getPageReviews(id).subscribe((p) => {
        this.reviews = p.data;
        if (this.reviews && this.reviews.length > 0) {
          this.reviews = this.reviews.map((r) => {
            const duplicate = { ...r };
            if (duplicate.link && duplicate.link.length > 0) {
              duplicate.safeLink = this.sanitizer.bypassSecurityTrustResourceUrl(r.link);
              return duplicate;
            }
          });
          const categories = this.reviews.map((r) => r.category);
          this.availableCategories = categories.filter((e, i, self) => self.indexOf(e) === i);
          if (this.availableCategories && this.availableCategories.length > 0) {
            this.currentCategory = this.availableCategories[0];
            this.setCategory(this.currentCategory);
          }
        }
        this.isLoaded = true;
      }, (error) => {
        this.isLoaded = false;
      });
    }
  }

  setCategory(c: string): void {
    if (this.availableCategories.includes(c)) {
      this.currentCategory = c;
      this.displayedReviews = this.reviews.filter((r) => r.category === c && !r.isHidden && r.link?.length > 0);
    }
  }

}
