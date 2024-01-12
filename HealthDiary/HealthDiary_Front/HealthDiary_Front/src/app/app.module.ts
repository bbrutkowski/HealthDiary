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
import { WeatherInfoBarComponent } from './components/weather-info-bar/weather-info-bar/weather-info-bar.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    DashboardComponent,
    SidebarComponent,
    PopupModalComponent,
    WeatherInfoBarComponent  
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatDialogModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
