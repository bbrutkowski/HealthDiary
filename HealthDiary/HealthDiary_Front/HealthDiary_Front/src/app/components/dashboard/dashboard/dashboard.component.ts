import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  public constructor(private authService: AuthService) {}

  ngOnInit(): void {
    debugger
    this.authService.storeToken();
  }

}
