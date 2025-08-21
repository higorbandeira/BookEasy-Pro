import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router } from "@angular/router";
import { AuthService } from "./auth.service";
import { Observable } from "rxjs";

// auth.guard.ts
@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.authService.currentUser$.pipe(
      map(user => {
        const requiredRoles = route.data['roles'] as UserRole[];
        
        if (user && requiredRoles.some(role => user.role === role)) {
          return true;
        }
        
        this.router.navigate(['/login']);
        return false;
      })
    );
  }
}