import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserService } from './services/auth/user.service';

export const roleGuard: CanActivateFn = (route, state) => {
  const router = inject(Router)
  const userService = inject(UserService);
  if (userService.checkUserRole() && (userService.checkUserRole()?.includes("Admin") || userService.checkUserRole()?.includes("Management_User") || userService.checkUserRole()?.includes("Contract_Approver"))) {
    return true;
  }
  else {
    return router.createUrlTree(['/dashboard']);
  }
};
