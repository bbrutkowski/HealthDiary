import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private isAuthenticated = false;

    login() {
        debugger      
        this.isAuthenticated = true;
    }

    logout() {
        // Logika wylogowywania, resetowanie stanu zalogowania itp.
        this.isAuthenticated = false;
      }
    
      isLoggedIn() {
        return this.isAuthenticated;
      }
}