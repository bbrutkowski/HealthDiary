import { Injectable } from "@angular/core";
import { UserAuthDto } from "src/app/models/user-auth-dto";

@Injectable({
    providedIn: 'root',
})
export class AuthService {

  public storeLoggedUser(userData: UserAuthDto): boolean {
    if (userData && userData.id && userData.token) {
      localStorage.setItem('name', userData.name);
      localStorage.setItem('role', userData.role.toString());
      localStorage.setItem('userId', userData.id.toString());
      localStorage.setItem('userToken', userData.token);
      return true;
    } 
    else console.error('Auth failed. Ensure id and token are provided.');
    return false;
  } 

  public isLoggedIn(): boolean{
    return !!localStorage.getItem('userToken');
  }
}