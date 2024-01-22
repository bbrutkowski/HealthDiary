import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './components/login/login/login.component';
import { RegisterComponent } from './components/register/register/register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { DashboardComponent } from './components/dashboard/dashboard/dashboard.component';
import { SidebarComponent } from './components/sidebar/sidebar/sidebar.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { PopupModalComponent } from './helpers/popup-modal/popup-modal/popup-modal.component';
import {MatDialogModule} from '@angular/material/dialog';
import { UserComponent } from './components/user/user/user.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { ChartComponent } from './helpers/chart/chart/chart.component';
import { WeightComponent } from './components/weight/weight/weight.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    DashboardComponent,
    SidebarComponent,
    PopupModalComponent,
    UserComponent,
    ChartComponent,
    WeightComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatDialogModule,
    HighchartsChartModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
