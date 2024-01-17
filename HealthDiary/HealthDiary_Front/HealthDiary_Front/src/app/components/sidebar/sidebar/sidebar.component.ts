import { Component, OnDestroy, OnInit} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { PopupModalComponent } from 'src/app/helpers/popup-modal/popup-modal/popup-modal.component';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject();
  public userName?: string;

  constructor(private router: Router,
    private dialog: MatDialog) {}
    
  public ngOnInit(): void {
    this.getUserLogin();  
  }

  public ngOnDestroy(): void {
    this.ngUnsubscribe.next(undefined);
    this.ngUnsubscribe.complete();
  }

  private getUserLogin(): void {
    const loggedUser = localStorage.getItem('loggedUser') as string;
    this.userName = JSON.parse(loggedUser).login
  }

  public logout(): void {
    const dialogRef = this.dialog.open(PopupModalComponent, {
      data: {
        modalTitle: 'Logout',
        modalBody: 'Are you sure you want to logout?'
      }
    });
  
    dialogRef.afterClosed()
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe(() => {});

    dialogRef.componentInstance.confirmationEvent
    .pipe(takeUntil(this.ngUnsubscribe))
    .subscribe((result: boolean) => {
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
