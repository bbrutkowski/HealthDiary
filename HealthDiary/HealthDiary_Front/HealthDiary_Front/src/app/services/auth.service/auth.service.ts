import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class AuthService {

  public storeToken(): void{
    const loggedUserData = localStorage.getItem('loggedUser');
    const userData = JSON.parse(loggedUserData!);

    const token = userData.token;
    localStorage.setItem('token', token);
  }

  public getToken(): string | null{
    return localStorage.getItem('token');
  }  

  public isLoggedIn(): boolean{
    return !!localStorage.getItem('token');
  }
  
}