import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './components/login/login/login.component';
import { RegisterComponent } from './components/register/register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DashboardComponent } from './components/dashboard/dashboard/dashboard.component';
import { SidebarComponent } from './components/sidebar/sidebar/sidebar.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { PopupModalComponent } from './helpers/popup-modal/popup-modal/popup-modal.component';
import { MatDialogModule } from '@angular/material/dialog';
import { UserComponent } from './components/user/user/user.component';
import { ChartComponent } from './helpers/chart/chart/chart.component';
import { WeightComponent } from './components/weight/weight/weight.component';
import { FoodComponent } from './components/food/food/food.component';
import { SleepComponent } from './components/sleep/sleep/sleep.component';
import { StartupComponent } from './components/startup/startup/startup.component';
import { ActivityComponent } from './components/activity/activity/activity.component';
import { MonthCounterComponent } from './components/month-counter/month-counter/month-counter.component';
import { HealthPreviewComponent } from './components/health-preview/health-preview/health-preview.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


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
    WeightComponent,
    FoodComponent,
    SleepComponent,
    StartupComponent,
    ActivityComponent,
    MonthCounterComponent,
    HealthPreviewComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    MatDialogModule,
    HttpClientModule,
    NgxChartsModule,
    FormsModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
