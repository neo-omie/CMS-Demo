import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { UserService } from './services/auth/user.service';

export const authGuard: CanActivateFn = (route, state) => {
  const userService=inject(UserService);
  if(userService.isLoggedIn()){
    return true;
  }
  else{
    return false;
  }
};
