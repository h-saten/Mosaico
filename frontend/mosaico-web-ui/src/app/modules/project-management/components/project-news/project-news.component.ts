import { Component, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { Store } from '@ngrx/store';
import { PERMISSIONS, ProjectPathEnum } from '../../constants';
import { selectPreviewProjectPermissions, selectProjectPreview } from '../../store';
import { Router } from '@angular/router';
import { DEFAULT_MODAL_OPTIONS, ErrorHandlingService, FormModeEnum } from 'mosaico-base';
import { ProjectArticle, TokenPageService } from 'mosaico-project';
import moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProjectNewsFormComponent } from './project-news-form/project-news-form.component';
import { BehaviorSubject } from 'rxjs';

interface SoialIcon {
  name: string;
  url: string;
}
@Component({
  selector: 'app-project-news',
  templateUrl: './project-news.component.html',
  styleUrls: ['./project-news.component.scss']

})
export class ProjectNewsComponent implements OnInit {

  socialIcons: SoialIcon[];
  articles: ProjectArticle[] = [];
  listRequestFinished$ = new BehaviorSubject<boolean>(false);

  private subs: SubSink = new SubSink();
  canEdit = false;

  FormModeEnum: typeof FormModeEnum = FormModeEnum;
  projectId = '';
  projectGuid = '';
  currentFormMode: FormModeEnum;
  showFormAdd = false;
  isMobileLayout = false;
  showSearchDetails = false;
  projectPathEnum: typeof ProjectPathEnum = ProjectPathEnum;
  totalPages = 0;
  totalItems = 0;
  rowsPerPage = 6;
  currentListSize = 0;

  constructor(
    private store: Store,
    private router: Router,
    private modalService: NgbModal,
    private tokenPageService: TokenPageService,
    private errorHandling: ErrorHandlingService,
    private toastr: ToastrService,
    private translateService: TranslateService
  ) { }

  isMobileAgent() {
    const agent = navigator.userAgent || navigator.vendor || (window as any).opera;
    return (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(agent) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(agent.substr(0, 4)));
  }

  ngOnInit(): void {
    this.subs.sink = this.store.select(selectPreviewProjectPermissions).subscribe((res) => {
      this.canEdit = res && res[PERMISSIONS.CAN_EDIT_DETAILS] === true;
    });
    this.getProject();
    this.getArticles(0);
    window.onresize = () => this.isMobileLayout = window.innerWidth <= 991;
    this.isMobileLayout = this.isMobileAgent();
    this.getSocailIcons();
  }

  getArticles(pageNumber: any) {
    this.subs.sink = this.tokenPageService.getArticles(pageNumber, this.rowsPerPage,this.projectGuid).subscribe((result) => {
      if (result) {
        this.totalItems = result.data.total;
        if (this.totalItems > 0) {
          this.totalPages = this.totalItems / this.rowsPerPage;

          if (this.currentListSize > 0 && pageNumber !== 0) {
            this.articles = this.articles.concat(result.data.entities);
          } else {
            this.articles = result.data.entities;
          }

          this.currentListSize = this.articles.length;

        }
        this.articles.forEach((article) => {
          if (article.date){
            article.date = moment(article.date).format('D MMM, YYYY');
          }
        });
      }
      this.listRequestFinished$.next(true);
    }, (error) => {
      this.errorHandling.handleErrorWithToastr(error);
    }, () => {
    });
  }

  onCopied(): void {
    this.toastr.success("Copied!");
  }

  private getProject(): void {
    this.subs.sink = this.store.select(selectProjectPreview).subscribe(data => {
      if (data) {
        this.projectId = data.projectId;
        this.projectGuid = data.project?.id;
      }
    });
  }

  openFormAdd(): void {
    const modalRef = this.modalService.open(ProjectNewsFormComponent, DEFAULT_MODAL_OPTIONS);
    modalRef.componentInstance.projectId = this.projectId;
    modalRef.componentInstance.currentFormMode = FormModeEnum.Add;
    this.subs.sink = modalRef.closed.subscribe((res?: any) => {
      this.getArticles(0);
    });
  }

  loadMoreResults(pageNumber: number): void {
    this.getArticles(pageNumber);
  }

  onShowSearchDetails() {
    this.showSearchDetails = true;
  }

  onUpdateArticlesList() {
    this.getArticles(0);
  }

  getSocailIcons() {
    this.socialIcons = [
      {
        name: 'Facebook',
        url: '/assets/media/footer/Facebook.svg'
      },
      {
        name: 'Instagram',
        url: '/assets/media/footer/Instagram.svg'
      },
      {
        name: 'Medium',
        url: '/assets/media/footer/Medium.svg'
      },
      {
        name: 'News',
        url: '/assets/media/footer/news.svg'
      },
      {
        name: 'Youtube',
        url: '/assets/media/footer/YouTube.svg'
      }
    ];
  }
}
