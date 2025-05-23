import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserService } from './services/auth/user.service';

export const roleGuard: CanActivateFn = (route, state) => {
  const router = inject(Router)
  const userService = inject(UserService);
  if (userService.checkUserRole() == "Admin" || userService.checkUserRole() == "Management User") {
    return true;
  }
  else {
    return router.createUrlTree(['/dashboard']);
  }
};
