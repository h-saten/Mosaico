import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-report-account-stolen',
  templateUrl: './report-account-stolen.component.html',
  styleUrls: ['./report-account-stolen.component.scss']
})
export class ReportAccountStolenComponent implements OnInit {

  private userId: string;
  private code: string;
  public initiated:boolean=true;

  constructor(
    private route: ActivatedRoute,
    private authClient: AuthService,
  ) { }

  ngOnInit(): void {
    this.initiated=true;
    this.userId = this.route.snapshot.queryParamMap.get('userId');
    this.code = this.route.snapshot.queryParamMap.get('code');
    this.authClient.reportStolenAccount({id:this.userId,code:this.code}).subscribe((res)=>{
      setTimeout(() => {
        this.initiated=false;
      }, 3000);
    })
  }

}
