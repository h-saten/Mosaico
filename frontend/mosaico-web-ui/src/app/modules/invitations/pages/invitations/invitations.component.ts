import { Component, OnInit } from '@angular/core';
import { Invitation, InvitationsService } from 'mosaico-project';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-invitations',
  templateUrl: './invitations.component.html',
  styleUrls: ['./invitations.component.scss']
})
export class InvitationsComponent implements OnInit {
  invitations: Invitation[] = [];
  sub: SubSink = new SubSink();

  constructor(
    private invitationsService: InvitationsService
  ) { }

  ngOnInit(): void {
    this.loadInvitations();
  }

  loadInvitations(): void {
    this.sub.sink = this.invitationsService.getInvitations().subscribe((response) => {
      if(response && response.data){
        this.invitations = response.data.invitations;
      }
    });
  }

  accept(id: string): void {
    this.sub.sink = this.invitationsService.acceptInvitations(id).subscribe((response) => {
      if(response && response.data){
        alert('Invitation was accepted');
        this.loadInvitations();
      }
    })
  }

}
