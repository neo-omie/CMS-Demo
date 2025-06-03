import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function dateValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null;
    }

    const inputDate = new Date(value);
    const today = new Date();

    today.setHours(0, 0, 0, 0);

    if (isNaN(inputDate.getTime())) {
      return { invalidDate: 'Invalid date format' };
    }

    if (inputDate < today) {
      return { pastDate: 'Date must not be in the past' };
    }

    return null;
  };
}

export function dateRangeValidator(fromKey: string, tillKey: string): ValidatorFn {

  return (group: AbstractControl): ValidationErrors | null => {

    const from = group.get(fromKey)?.value;

    const till = group.get(tillKey)?.value;
 
    if (from && till && new Date(from) >= new Date(till)) {
      return { invalidDateRange: true };
    }
 
    return null;

  };

}

export function dateBetweenValidator(fromKey: string, currentKey: string,tillKey: string): ValidatorFn {

  return (group: AbstractControl): ValidationErrors | null => {

    const from = group.get(fromKey)?.value;
    const current = group.get(currentKey)?.value;
    const till = group.get(tillKey)?.value;
 
    if (from && current && till && (new Date(from) > new Date(current) || new Date(current) > new Date(till))) {
      return { invalidDateBetweenRange: true };
    }
 
    return null;

  };

}