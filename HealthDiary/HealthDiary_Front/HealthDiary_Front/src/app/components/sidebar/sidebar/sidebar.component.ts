import { Component, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { ModalService } from 'src/app/services/modal-service/modal-service.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {

  constructor(private router: Router,
    private modalService: ModalService){}

  public logout(modalTemplate: TemplateRef<any>){
    this.modalService.open(modalTemplate, {body: 'Are you sure you want to logout?', title: 'Logout'}).subscribe(action => {
       localStorage.clear();
       this.router.navigate(['/login']);      
    });
  }
}
