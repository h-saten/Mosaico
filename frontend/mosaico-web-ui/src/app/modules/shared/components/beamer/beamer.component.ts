import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ConfigService, TranslationService } from 'mosaico-base';
import { UserInformation } from 'src/app/modules/user-management/models';
import { selectUserInformation } from '../../../user-management/store/user.selectors';
declare var Beamer: any;
declare var beamer_config: any;

@Component({
  selector: 'app-beamer',
  templateUrl: './beamer.component.html',
  styleUrls: ['./beamer.component.scss']
})
export class BeamerComponent implements OnInit {
  constructor(private configService: ConfigService, private store: Store, private translationService: TranslationService, private translate: TranslateService) { }
  shouldRender = false;
  user: UserInformation;
  lang = 'EN';

  ngOnInit(): void {
    this.store.select(selectUserInformation).subscribe((user) => {
      this.user = user;
    });
    this.lang = this.translationService.getSelectedLanguage();
    this.translate.onLangChange.subscribe((n) => {
      this.lang = n?.lang;
    });
  }

  render(): void {
    if (this.user) {
      
      beamer_config = {
        product_id: this.configService.getConfig()?.beamerProductKey,
        selector: null, /*Optional: Id, class (or list of both) of the HTML element to use as a button*/
        display: 'right', /*Optional: Choose how to display the Beamer panel in your site*/
        top: 0, /*Optional: Top position offset for the notification bubble*/
        right: 0, /*Optional: Right position offset for the notification bubble*/
        bottom: 0, /*Optional: Bottom position offset for the notification bubble*/
        left: 0, /*Optional: Left position offset for the notification bubble*/
        //button_position: 'bottom-right', /*Optional: Position for the notification button that shows up when the selector parameter is not set*/
        //icon: 'bell_lines', /*Optional: Alternative icon to display in the notification button*/
        language: this.lang, /*Optional: Bring news in the language of choice*/
        //filter: 'admin', /*Optional : Bring the news for a certain role as well as all the public news*/
        lazy: true, /*Optional : true if you want to manually start the script by calling Beamer.init()*/
        alert: false, /*Optional : false if you don't want to initialize the selector*/
        //delay : 0, /*Optional : Delay (in milliseconds) before initializing Beamer*/
        //embed : false, /*Optional : true if you want to embed and display the feed inside the element selected by the 'selector' parameter*/
        //mobile : true, /*Optional : false if you don't want to initialize Beamer on mobile devices*/
        //notification_prompt : 'sidebar', /*Optional : override the method selected to prompt users for permission to receive web push notifications*/
        //callback : your_callback_function, /*Optional : Beamer will call this function, with the number of new features as a parameter, after the initialization*/
        //onclick : your_onclick_function(url, openInNewWindow), /*Optional : Beamer will call this function when a user clicks on a link in one of your posts*/
        //onopen : your_onopen_function, /*Optional : Beamer will call this function when opening the panel*/
        //onclose : your_onclose_function, /*Optional : Beamer will call this function when closing the panel*/
        //---------------Visitor Information---------------
        user_firstname: this.user.firstName, /*Optional : Input your user firstname for better statistics*/
        user_lastname: this.user.lastName, /*Optional : Input your user lastname for better statistics*/
        user_email: this.user.email, /*Optional : Input your user email for better statistics*/
        user_id: this.user.id /*Optional : Input your user ID for better statistics*/
      };
      this.shouldRender = true;
      Beamer.update(beamer_config);
    }
    Beamer.init();
    Beamer.show();
  }

}
