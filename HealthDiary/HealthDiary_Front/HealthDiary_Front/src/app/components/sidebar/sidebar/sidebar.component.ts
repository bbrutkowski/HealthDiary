import { Component, TemplateRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { PopupModalComponent } from 'src/app/helpers/popup-modal/popup-modal/popup-modal.component';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {

  constructor(private router: Router,
    private dialog: MatDialog) {}

    public logout(): void {
      const dialogRef = this.dialog.open(PopupModalComponent, {
        data: {
          modalTitle: 'Logout',
          modalBody: 'Are you sure you want to logout?'
        }
      });
  
      dialogRef.afterClosed().subscribe(() => {});

      dialogRef.componentInstance.confirmationEvent.subscribe((result: boolean) => {
        if (result) {
          localStorage.clear();
          this.router.navigate(['/login']);        
        }
      });
    }

    public openUserProfile():void {
      const userRef = this.dialog.open(PopupModalComponent, {
        data: {
          modalTitle: 'User profile',
          modalBody: 'Do you want to go to the user`s profile?',
        }
      });

      userRef.afterClosed().subscribe(() => {});

      userRef.componentInstance.confirmationEvent.subscribe((result: boolean) => {
        if (result) {
          this.router.navigate(['/user']);        
        }
      });
    }
}
