import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit, OnDestroy {

  public constructor(){}

 
  ngOnInit(): void {
    const userdata = localStorage.getItem('loggedUser')
  }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

}
