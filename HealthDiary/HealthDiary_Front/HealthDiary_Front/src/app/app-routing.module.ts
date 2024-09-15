import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login/login.component';
import { RegisterComponent } from './components/register/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard/dashboard.component';
import { SidebarComponent } from './components/sidebar/sidebar/sidebar.component';
import { AuthService } from './services/auth.service/auth.service';
import { AuthGuard } from './guards/auth.guard';
import { UserComponent } from './components/user/user/user.component';
import { WeightComponent } from './components/weight/weight/weight.component';
import { SleepComponent } from './components/sleep/sleep/sleep.component';
import { FoodComponent } from './components/food/food/food.component';
import { StartupComponent } from './components/startup/startup/startup.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'user',
    component: UserComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'weight',
    component: WeightComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'sleep',
    component: SleepComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'food',
    component: FoodComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'startup',
    component: StartupComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
