import { MenuProjects } from './menu-projects.interface';
import { ProjectsPathEnum, ProjectsStatusEnum } from './../../constants';
import { Component, OnInit, Output, EventEmitter, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Data } from '@angular/router';
import { LanguageEnum } from 'src/app/modules/shared/models';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { SubSink } from 'subsink';
import { MenuProjectsService } from './menu-projects.service';

@Component({
  selector: 'app-menu-projects',
  templateUrl: './menu-projects.component.html',
  styleUrls: ['./menu-projects.component.scss']
})
export class MenuProjectsComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput') searchInput: ElementRef<HTMLInputElement>;

  @Output() statusEvent: EventEmitter<string> = new EventEmitter<string>();
  @Output() searchEvent: EventEmitter<string> = new EventEmitter<string>();

  projectsPathEnum: typeof ProjectsPathEnum = ProjectsPathEnum;
  projectsPath: ProjectsPathEnum;

  projectsStatusEnum: typeof ProjectsStatusEnum = ProjectsStatusEnum;
  timer: NodeJS.Timeout;
  classBtnMenuMobile = '';
  classFormSearchMobile = '';

  currentLang = '';
  languageEnum: typeof LanguageEnum = LanguageEnum;

  sub: SubSink = new SubSink();

  showDropDownMenu = false;

  categories: MenuProjects[] = [];
  currentPath: ProjectsStatusEnum;
  nameSelectedCategory = '';

  showSearchForm2 = false;

  searchText = '';
  previousSearchText = '';

  constructor(
    private activatedRoute: ActivatedRoute,
    private translateService: TranslateService,
    private menuProjectsService: MenuProjectsService
  ) { }

  ngOnInit(): void {

    this.getLanguage();
    this.getStatusAndPathFromRouting();

    this.showDropDownMenu = this.menuProjectsService.getShowDropDownMenu();
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  private getStatusAndPathFromRouting(): void {
    this.activatedRoute.data.subscribe((response: Data) => {
      const status: ProjectsStatusEnum = response['status'];
      if (status) {
        this.statusEvent.emit(status);
      }

      this.currentPath = response['path'];
      this.selectCategory(this.currentPath);
    });
  }

  private getLanguage(): void {
    this.currentLang = this.translateService.currentLang;

    this.getCategories();

    this.sub.sink = this.translateService.onLangChange
      .subscribe((langChangeEvent: LangChangeEvent) => {
        this.currentLang = langChangeEvent.lang;

        this.getCategories();
        this.selectCategory(this.currentPath);
      });
  }

  private getCategories(): void {
    this.categories = [
      {
        id: 1,
        path: this.projectsPathEnum.Main,
        title: this.translateService.instant('MARKETPLACE.menu.all')
      },
      {
        id: 2,
        path: this.projectsPathEnum.Sale,
        title: this.translateService.instant('MARKETPLACE.menu.sale')
      },
      {
        id: 3,
        path: this.projectsPathEnum.PreSale,
        title: this.translateService.instant('MARKETPLACE.menu.presale')
      },
      {
        id: 4,
        path: this.projectsPathEnum.PrivateSale,
        title: this.translateService.instant('MARKETPLACE.menu.privatesale')
      },
      {
        id: 5,
        path: this.projectsPathEnum.Upcoming,
        title: this.translateService.instant('MARKETPLACE.menu.upcoming')
      },
      {
        id: 7,
        path: this.projectsPathEnum.Closed,
        title: this.translateService.instant('MARKETPLACE.menu.completed')
      },
    ];
  }

  // from directive appMenuProjects
  numberHeightEvent(btnMenuheight: number): void {
    this.showDropDownMenu = window.innerWidth < 1080;
    this.menuProjectsService.setShowDropDownMenu(this.showDropDownMenu);
  }

  selectCategory(path: string): void {
    this.categories.find ((cat) => {
      if (cat.path === path) {
        this.nameSelectedCategory = cat.title;
      }
    });
  }

  // it is a copy of the menu placed in a space inaccessible to the visitor.
  // It is used to read the current size of the menu - to know at which resolution to change the menu from veritcal to horizontal mode and vice versa
  showSearchForm2Func(): void {
    this.showSearchForm2 = !this.showSearchForm2;
    if(this.showSearchForm2) {
      setTimeout(()=>{
        this.searchInput.nativeElement.focus();
      })
    }
  }

  onSearchChange(e: string): void {
    if(e !== this.previousSearchText) {
      clearTimeout(this.timer);
      this.previousSearchText = e;
      this.timer = setTimeout(() => {
        this.searchEvent.emit(this.searchText);
      }, 1000);
    }
  }


}
